import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import HoneycombStandaloneCell from "../../components/Honeycomb/HoneycombStandaloneCell";
import HoneycombBoard from "../../components/Honeycomb/HoneycombBoard";
import { fetchGameById } from '../../data-access/game';
import { useAuth } from "../../infra/AuthProvider";

export const GamePage = () => {
  const { gameId } = useParams();
  const [game, setGame] = useState(null);
  const [loading, setLoading] = useState(true);
  const { user } = useAuth();
  const playerId = user?.id;

  useEffect(() => {
    async function fetchGame() {
      const gameData = await fetchGameById(gameId);
      setGame(gameData);
      setLoading(false);
    }
    fetchGame();
  }, [gameId]);

  if (loading) return <div>Loading...</div>;
  if (!game || !game.id) return <div style={{ color: '#888', textAlign: 'center', marginTop: '2rem' }}>Game not found.</div>;

  const playerBoard = game.playerBoards.find(board => board.playerId === playerId);
  const currentTile = game.nextTile;
  const canPlay = currentTile !== null && playerBoard !== undefined && playerBoard.spaces.some(space => space.placedTile?.id === currentTile.id);

  return (
    <div style={{ display: 'flex', flexDirection: 'column', flex: 1, boxSizing: 'border-box', alignItems: 'center', justifyContent: 'flex-start',
    }}>
      <div style={{display: 'flex',flexDirection: 'column',gap: 32,alignItems: 'center',justifyContent: 'flex-start',width: '100%',maxWidth: 900,margin: '0 auto',boxSizing: 'border-box',
      }}>
        <div style={{ display: 'flex', flexDirection: 'row', gap: 40, alignItems: 'flex-start', justifyContent: 'center', width: '100%' }}>
          {/* Score */}
          <div style={{ minWidth: 120, textAlign: 'center', padding: '1.2rem 1.5rem', borderRadius: 10, boxShadow: '0 1px 6px rgba(0,0,0,0.04)', fontWeight: 600, fontSize: '1.1rem', color: '#222', display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', height: 120 }}>
            <span style={{ fontSize: '0.9rem', color: '#888', fontWeight: 400, marginBottom: 4 }}>Score</span>
            <span style={{ fontSize: '2rem', fontWeight: 700 }}>{typeof playerBoard.score !== 'undefined' ? playerBoard.score : '-'}</span>
          </div>

          {/* Next Cell Preview (no text) */}
          <div style={{ minWidth: 120, textAlign: 'center', padding: '1.2rem 1.5rem', borderRadius: 10, boxShadow: '0 1px 6px rgba(0,0,0,0.04)', display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', height: 120 }}>
            {currentTile ? (
              <HoneycombStandaloneCell tile={currentTile} />
            ) : (
              <span style={{ color: '#bbb', fontSize: '1.2rem' }}>—</span>
            )}
          </div>
        </div>

        {/* Game Board below */}
        <div style={{ width: '50%', minWidth: 320, maxWidth: 480, display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center', borderRadius: 10, boxShadow: '0 1px 6px rgba(0,0,0,0.04)', padding: '1.2rem 1.5rem', minHeight: 400 }}>
          <HoneycombBoard 
            tiles={playerBoard.spaces} 
            playerId={playerId} 
            gameId={gameId} 
            canPlay={canPlay}
            refreshGame={async () => setGame(await fetchGameById(gameId))}
          />
        </div>
      </div>
    </div>
  );
}
