import { useRouter } from "next/router";
import PropTypes from "prop-types";
import HoneycombBoard from "@/components/HoneycombBoard";
import HoneycombCell from "@/components/HoneycombCell";

export default function GamePage({ game }) {
  const playerBoard = game.playerBoards?.[0];
  // Example: current tile to be placed
  const currentTile = game.isCompleted ? null : game.callerBag?.tiles?.[0];
  console.log(currentTile);

  const minX = 60;
  const minY = -80;
  const maxX = 160;
  const maxY = 40;
  const width = maxX - minX;
  const height = maxY - minY;
  const viewBox = `${minX} ${minY} ${width} ${height}`;

  return (
    <div>
      <h1>Game Page</h1>
      <p>Welcome to the game with ID: {game.id}</p>
      {currentTile && (
        <div
          style={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            marginBottom: 24,
          }}
        >
          <h2>Current Tile</h2>
          <svg viewBox={viewBox} width={width} height={height} role="img">
            <g transform={`rotate(90 0 0)`}>
              <HoneycombCell
                cell={{ i: 0, x: 0, y: -120 }}
                index={0}
                tile={currentTile}
                pts="34.6,-140 34.6,-100 0,-80 -34.6,-100 -34.6,-140 0,-160"
              />
            </g>
          </svg>
        </div>
      )}
      {playerBoard && (
        <>
          <div style={{ fontWeight: "bold", marginBottom: "1em" }}>
            Score: {playerBoard.score}
          </div>
          <HoneycombBoard tiles={playerBoard.spaces} />
        </>
      )}
    </div>
  );
}

GamePage.propTypes = {
  game: PropTypes.shape({
    id: PropTypes.oneOfType([PropTypes.string, PropTypes.number]).isRequired,
    playerBoards: PropTypes.arrayOf(PropTypes.array).isRequired,
  }).isRequired,
};

export async function getServerSideProps({ params }) {
  // Fetch game data based on the ID
  const res = await fetch(`http://localhost:5124/api/Game/${params.id}`);
  const game = await res.json();
  console.log("game: " + JSON.stringify(game));

  return {
    props: {
      game,
    },
  };
}
