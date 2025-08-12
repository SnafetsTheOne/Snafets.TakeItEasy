import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getLobbyById, startGameFromLobby } from '../../data-access/lobby';
import { getPlayerById } from '../../data-access/player';
import { useAuth } from "../../infra/AuthProvider";

export const LobbyPage = () => {
  const { lobbyId } = useParams();
  const navigate = useNavigate();
  const [lobby, setLobby] = useState(null);
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const { user } = useAuth();
  const myUserId = user?.id;
  const isInLobby = myUserId != null && lobby?.playerIds?.includes(myUserId);

  const handleJoinLobby = async () => {
    if (!lobbyId || !myUserId) return;
    await import('../../data-access/lobby').then(mod => mod.joinLobby(lobbyId, myUserId));
    // Refresh lobby after join
    const lobbyData = await getLobbyById(lobbyId);
    setLobby(lobbyData);
    if (lobbyData?.playerIds) {
      const userObjs = await Promise.all(
        lobbyData.playerIds.map(async (id) => {
          const player = await getPlayerById(id);
          return { id, name: player ? player.name : 'Unknown' };
        })
      );
      setUsers(userObjs);
    }
  };
  const handleLeaveLobby = async () => {
    if (!lobbyId || !myUserId) return;
    await import('../../data-access/lobby').then(mod => mod.leaveLobby(lobbyId, myUserId));
    // Refresh lobby after leave
    const lobbyData = await getLobbyById(lobbyId);
    setLobby(lobbyData);
    if (lobbyData?.playerIds) {
      const userObjs = await Promise.all(
        lobbyData.playerIds.map(async (id) => {
          const player = await getPlayerById(id);
          return { id, name: player ? player.name : 'Unknown' };
        })
      );
      setUsers(userObjs);
    }
  };

  useEffect(() => {
    async function fetchLobby() {
      const lobbyData = await getLobbyById(lobbyId);
      setLobby(lobbyData);
      if (lobbyData?.playerIds) {
        const userObjs = await Promise.all(
          lobbyData.playerIds.map(async (id) => {
            const player = await getPlayerById(id);
            return { id, name: player ? player.name : 'Unknown' };
          })
        );
        setUsers(userObjs);
      }
      setLoading(false);
    }
    fetchLobby();
  }, [lobbyId]);

  const handleStartGame = async (e) => {
    e.preventDefault();
    const game = await startGameFromLobby(lobbyId);
    navigate(`/game/${game.id}`);
  };

  if (loading) return <div style={{ color: '#888', textAlign: 'center', marginTop: '2rem' }}>Loading...</div>;
  if (!lobby) return <div style={{ color: '#888', textAlign: 'center', marginTop: '2rem' }}>Lobby not found.</div>;

  return (
    <div style={{ maxWidth: 480, margin: '2rem auto', padding: '0 1rem', background: '#fff', borderRadius: '10px', boxShadow: '0 2px 8px rgba(0,0,0,0.04)' }}>
      <h1 style={{ fontSize: '2rem', fontWeight: 600, marginBottom: '1.5rem', color: '#222', textAlign: 'center' }}>
        Lobby: {lobby.name}
      </h1>
      <h2 style={{ fontSize: '1.2rem', fontWeight: 500, marginBottom: '1rem', color: '#444' }}>Players</h2>
      <ul style={{ listStyle: 'none', padding: 0, marginBottom: '2rem' }}>
        {users.map((user) => (
          <li key={user.id} style={{ padding: '0.5rem 0', borderBottom: '1px solid #f0f0f0', color: '#222', fontSize: '1rem' }}>
            {user.name}
          </li>
        ))}
      </ul>
      {/* Join/Leave buttons */}
      {myUserId && !isInLobby && (
        <div style={{ display: 'flex', justifyContent: 'center', marginBottom: '1rem' }}>
          <button
            type="button"
            onClick={handleJoinLobby}
            style={{ padding: '0.75rem 2rem', fontSize: '1.1rem', border: '1px solid #4caf50', borderRadius: '6px', background: '#e8f5e9', color: '#222', fontWeight: 500, cursor: 'pointer', boxShadow: '0 1px 4px rgba(0,0,0,0.03)', transition: 'background 0.2s, border 0.2s' }}
          >
            Join Lobby
          </button>
        </div>
      )}
      {myUserId && isInLobby && (
        <div style={{ display: 'flex', justifyContent: 'center', marginBottom: '1rem' }}>
          <button
            type="button"
            onClick={handleLeaveLobby}
            style={{ padding: '0.75rem 2rem', fontSize: '1.1rem', border: '1px solid #f44336', borderRadius: '6px', background: '#ffebee', color: '#222', fontWeight: 500, cursor: 'pointer', boxShadow: '0 1px 4px rgba(0,0,0,0.03)', transition: 'background 0.2s, border 0.2s' }}
          >
            Leave Lobby
          </button>
        </div>
      )}
      <form onSubmit={handleStartGame} style={{ display: 'flex', justifyContent: 'center' }}>
        <button
          type="submit"
          disabled={!isInLobby}
          style={{ padding: '0.75rem 2rem', fontSize: '1.1rem', border: '1px solid #1976d2', borderRadius: '6px', background: !isInLobby ? '#e3f2fd' : '#1976d2', color: !isInLobby ? '#1976d2' : '#fff', fontWeight: 500, cursor: !isInLobby ? 'not-allowed' : 'pointer', boxShadow: '0 1px 4px rgba(25, 118, 210, 0.08)', transition: 'background 0.2s, border 0.2s', opacity: !isInLobby ? 0.6 : 1, }}
        >
          Start Game
        </button>
      </form>
    </div>
  );
};
