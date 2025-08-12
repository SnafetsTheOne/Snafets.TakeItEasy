'use client'

import { makeMoveAction } from './EmptyHoneycombCell.Actions';
import { useActionState } from "react";
import { startTransition } from "react";

export default function EmptyHoneycombCell({ cell, index, pts, playerId, gameId }) {
  const [state, action] = useActionState(makeMoveAction, {
    gameId: gameId,
    playerId: playerId,
    index: index,
  });

  return (
    <g
      key={cell.i}
      tabIndex={0}
      onClick={() => startTransition(() => action())}
      style={{ cursor: "pointer" }}
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
