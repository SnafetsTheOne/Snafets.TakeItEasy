


import { useState } from "react";
import { postPlayerMove } from '../../data-access/game';

export default function EmptyHoneycombCell({ cell, index, pts, playerId, gameId, canPlay, refreshGame }) {
  const [loading, setLoading] = useState(false);

  const handleClick = async () => {
    if (loading || !canPlay) return; // Prevent action if already loading or can't play
    setLoading(true);
    await postPlayerMove(gameId, playerId, index);
    await refreshGame();
    setLoading(false);
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
