const baseUrl = window.__ENV__?.BACKEND_URL ?? "";

// Centralized API calls for player
export async function getPlayerById(id) {
    const response = await fetch(`${baseUrl}api/Player/${id}`);
    if (!response.ok) throw new Error(`Failed to fetch player ${response.statusText}`);
    return await response.json();
}

export async function signUp(name, passwordHash) {
    const response = await fetch(`${baseUrl}api/Player/signup`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ Name: name, PasswordHash: passwordHash }),
    });
    if (!response.ok) throw new Error(`Failed to signup player ${response.statusText}`);
    return await response.json();
}

export async function loginPlayer(name, passwordHash) {
    const response = await fetch(`${baseUrl}api/Player/login`, {
        method: 'POST',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ Name: name, PasswordHash: passwordHash }),
    });
    if (!response.ok) throw new Error(`Failed to login player ${response.statusText}`);
    return await response.json();
}

export async function me() {
    const response = await fetch(`${baseUrl}api/player/me`, {
        method: 'GET',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json',
        },
    });
    if (!response.ok) throw new Error(`Failed to fetch player ${response.statusText}`);
    return await response.json();
}

export async function logoutPlayer() {
    const response = await fetch(`${baseUrl}api/Player/logout`, {
        method: 'POST',
        credentials: "include",
        headers: {
            'Content-Type': 'application/json',
        },
    });
    if (!response.ok) throw new Error(`Failed to logout player ${response.statusText}`);
    return await response.json();
}