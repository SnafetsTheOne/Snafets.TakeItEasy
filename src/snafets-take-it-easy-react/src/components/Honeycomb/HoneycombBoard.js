
import HoneycombCell from "./HoneycombCell";
import EmptyHoneycombCell from "./EmptyHoneycombCell";

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

function layoutCells(tilesLen, R, size, startCorner = 'topLeft') {
  const rows = [];
  for (let r = -R; r <= R; r++) rows.push(r);

  const verticalFromTop = startCorner === 'topLeft' || startCorner === 'topRight';
  if (!verticalFromTop) rows.reverse();

  const cells = [];
  let i = 0;
  for (const r of rows) {
    const qs = [];
    for (let q = qMin(r, R); q <= qMax(r, R); q++) qs.push(q);

    const horizontalFromLeft = startCorner === 'topLeft' || startCorner === 'bottomLeft';
    if (!horizontalFromLeft) qs.reverse();

    for (const q of qs) {
      if (i >= tilesLen) break;
      const s = -q - r;
      const { x, y } = axialToPixel(q, r, size);
      cells.push({ i, q, r, s, x, y });
      i++;
    }
  }
  return cells;
}

function enumerateAxials(R, startCorner = 'topLeft') {
  const rows = [];
  for (let r = -R; r <= R; r++) rows.push(r);
  const verticalFromTop = startCorner === 'topLeft' || startCorner === 'topRight';
  if (!verticalFromTop) rows.reverse();
  const coords = [];
  for (const r of rows) {
    const qs = [];
    for (let q = qMin(r, R); q <= qMax(r, R); q++) qs.push(q);
    const horizontalFromLeft = startCorner === 'topLeft' || startCorner === 'bottomLeft';
    if (!horizontalFromLeft) qs.reverse();
    for (const q of qs) {
      const s = -q - r;
      coords.push({ q, r, s });
    }
  }
  return coords;
}

function totalCells(R) {
  return (2 * R + 1) ** 2 - 3 * R * (R + 1);
}

// --- Component -----------------------------------------------------------------

export default function HoneycombBoard({ tiles, gameId, playerId }) {
  const radius = 2;
  const size = 40;
  const startCorner = 'topLeft'
  const ariaLabel = "Honeycomb board";

  const expected = totalCells(radius);

  const cells = React.useMemo(
    () => layoutCells(tiles.length, radius, size, startCorner),
    [tiles.length, radius, size, startCorner]
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
  const cx = (minX + maxX) / 2 - 20;
  const cy = (minY + maxY) / 2 + 20;

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
            const tile = tiles[c.i];
            if(tile.placedTile) {
                return <HoneycombCell
                    key={c.i}
                    cell={c}
                    index={tile.index}
                    tile={tile.placedTile}
                    pts={pts}
                />
            }
            return <EmptyHoneycombCell
                key={c.i}
                cell={c}
                index={tile.index}
                pts={pts}
                playerId={playerId}
                gameId={gameId}
              />
            
          })}
        </g>
      </svg>
    </div>
  );
}

// --- Optional helpers you might want to reuse elsewhere ------------------------
export function indexToAxial(i, radius, startCorner = 'topLeft') {
  const coords = enumerateAxials(radius, startCorner);
  const { q, r, s } = coords[i];
  return { q, r, s };
}

export function axialToIndex(q, r, radius, startCorner = 'topLeft') {
  const coords = enumerateAxials(radius, startCorner);
  return coords.findIndex((c) => c.q === q && c.r === r);
}
