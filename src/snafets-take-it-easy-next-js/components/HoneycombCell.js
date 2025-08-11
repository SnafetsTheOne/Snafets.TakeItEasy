export default function HoneycombCell({ cell, tile, pts, strokeColor, strokeWidth, onCellClick }) {
  return (
    <g
      key={cell.i}
      tabIndex={0}
      onClick={() => onCellClick?.({ ...cell, tile })}
      style={{ cursor: onCellClick ? "pointer" : "default" }}
    >
      <polygon
        points={pts}
        fill={tile.label ?? "transparent"}
        stroke={strokeColor}
        strokeWidth={strokeWidth}
      />
      <text
        x={cell.x}
        y={cell.y + 4}
        textAnchor="middle"
        fontSize={14}
        fontWeight={600}
      >
        {tile.index ?? cell.i}
      </text>
    </g>
  );
}
