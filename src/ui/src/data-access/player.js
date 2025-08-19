// src/api/player.js
// Centralized, resilient API client for Player endpoints.
// - Normalizes base URL and joins paths safely
// - Unified request() with timeouts, retries (for transient failures), and rich errors
// - Optional ETag caching (304 -> returns cached JSON)
// - Credentials handling for cookie-based auth
// - JSDoc annotations for editor IntelliSense
// - CSRF header auto-injection if a meta[name="csrf-token"] exists

/* eslint-disable no-console */

/** @typedef {{status:number,statusText:string,url:string,body?:any}} HttpErrorInfo */

class HttpError extends Error {
  /** @param {string} message @param {HttpErrorInfo} info */
  constructor(message, { status, statusText, url, body }) {
    super(message);
    this.name = 'HttpError';
    this.status = status;
    this.statusText = statusText;
    this.url = url;
    this.body = body;
  }
}

/** Default request timeout (ms). */
const DEFAULT_TIMEOUT_MS = 15000;

/** Simple ETag cache: url -> { etag: string, data: any } */
const etagCache = new Map();

/** True if an HTTP status is likely transient (safe to retry). */
const isTransientStatus = (s) =>
  s === 408 || s === 429 || (s >= 500 && s <= 599);

/** Sleep helper. */
const wait = (ms) => new Promise((r) => setTimeout(r, ms));

/** Exponential backoff with jitter (ms). */
const backoff = (attempt) => {
  const base = 250 * 2 ** attempt; // 250, 500, 1000, 2000...
  const jitter = Math.random() * 120;
  return Math.min(4000, base + jitter);
};

/** Attempt to read a CSRF token from a meta tag. */
function readCsrfToken() {
  if (typeof document === 'undefined') return null;
  const tag = document.querySelector('meta[name="csrf-token"]');
  return tag?.getAttribute('content') || null;
}

/** Normalize a base URL string and ensure it ends with a single slash. */
function normalizeBaseUrl(raw) {
  if (!raw) return '';
  try {
    // Allow absolute or relative base. The URL constructor will resolve relative to current origin.
    const u = new URL(raw, (typeof window !== 'undefined' && window.location?.origin) || 'http://localhost');
    return u.toString().replace(/\/+$/, '/') /** ensure trailing slash */;
  } catch {
    return String(raw).replace(/\/+$/, '/') + '/';
  }
}

