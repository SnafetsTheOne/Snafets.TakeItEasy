import styles from "./HoneycombCell.module.css";

export default function HoneycombCell({ cell, index, tile, pts }) {
  const onCellClick = () => {};
  if (tile) {
    const corners = [];
    for (const pt of pts.split(" ")) {
      const [x, y] = pt.split(",").map(Number);
      corners.push({ x, y });
    }
    const middles = [];
    let previous = corners[corners.length - 1];
    for (const pt of corners) {
      middles.push({ x: (pt.x + previous.x) / 2, y: (pt.y + previous.y) / 2 });
      previous = pt;
    }

    const id = tile.id;
    // Colors for each line
    const vertical_colors = { 1: "#888888", 5: "#0099ff", 9: "#ffe600" };
    const diag_left_colors = { 2: "#ffb6c1", 7: "#90ee90", 6: "#ff0000" };
    const diag_right_colors = { 3: "#ff69b4", 8: "#ffa500", 4: "#00cfff" };

    const verticalColor = vertical_colors[tile.vertical] || "#0000ff"; // blue
    const leftDiagonalColor =
      diag_left_colors[tile.leftDiagonal] || "#00ff00"; // green
    const rightDiagonalColor =
      diag_right_colors[tile.rightDiagonal] || "#ff00ff"; // magenta
    const strokeColor = "#334155";
    const strokeWidth = 2;
    const lineWidth = 10;
    const opacityLine = 0.7;
    console.log(pts);

    return (
      <g>
        <polygon
          points={pts}
          fill={"#e0e0e0"}
          stroke={strokeColor}
          strokeWidth={strokeWidth}
        />
        {/* Left diagonal */}
        <line
          x1={middles[2].x}
          y1={middles[2].y}
          x2={middles[5].x}
          y2={middles[5].y}
          stroke={leftDiagonalColor}
          strokeWidth={lineWidth}
          opacity={opacityLine}
        />
        <text
          transform={`rotate(-90 ${(middles[2].x * 4 + cell.x) / 5} ${
            (middles[2].y * 4 + cell.y) / 5
          })`}
          x={(middles[2].x * 4 + cell.x) / 5}
          y={(middles[2].y * 4 + cell.y) / 5}
          textAnchor="middle"
          fontSize={20}
          fontWeight={600}
        >
          {tile.leftDiagonal}
        </text>
        {/* Right diagonal */}
        <line
          x1={middles[0].x}
          y1={middles[0].y}
          x2={middles[3].x}
          y2={middles[3].y}
          stroke={rightDiagonalColor}
          strokeWidth={lineWidth}
          opacity={opacityLine}
        />
        <text
          transform={`rotate(-90 ${(middles[0].x * 4 + cell.x) / 5} ${
            (middles[0].y * 4 + cell.y) / 5
          })`}
          x={(middles[0].x * 4 + cell.x) / 5}
          y={(middles[0].y * 4 + cell.y) / 5}
          textAnchor="middle"
          fontSize={20}
          fontWeight={600}
        >
          {tile.rightDiagonal}
        </text>
        {/* Vertical */}
        <line
          x1={middles[1].x}
          y1={middles[1].y}
          x2={middles[4].x}
          y2={middles[4].y}
          stroke={verticalColor}
          strokeWidth={lineWidth}
          opacity={opacityLine}
        />
        <text
          transform={`rotate(-90 ${(middles[4].x + cell.x) / 2} ${
            (middles[4].y + cell.y) / 2
          })`}
          x={(middles[4].x + cell.x) / 2}
          y={(middles[4].y + cell.y) / 2}
          textAnchor="middle"
          fontSize={20}
          fontWeight={600}
        >
          {tile.vertical}
        </text>
      </g>
    );
  } else {
    return (
      <g
        key={cell.i}
        tabIndex={0}
        onClick={() => onCellClick?.({ ...cell, tile })}
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
}
