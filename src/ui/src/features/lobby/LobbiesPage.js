import React from "react";
import {
  fetchAllLobbies,
  joinLobby,
  createLobby,
} from "../../data-access/lobby";
import LobbyCard from "./LobbyCard";
import { useAuth } from "../../infra/AuthProvider";

export const LobbiesPage = () => {
  const [lobbys, setLobbys] = React.useState([]);
  const { user } = useAuth();

  const currentUserId = user?.id;

  const reloadPage = () => {
    fetchAllLobbies().then(setLobbys);
  };

  React.useEffect(() => {
    reloadPage();
  }, []);

  const handleJoin = async (lobbyId) => {
    await joinLobby(lobbyId, currentUserId);
    // Refresh lobbys after join
    reloadPage();
  };

  const [newLobbyName, setNewLobbyName] = React.useState("");

  const handleAddLobby = async (e) => {
    e.preventDefault();
    if (!newLobbyName.trim()) return;
    await createLobby(newLobbyName.trim(), currentUserId);
    setNewLobbyName("");
    reloadPage();
  };

  return (
    <div className="container" style={{ 
      maxWidth: 600, 
      margin: "2rem auto", 
      padding: "1rem"
    }}>
      <h1 style={{ 
        fontSize: "2.5rem", 
        fontWeight: 600, 
        marginBottom: "2rem", 
        color: "#222", 
        textAlign: "center" 
      }}>
        Lobbies
      </h1>
      <div>
        {user ? (
          <form
            style={{ 
              display: "flex", 
              flexDirection: "column",
              gap: "1rem",
              marginBottom: "2rem",
              padding: "1.5rem",
              background: "#f8fafc",
              borderRadius: "12px",
              border: "1px solid #e2e8f0"
            }}
            onSubmit={handleAddLobby}
          >
            <div>
              <label htmlFor="lobbyName" style={{
                display: "block",
                marginBottom: "0.5rem",
                fontWeight: 500,
                color: "#374151",
                fontSize: "0.9rem"
              }}>
                Lobby Name
              </label>
              <input
                id="lobbyName"
                type="text"
                value={newLobbyName}
                onChange={(e) => setNewLobbyName(e.target.value)}
                placeholder="Enter lobby name"
                style={{ 
                  width: "100%",
                  padding: "0.875rem 1rem", 
                  fontSize: "1rem", 
                  border: "1px solid #d1d5db", 
                  borderRadius: "8px", 
                  outline: "none", 
                  background: "#fff", 
                  color: "#222", 
                  boxSizing: "border-box",
                  minHeight: "44px"
                }}
                required
              />
            </div>
            <button
              type="submit"
              className="btn btn-primary"
              style={{ 
                width: "100%",
                fontSize: "1.1rem",
                fontWeight: 600
              }}
            >
              Create Lobby
            </button>
          </form>
        ) : (
          <div style={{ 
            color: "#888", 
            background: '#fff', 
            padding: '1.5rem', 
            borderRadius: '12px', 
            textAlign: "center", 
            marginBottom: "2rem",
            border: "1px solid #e2e8f0"
          }}>
            Sign Up to Play
          </div>
        )}
        {lobbys && lobbys.length > 0 ? (
          <div style={{ display: "flex", flexDirection: "column", gap: "1rem" }}>
            {lobbys.map((lobby) => {
              return (
                <LobbyCard
                  key={lobby.id}
                  lobby={lobby}
                  currentUserId={currentUserId}
                  handleJoin={handleJoin}
                />
              );
            })}
          </div>
        ) : (
          <div style={{ 
            color: "#888", 
            background: '#fff', 
            padding: '2rem', 
            borderRadius: '12px', 
            textAlign: "center", 
            marginTop: "2rem",
            border: "1px solid #e2e8f0"
          }}>
            No lobbies found.
          </div>
        )}
      </div>
    </div>
  );
};