/** Safe URL join that avoids `//` issues and double "api". */
function joinUrl(base, path) {
  const clean = String(path).replace(/^\/+/, '');
  if (/^https?:\/\//i.test(clean)) return clean;
  const b = base || ((typeof window !== 'undefined' && window.location?.origin) || '');
  return new URL(clean, b).toString();
}

/** Determine the app's backend base URL from injected env or fallback. */
const baseUrl =
  normalizeBaseUrl(
    (typeof window !== 'undefined' && window.__ENV__ && window.__ENV__.BACKEND_URL) ||
      (typeof process !== 'undefined' && process.env && process.env.BACKEND_URL) ||
      ''
  );

/**
 * Core request helper.
 * @template T
 * @param {string} path - Relative or absolute path.
 * @param {Object} [opts]
 * @param {'GET'|'POST'|'PUT'|'PATCH'|'DELETE'} [opts.method='GET']
 * @param {any} [opts.body] - JSON payload or FormData/string/Blob.
 * @param {RequestCredentials} [opts.credentials] - e.g. 'include' for cookie auth.
 * @param {number} [opts.timeout=DEFAULT_TIMEOUT_MS]
 * @param {number} [opts.retries=0] - Retries for transient failures (GETs typically).
 * @param {AbortSignal} [opts.signal]
 * @param {boolean} [opts.cacheETag=false] - Enables If-None-Match/ETag caching for GET.
 * @param {Record<string,string>} [opts.headers]
 * @returns {Promise<T>}
 */
async function request(path, opts = {}) {
  const {
    method = 'GET',
    body,
    credentials,
    timeout = DEFAULT_TIMEOUT_MS,
    retries = 0,
    signal,
    cacheETag = false,
    headers = {},
  } = opts;

  const url = joinUrl(baseUrl, path);
  const controller = new AbortController();
  const signals = [controller.signal, signal].filter(Boolean);

  // Compose signals so external aborts cancel us, too.
  const composed = composeAbortSignal(signals);

  // Prepare headers
  const h = new Headers({
    Accept: 'application/json',
    'X-Requested-With': 'fetch',
    ...headers,
  });

  // Add CSRF token automatically if present and not already set.
  if (!h.has('X-CSRF-Token')) {
    const csrf = readCsrfToken();
    if (csrf) h.set('X-CSRF-Token', csrf);
  }

  const init = /** @type {RequestInit} */ ({
    method,
    headers: h,
    credentials,
    signal: composed,
  });

  // Content & body handling
  if (body !== undefined) {
    if (body instanceof FormData || typeof body === 'string' || body instanceof Blob) {
      init.body = body;
    } else {
      if (!h.has('Content-Type')) h.set('Content-Type', 'application/json');
      init.body = JSON.stringify(body);
    }
  }

  // ETag conditional request
  let cacheEntry;
  if (cacheETag && method === 'GET') {
    cacheEntry = etagCache.get(url);
    if (cacheEntry?.etag && !h.has('If-None-Match')) h.set('If-None-Match', cacheEntry.etag);
  }

  // Timeout
  const timer = setTimeout(() => controller.abort(new DOMException('Request timed out', 'AbortError')), timeout);

  let attempt = 0;
  try {
    // Retry loop (only on transient failures or network errors)
    // eslint-disable-next-line no-constant-condition
    while (true) {
      try {
        const res = await fetch(url, init);

        // 304 Not Modified -> serve cached data
        if (res.status === 304 && cacheEntry) {
          return /** @type {any} */ (cacheEntry.data);
        }

        const text = await res.text(); // Read once
        const isJson = res.headers.get('content-type')?.includes('application/json');
        const data = text ? (isJson ? safeJsonParse(text) : text) : null;

        if (!res.ok) {
          // Build rich error and maybe retry
          const info = {
            status: res.status,
            statusText: res.statusText,
            url,
            body: data,
          };
          if (attempt < retries && isTransientStatus(res.status) && method === 'GET') {
            attempt++;
            await wait(backoff(attempt));
            continue;
          }
          const msg = deriveErrorMessage(info);
          throw new HttpError(msg, info);
        }

        // Store ETag
        if (cacheETag && method === 'GET') {
          const etag = res.headers.get('etag');
          if (etag) etagCache.set(url, { etag, data });
        }

        return /** @type {any} */ (data);
      } catch (err) {
        // Network/Abort errors: optionally retry
        if (shouldRetryNetworkError(err) && attempt < retries && method === 'GET') {
          attempt++;
          await wait(backoff(attempt));
          continue;
        }
        throw err;
      }
    }
  } finally {
    clearTimeout(timer);
  }
}

/** Merge multiple AbortSignals into one. */
function composeAbortSignal(signals) {
  if (signals.length === 0) return undefined;
  if (signals.length === 1) return signals[0];
  const ctrl = new AbortController();
  const onAbort = () => ctrl.abort();
  signals.forEach((s) => {
    if (!s) return;
    if (s.aborted) ctrl.abort(s.reason);
    else s.addEventListener('abort', onAbort, { once: true });
  });
  return ctrl.signal;
}

function safeJsonParse(text) {
  try {
    return JSON.parse(text);
  } catch {
    return text; // return raw if it wasn't valid JSON
  }
}

function shouldRetryNetworkError(err) {
  // AbortError or TypeError (network) can be retriable for GETs
  if (!err) return false;
  const name = /** @type {any} */ (err).name || '';
  return name === 'AbortError' || err instanceof TypeError;
}

function deriveErrorMessage(info) {
  const { status, statusText, body } = info;
  const serverMsg =
    body && typeof body === 'object'
      ? body.message || body.error || body.title
      : typeof body === 'string'
      ? body.slice(0, 300)
      : null;
  return `HTTP ${status} ${statusText}${serverMsg ? ` – ${serverMsg}` : ''}`;
}

// -------------------------------
// Public Player API
// -------------------------------

/**
 * Get a player by ID.
 * @param {string|number} id
 * @param {{signal?:AbortSignal}} [options]
 */
export function getPlayerById(id, options) {
  const encoded = encodeURIComponent(String(id));
  return request(`api/Player/${encoded}`, {
    method: 'GET',
    credentials: 'include',
    retries: 2,
    timeout: 10000,
    cacheETag: true,
    ...(options || {}),
  });
}

/**
 * Create a new player (sign up).
 * @param {string} name
 * @param {string} passwordHash
 * @param {{signal?:AbortSignal}} [options]
 */
export function signUp(name, passwordHash, options) {
  return request('api/Player/signup', {
    method: 'POST',
    credentials: 'include',
    body: { Name: name, PasswordHash: passwordHash },
    timeout: 15000,
    ...(options || {}),
  });
}

/**
 * Login and establish session (cookie-based).
 * @param {string} name
 * @param {string} passwordHash
 * @param {{signal?:AbortSignal}} [options]
 */
export function loginPlayer(name, passwordHash, options) {
  return request('api/Player/login', {
    method: 'POST',
    credentials: 'include',
    body: { Name: name, PasswordHash: passwordHash },
    timeout: 15000,
    ...(options || {}),
  });
}

/**
 * Get the current authenticated player (via session cookie).
 * @param {{signal?:AbortSignal}} [options]
 */
export function me(options) {
  // Keep path casing as in original service (`api/player/me`)
  return request('api/player/me', {
    method: 'GET',
    credentials: 'include',
    retries: 2,
    timeout: 8000,
    cacheETag: true,
    ...(options || {}),
  });
}

/**
 * Logout the current player (invalidates session).
 * Accepts 200/204 with or without JSON.
 * @param {{signal?:AbortSignal}} [options]
 */
export async function logoutPlayer(options) {
  try {
    const res = await request('api/Player/logout', {
      method: 'POST',
      credentials: 'include',
      timeout: 8000,
      ...(options || {}),
    });
    return res ?? { ok: true };
  } catch (err) {
    // If server returns 204 No Content, some setups may still treat as ok.
    if (err instanceof HttpError && err.status === 204) return { ok: true };
    throw err;
  }
}

/**
 * Optional: expose low-level request for advanced callers.
 */
export const http = { request, HttpError };
