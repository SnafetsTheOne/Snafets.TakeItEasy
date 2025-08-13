import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { fetchGameById } from '../../data-access/game';
import { useAuth } from "../../infra/AuthProvider";
import { useRealtime } from "../../infra/RealtimeProvider";
import { GameBoard } from "./GameBoard";

export const GamePage = () => {
  const { gameId } = useParams();
  const [game, setGame] = useState(null);
  const [loading, setLoading] = useState(true);
  const [boardIndex, setBoardIndex] = useState(0);
  const { user } = useAuth();
  const playerId = user?.id;
  const { on } = useRealtime();

  const fetchGame = async () => {
    const gameData = await fetchGameById(gameId);
    setGame(gameData);
    setLoading(false);
  }

  useEffect(() => {
    fetchGame();
  }, [gameId]);

  useEffect(() => {
    on("gameUpdate", (p) => {
      if(p == gameId) {
        fetchGame();
      }
    });
  }, [gameId]);

  if (loading) return <div>Loading...</div>;
  if (!game?.id || !playerId) return <div style={{ color: '#888', textAlign: 'center', marginTop: '2rem' }}>Game not found.</div>;

  const playerBoard = game.myBoard;
  const currentTile = game.nextTile;
  const canPlay = game.myTurn;
  const otherGameboards = game.otherPlayerBoards || [];
  const allBoards = [
    { title: 'My Game', playerBoard, currentTile, canPlay, playerId, gameId, refreshGame: fetchGame },
    ...otherGameboards.map((b, i) => ({
      title: b.playerName || `Player ${i+1}`,
      playerBoard: b,
      currentTile: currentTile,
      canPlay: false,
      playerId: b.playerId,
      gameId,
      refreshGame: () => {},
    }))
  ];

  const handleSwipe = (direction) => {
    setBoardIndex(idx => {
      if (direction === 'left') return idx === 0 ? allBoards.length - 1 : idx - 1;
      if (direction === 'right') return idx === allBoards.length - 1 ? 0 : idx + 1;
      return idx;
    });
  };

  return (
    <div style={{ width: '100%', maxWidth: 900, margin: '0 auto', position: 'relative' }}>
      <div style={{ display: 'flex', flexDirection: 'row', alignItems: 'center', justifyContent: 'center', gap: 24 }}>
        <button onClick={() => handleSwipe('left')} style={{ fontSize: 24, padding: '0.5rem 1rem', borderRadius: 8, border: '1px solid #ccc', background: '#f5f5f5', cursor: 'pointer' }}>&lt;</button>
        <div style={{ flex: 1 }}>
          <GameBoard {...allBoards[boardIndex]} />
        </div>
        <button onClick={() => handleSwipe('right')} style={{ fontSize: 24, padding: '0.5rem 1rem', borderRadius: 8, border: '1px solid #ccc', background: '#f5f5f5', cursor: 'pointer' }}>&gt;</button>
      </div>
    </div>
  );
}
