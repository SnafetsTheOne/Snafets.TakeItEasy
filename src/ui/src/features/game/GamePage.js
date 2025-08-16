import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { fetchGameById } from '../../data-access/game';
import { getPlayerById } from '../../data-access/player';
import { useAuth } from "../../infra/AuthProvider";
import { useRealtime } from "../../infra/RealtimeProvider";
import { GameBoard } from "./GameBoard";

export const GamePage = () => {
  const [game, setGame] = useState(null);
  const [loading, setLoading] = useState(true);
  const [boardIndex, setBoardIndex] = useState(0);
  const [playerNames, setPlayerNames] = useState({});
  const [showPopup, setShowPopup] = useState(false);

  const { gameId } = useParams();
  const { on } = useRealtime();
  const { user } = useAuth();

  // reload game
  const reloadGame = async () => {
    const gameData = await fetchGameById(gameId);
    setGame(gameData);
  }
  
  const handleSwipe = (direction) => {
    setBoardIndex(idx => {
      if (direction === 'left') return idx === 0 ? boards.length - 1 : idx - 1;
      if (direction === 'right') return idx === boards.length - 1 ? 0 : idx + 1;
      return idx;
    });
  };

  // load page
  useEffect(() => {
    fetchGameById(gameId).then(gameData => {
      setGame(gameData);
      const ids = gameData.boards.map(b => b.playerId);
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

  // Check if game is completed
  useEffect(() => {
    if (game?.isCompleted) {
      setShowPopup(true);
    } else {
      setShowPopup(false);
    }
  }, [game]);

  if (loading) return (
    <div style={{ 
      display: 'flex', 
      justifyContent: 'center', 
      alignItems: 'center', 
      minHeight: '50vh',
      fontSize: '1.2rem',
      color: '#666'
    }}>
      Loading...
    </div>
  );
  
  if (!game?.id) return (
    <div style={{ 
      color: '#888', 
      textAlign: 'center', 
      marginTop: '2rem',
      padding: '0 1rem'
    }}>
      Game not found.
    </div>
  );

  const scores = game.boards
                  .map(b => ({ id: b.playerId, name: playerNames[b.playerId], score: b.score }))
                  .sort((a, b) => b.score - a.score);
  const boards = game.boards.map((b, i) => ({
      title: playerNames[b.playerId] || `Player ${i+1}`,
      gameId,
      board: b,
      currentTile: game.currentTile,
      reloadGame: reloadGame,
      isUser: b.playerId == user?.id
    }));

  return (
    <div style={{ 
      width: '100%', 
      maxWidth: 900, 
      margin: '0 auto', 
      position: 'relative',
      padding: '0 1rem'
    }}>
      {/* Game holder */}
      <div style={{ 
        display: 'flex', 
        flexDirection: 'column', 
        alignItems: 'center', 
        gap: '1rem',
        marginBottom: '1rem'
      }}>
        {/* Player selector */}
        <div style={{
          display: 'flex',
          gap: '0.5rem',
          flexWrap: 'wrap',
          justifyContent: 'center'
        }}>
          {boards.map((board, idx) => (
            <button
              key={idx}
              onClick={() => setBoardIndex(idx)}
              style={{
                padding: '0.5rem 1rem',
                borderRadius: '20px',
                border: boardIndex === idx ? '2px solid #6366f1' : '1px solid #ddd',
                background: boardIndex === idx ? '#6366f1' : '#fff',
                color: boardIndex === idx ? '#fff' : '#333',
                fontSize: '0.9rem',
                cursor: 'pointer',
                minHeight: '44px'
              }}
            >
              {board.title}
            </button>
          ))}
        </div>

        {/* GameBoard */}
        <div style={{ 
          display: 'flex', 
          flexDirection: 'row', 
          alignItems: 'center', 
          justifyContent: 'center', 
          gap: 24 
        }}>
          <div style={{ flex: 1 }}>
            <GameBoard {...boards[boardIndex]} />
          </div>
        </div>
      </div>

      {/* Game completion popup */}
      {showPopup && (
        <>
          <div style={{ 
            position: "fixed", 
            top: 0, 
            left: 0, 
            right: 0, 
            bottom: 0, 
            backgroundColor: "rgba(0,0,0,0.6)", 
            display: "flex", 
            justifyContent: "center", 
            alignItems: "center", 
            zIndex: 1000,
            padding: '0.5rem'
          }}>
            <div style={{ 
              background: "linear-gradient(135deg, #f8fafc 0%, #e0e7ff 100%)", 
              padding: "1.5rem 5rem 1rem 5rem", 
              borderRadius: "16px", 
              boxShadow: "0 8px 32px rgba(0,0,0,0.18)", 
              textAlign: "center", 
              border: "2px solid #6366f1" 
            }}>
              <h2 style={{ 
                fontSize: "1.75rem", 
                marginBottom: "0.75rem", 
                color: "#6366f1" 
              }}>
                Game Over
              </h2>
              <h3 style={{ 
                fontSize: "1.1rem", 
                marginBottom: "1rem", 
                color: "#334155" 
              }}>
                Player Scores
              </h3>
              <ul style={{ 
                listStyle: "none", 
                padding: 0, 
                marginBottom: "1.5rem" 
              }}>
                {scores.map(({ id, name, score }) => (
                  <li key={id} style={{ 
                    fontSize: "1rem", 
                    margin: "0.25rem 0", 
                    color: "#475569", 
                    fontWeight: "bold" 
                  }}>
                    {name}: <span style={{ color: "#6366f1" }}>{score}</span>
                  </li>
                ))}
              </ul>
              <button
                onClick={() => setShowPopup(false)}
                style={{ 
                  fontSize: "1rem", 
                  padding: "0.75rem 1.5rem", 
                  borderRadius: "8px", 
                  border: "none", 
                  background: "#6366f1", 
                  color: "#fff", 
                  cursor: "pointer", 
                  boxShadow: "0 2px 8px rgba(99,102,241,0.15)",
                  minHeight: '44px',
                  width: '100%',
                  maxWidth: '150px'
                }}
              >
                Close
              </button>
            </div>
          </div>
        </>
      )}
    </div>
  );
}
export default GamePage;
