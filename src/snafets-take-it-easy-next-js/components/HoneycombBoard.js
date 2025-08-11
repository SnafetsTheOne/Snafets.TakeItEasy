"use client";
import HoneycombCell from "@/components/HoneycombCell";

import * as React from "react";

// A lightweight, reusable honeycomb board that renders from a 1D tiles array.
// Works great for the Take It Easy board (radius = 2 -> 19 cells).
// Drop this into a Next.js app (App Router or Pages) and import it.

// Tile: { label, color, data }

// --- Geometry helpers (pointy-top axial coords) --------------------------------
const SQRT3 = Math.sqrt(3);

function axialToPixel(q, r, size) {
  return {
    x: size * (SQRT3 * q + (SQRT3 / 2) * r),
    y: size * (1.5 * r),
  };
}

function hexPolygonPoints(cx, cy, size) {
  // pointy-top: angles = -30, 30, 90, 150, 210, 270 degrees
  const pts = [];
  for (let k = 0; k < 6; k++) {
    const angle = ((60 * k - 30) * Math.PI) / 180;
    pts.push(`${cx + size * Math.cos(angle)},${cy + size * Math.sin(angle)}`);
  }
  return pts.join(" ");
}

function rowLength(r, R) {
  // lengths: (2R+1) - |r| → e.g., R=2 => 3,4,5,4,3
  return 2 * R + 1 - Math.abs(r);
}

function qMin(r, R) {
  return -Math.min(R, R + r);
}
function qMax(r, R) {
  return Math.min(R, R - r);
}

function makeRowStarts(R) {
  const starts = [];
  let acc = 0;
  for (let r = -R; r <= R; r++) {
    starts.push(acc);
    acc += rowLength(r, R);
  }
  return starts; // length = 2R+1; total cells = last + last row length
}

// Cell: { i, q, r, s, x, y }

function layoutCells(tilesLen, R, size) {
  const rowStarts = makeRowStarts(R);
  const cells = [];
  for (let i = 0; i < tilesLen; i++) {
    // find rIndex such that rowStarts[rIndex] <= i < rowStarts[rIndex] + rowLength(r, R)
    let rIndex = 0;
    while (
      rIndex < rowStarts.length - 1 &&
      !(
        i >= rowStarts[rIndex] &&
        i < rowStarts[rIndex] + rowLength(-R + rIndex, R)
      )
    ) {
      rIndex++;
    }
    const r = -R + rIndex;
    const iInRow = i - rowStarts[rIndex];
    const q = qMin(r, R) + iInRow;
    const s = -q - r;
    const { x, y } = axialToPixel(q, r, size);
    cells.push({ i, q, r, s, x, y });
  }
  return cells;
}

function totalCells(R) {
  return (2 * R + 1) ** 2 - 3 * R * (R + 1);
}

// --- Component -----------------------------------------------------------------

export default function HoneycombBoard({ tiles }) {
  const radius = 2;
  const size = 40;
  const strokeColor = "#334155"; // slate-700
  const strokeWidth = 2;
  const onCellClick = () => {};
  const ariaLabel = "Honeycomb board";

  const expected = totalCells(radius);

  // Warn in dev if tiles length doesn't match the radius
  React.useEffect(() => {
    if (process.env.NODE_ENV !== "production" && tiles.length !== expected) {
      // eslint-disable-next-line no-console
      console.warn(
        `[HoneycombBoard] tiles.length = ${tiles.length}, but radius ${radius} expects ${expected} cells.`
      );
    }
  }, [tiles.length, expected, radius]);

  const cells = React.useMemo(
    () => layoutCells(tiles.length, radius, size),
    [tiles.length, radius, size]
  );

  // Compute extents for a tight SVG viewBox
  const { minX, maxX, minY, maxY } = React.useMemo(() => {
    const xs = cells.map((c) => c.x);
    const ys = cells.map((c) => c.y);
    return {
      minX: Math.min(...xs) - size,
      maxX: Math.max(...xs) + size,
      minY: Math.min(...ys) - size,
      maxY: Math.max(...ys) + size,
    };
  }, [cells, size]);

  const width = maxX - minX;
  const height = maxY - minY + 60;
  // Center point for rotation (in SVG/viewBox coords)
  const cy = (minX + maxX) / 2;
  const cx = (minY + maxY - 30) / 2;

  return (
    <div style={{ display: "grid", placeItems: "center" }}>
      <svg
        viewBox={`${minX} ${minY} ${width} ${height}`}
        width={Math.min(1080, width)}
        height={Math.min(1080, height)}
        role="img"
        aria-label={ariaLabel}
      >
        <g transform={`rotate(90 ${cx} ${cy})`}>
          {cells.map((c) => {
            const pts = hexPolygonPoints(c.x, c.y, size);
            const tile = tiles[c.i] ?? {};
            return (
              <HoneycombCell
                cell={c}
                tile={tile}
                pts={pts}
                strokeColor={strokeColor}
                strokeWidth={strokeWidth}
                onCellClick={onCellClick}
              />
            );
          })}
        </g>
      </svg>

      <div style={{ marginTop: 8, fontSize: 12, opacity: 0.7 }}>
        <span>
          Radius: {radius} • Cells: {expected} • Indexing: row‑major
          (top→bottom, left→right)
        </span>
      </div>
    </div>
  );
}

// --- Optional helpers you might want to reuse elsewhere ------------------------
export function indexToAxial(i, radius) {
  const rowStarts = makeRowStarts(radius);
  let rIndex = 0;
  while (
    rIndex < rowStarts.length - 1 &&
    !(
      i >= rowStarts[rIndex] &&
      i < rowStarts[rIndex] + rowLength(-radius + rIndex, radius)
    )
  ) {
    rIndex++;
  }
  const r = -radius + rIndex;
  const iInRow = i - rowStarts[rIndex];
  const q = qMin(r, radius) + iInRow;
  const s = -q - r;
  return { q, r, s };
}

export function axialToIndex(q, r, radius) {
  const rowStarts = makeRowStarts(radius);
  const rIndex = r + radius;
  return rowStarts[rIndex] + (q - qMin(r, radius));
}
