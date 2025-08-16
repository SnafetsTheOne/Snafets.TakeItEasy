import React from "react";
import {
  fetchAllLobbies,
  joinLobby,
  createLobby,
} from "../../data-access/lobby";
import LobbyCard from "./LobbyCard";
import CreateLobbyCard from "./CreateLobbyCard";
import { useAuth } from "../../infra/AuthProvider";
import {
  cardStyle,
  cardNameStyle,
  cardIdStyle,
  head1,
  head2,
  verticalContainer,
  verticalContainerItem,
} from "../../infra/css";

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
    <div
      style={{
        ...verticalContainer,
        maxWidth: 500,
        margin: "auto",
      }}
    >
      <h1
        style={{
          ...verticalContainerItem,
          ...head1
        }}
      >
        Lobbies
      </h1>
      <div style={{ ...verticalContainerItem, ...verticalContainer }}>
        {/* Lobby creation form */}
        <CreateLobbyCard 
          user={user}
          handleAddLobby={handleAddLobby} 
          newLobbyName={newLobbyName} 
          setNewLobbyName={setNewLobbyName} 
        />
        {/* Lobby list */}
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
            style={{
              ...verticalContainerItem,
              color: "#888",
              background: "#fff",
              padding: "2rem",
              borderRadius: "12px",
              textAlign: "center",
              marginTop: "2rem",
              border: "1px solid #e2e8f0",
            }}
          >
            No lobbies found.
          </div>
        )}
      </div>
    </div>
  );
};
