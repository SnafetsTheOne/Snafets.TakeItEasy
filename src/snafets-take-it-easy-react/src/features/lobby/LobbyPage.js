import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getLobbyById, startGameFromLobby } from '../../data-access/lobby';
import { getPlayerById } from '../../data-access/player';

export const LobbyPage = () => {
  const { lobbyId } = useParams();
  const navigate = useNavigate();
  const [lobby, setLobby] = useState(null);
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);

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

  if (loading) return <div>Loading...</div>;
  if (!lobby) return <div>Lobby not found.</div>;

  return (
    <div>
      <h1>Lobby: {lobby.name}</h1>
      <h2>Players:</h2>
      <ul>
        {users.map((user) => (
          <li key={user.id}>{user.name}</li>
        ))}
      </ul>
      <form onSubmit={handleStartGame}>
        <button type="submit">Start Game</button>
      </form>
    </div>
  );
};
