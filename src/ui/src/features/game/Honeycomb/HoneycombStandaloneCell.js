import HoneycombCell from "./HoneycombCell";

export default function HoneycombStandaloneCell({ tile }) {
  const sizeMultiplier = 0.80;
  const minX = 80;
  const minY = -40;
  const maxX = 160;
  const maxY = 40;
  const width = maxX - minX;
  const height = maxY - minY;
  const viewBox = `${minX} ${minY} ${width} ${height}`;

  return (
    <svg viewBox={viewBox} width={width} height={height} role="img">
      <g transform={`rotate(90 0 0)`}>
        <HoneycombCell
          cell={{ i: 0, x: 0, y: -120 }}
          index={0}
          tile={tile}
          pts={"34.6,-140 34.6,-100 0,-80 -34.6,-100 -34.6,-140 0,-160"}
        />
      </g>
    </svg>
  );
}
