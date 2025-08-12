


import { useState } from "react";
import { makeMoveAction } from "./EmptyHoneycombCell.Actions";

export default function EmptyHoneycombCell({ cell, index, pts, playerId, gameId }) {
  const [loading, setLoading] = useState(false);

  const handleClick = async () => {
    setLoading(true);
    await makeMoveAction({ gameId, playerId, index });
    setLoading(false);
    // Optionally trigger a state update or re-fetch here
  };

  return (
    <g
      key={cell.i}
      tabIndex={0}
      onClick={handleClick}
      style={{ cursor: loading ? "wait" : "pointer" }}
    >
      <polygon
        points={pts}
        fill="transparent"
        stroke={"#334155"}
        strokeWidth={2}
      />
      <text
        transform={`rotate(-90 ${cell.x} ${cell.y})`}
        x={cell.x}
        y={cell.y + 4}
        textAnchor="middle"
        fontSize={14}
        fontWeight={600}
      >
        {index}
      </text>
    </g>
  );
}
