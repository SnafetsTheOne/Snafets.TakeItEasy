import HoneycombStandaloneCell from "@/components/Honeycomb/HoneycombStandaloneCell";
import HoneycombBoard from "@/components/Honeycomb/HoneycombBoard";
import { fetchGameById } from '@/data-access/game';

export default async function GamePage({ params }) {
  const game = await fetchGameById((await params).gameId);
  const playerBoard = game.playerBoards[0];
  const currentTile = game.isCompleted ? null : game.callerBag.tiles[0];
  const gameId = game.id;
  const playerId = playerBoard.player.id;

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
            <h2>Next</h2>
            <HoneycombStandaloneCell 
              tile={currentTile} 
            />
          </div>
        )}
        {playerBoard && playerId && (
          <div style={{ display: "flex", flexDirection: "column", alignItems: "center" }}>
            <HoneycombBoard 
              tiles={playerBoard.spaces} 
              playerId={playerId} 
              gameId={gameId} 
            />
          </div>
        )}
      </div>
    </div>
  );
}
