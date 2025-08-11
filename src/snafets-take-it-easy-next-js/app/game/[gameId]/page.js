import HoneycombCell from "@/components/HoneycombCell";
import HoneycombBoard from "@/components/HoneycombBoard";
import { fetchGameById } from '@/data-access/game';

export default async function GamePage({ params }) {
  const game = await fetchGameById((await params).gameId);
  const playerBoard = game?.playerBoards?.[0];
  const currentTile = game?.isCompleted ? null : game?.callerBag?.tiles?.[0];
  const gameId = game?.id;
  const playerId = playerBoard?.player?.id;
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
      {gameId ? <p>Welcome to the game with ID: {gameId}</p> : <p>Game not found.</p>}
      <div style={{ display: "flex", flexDirection: "row", alignItems: "flex-start", gap: 32, marginBottom: 32 }}>
        {typeof playerBoard.score !== 'undefined' && (
              <div style={{ fontWeight: "bold", marginBottom: "1em" }}>
                Score: {playerBoard.score}
              </div>
            )}
        {currentTile && (
          <div style={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
            
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
        {playerBoard && playerId && (
          <div style={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
            <h2>Player Board</h2>
            <HoneycombBoard 
              tiles={playerBoard.spaces} 
              playerId={playerId} 
              gameId={gameId} />
          </div>
        )}
      </div>
    </div>
  );
}
