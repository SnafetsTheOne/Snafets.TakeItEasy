import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { fetchGameById } from '../../data-access/game';
import { getPlayerById } from '../../data-access/player';
import { useAuth } from "../../infra/AuthProvider";
import { useRealtime } from "../../infra/RealtimeProvider";
import { GameBoard } from "./GameBoard";

export const GamePage = () => {
  const { gameId } = useParams();
  const [game, setGame] = useState(null);
  const [loading, setLoading] = useState(true);
  const [boardIndex, setBoardIndex] = useState(0);
  const [playerNames, setPlayerNames] = useState({});
  const { user } = useAuth();
  const playerId = user?.id;
  const { on } = useRealtime();

  // reload game
  const reloadGame = async () => {
    const gameData = await fetchGameById(gameId);
    setGame(gameData);
  }

  // load page
  useEffect(() => {
    fetchGameById(gameId).then(gameData => {
      setGame(gameData);
      const ids = [gameData.myBoard?.playerId, ...(gameData.otherPlayerBoards || []).map(b => b.playerId)].filter(Boolean);
      const names = {};
      Promise.all(ids.map(async (id) => {
        try {
          const player = await getPlayerById(id);
          names[id] = player?.name || `Player ${id}`;
        } catch {
          names[id] = `Player ${id}`;
        }
      })).then(() => {
        setPlayerNames(names);
        setLoading(false);
      });
    });
  }, [gameId]);

  // subscribe to game updates
  useEffect(() => {
    on("gameUpdate", (p) => {
      if(p == gameId) {
        reloadGame();
      }
    });
  }, [gameId]);

  if (loading) return <div>Loading...</div>;
  if (!game?.id || !playerId) return <div style={{ color: '#888', textAlign: 'center', marginTop: '2rem' }}>Game not found.</div>;

  const playerBoard = game.myBoard;
  const currentTile = game.nextTile;
  const canPlay = game.myTurn;
  const isCompleted = game.isCompleted
  const otherGameboards = game.otherPlayerBoards || [];
  const allBoards = [
    { title: playerNames[playerBoard?.playerId] || 'My Game', playerBoard, currentTile, canPlay, playerId, gameId, refreshGame: reloadGame },
    ...otherGameboards.map((b, i) => ({
      title: playerNames[b.playerId] || `Player ${i+1}`,
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
