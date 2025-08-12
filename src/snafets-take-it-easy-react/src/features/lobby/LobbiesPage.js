
import React from 'react';
import { fetchAllLobbies, joinLobby } from '../../data-access/lobby';
import LobbyCard from "./LobbyCard";

export const LobbiesPage = () => {
  const [lobbys, setLobbys] = React.useState([]);
  // Replace with actual user id logic
  const currentUserId = '00000000-0000-0000-0000-000000000001';

  React.useEffect(() => {
    fetchAllLobbies().then(setLobbys);
  }, []);

  const handleJoin = async (lobbyId) => {
    await joinLobby(lobbyId, currentUserId);
    // Refresh lobbys after join
    fetchAllLobbies().then(setLobbys);
  };

  return (
    <div style={{ maxWidth: 480, margin: '2rem auto', padding: '0 1rem' }}>
      <h1 style={{ fontSize: '2rem', fontWeight: 600, marginBottom: '2rem', color: '#222', textAlign: 'center' }}>Lobbys</h1>
      <div>
        {lobbys && lobbys.length > 0 ? (
          lobbys.map(lobby => {
            return (
              <LobbyCard
                key={lobby.id}
                lobby={lobby}
                currentUserId={currentUserId}
                handleJoin={handleJoin}
              />
            );
          })
        ) : (
          <div style={{ color: '#888', textAlign: 'center', marginTop: '2rem' }}>No lobbys found.</div>
        )}
      </div>
    </div>
  );
}

