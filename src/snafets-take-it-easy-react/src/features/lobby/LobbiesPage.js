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

  React.useEffect(() => {
    fetchAllLobbies().then(setLobbys);
  }, []);

  const handleJoin = async (lobbyId) => {
    await joinLobby(lobbyId, currentUserId);
    // Refresh lobbys after join
    fetchAllLobbies().then(setLobbys);
  };

  const [newLobbyName, setNewLobbyName] = React.useState("");

  const handleAddLobby = async (e) => {
    e.preventDefault();
    if (!newLobbyName.trim()) return;
    await createLobby(newLobbyName.trim(), currentUserId);
    setNewLobbyName("");
    fetchAllLobbies().then(setLobbys);
  };

  return (
    <div style={{ maxWidth: 480, margin: "2rem auto", padding: "0 1rem" }}>
      <h1
        style={{ fontSize: "2rem", fontWeight: 600, marginBottom: "2rem", color: "#222", textAlign: "center", }}
      >
        Lobbys
      </h1>
      <div>
        <form
          style={{ display: "flex", alignItems: "center", marginBottom: "1rem", gap: "0.5rem" }}
          onSubmit={handleAddLobby}
        >
          <input
            type="text"
            value={newLobbyName}
            onChange={(e) => setNewLobbyName(e.target.value)}
            placeholder="Enter lobby name"
            style={{ flex: 1, padding: "0.5rem 0.75rem", fontSize: "1rem", border: "1px solid #ddd", borderRadius: "6px", outline: "none", background: "#fafafa", color: "#222", boxSizing: "border-box" }}
          />
          <button
            type="submit"
            style={{ padding: "0.5rem 1rem", fontSize: "1rem", border: "1px solid #ddd", borderRadius: "6px", background: "#fff", color: "#222", fontWeight: 500, cursor: "pointer", transition: "background 0.2s, border 0.2s" }}
          >
            Add Lobby
          </button>
        </form>
        {lobbys && lobbys.length > 0 ? (
          lobbys.map((lobby) => {
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
          <div
            style={{ color: "#888", textAlign: "center", marginTop: "2rem" }}
          >
            No lobbys found.
          </div>
        )}
      </div>
    </div>
  );
};
