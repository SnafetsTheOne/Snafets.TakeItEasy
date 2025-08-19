import React from "react";
import {
  fetchAllLobbies,
  joinLobby,
  createLobby,
  getLobbyById,
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
import { useRealtime } from "../../infra/RealtimeProvider";

export const LobbiesPage = () => {
  const [lobbys, setLobbys] = React.useState([]);
  const { user } = useAuth();
  const { on } = useRealtime();

  const currentUserId = user?.id;

  const reloadPage = () => {
    fetchAllLobbies().then(setLobbys);
  };

  React.useEffect(() => {
    reloadPage();
  }, []);

  // Function to update the lobby list with a given id and lobby object
  // Expects lobbyObj=null when lobby not found (deleted)
  const updateLobbyList = (prevLobbys, lobbyId, lobbyObj) => {
    if (lobbyObj == null) {
      // Remove the lobby by id
      return prevLobbys.filter(l => l.id !== lobbyId);
    }
    const idx = prevLobbys.findIndex(l => l.id === lobbyId);
    if (idx !== -1) {
      const newLobbys = [...prevLobbys];
      newLobbys[idx] = lobbyObj;
      return newLobbys;
    } else {
      return [...prevLobbys, lobbyObj];
    }
  };

  // Subscribe to LobbyUpdate SignalR event
  React.useEffect(() => {
    const unsubscribe = on("lobbyUpdate", async (updatedLobbyId) => {
      let lobbyObj = null;
      try {
        lobbyObj = await getLobbyById(updatedLobbyId);
      } catch (e) {
        lobbyObj = null;
      }
      setLobbys((prevLobbys) => updateLobbyList(prevLobbys, updatedLobbyId, lobbyObj));
    });
    return unsubscribe;
  }, [on]);

  const handleJoin = async (lobbyId) => {
    const newLobby = await joinLobby(lobbyId, currentUserId);
    setLobbys((prevLobbys) => updateLobbyList(prevLobbys, lobbyId, newLobby));
  };

  const [newLobbyName, setNewLobbyName] = React.useState("");

  const handleAddLobby = async (e) => {
    e.preventDefault();
    if (!newLobbyName.trim()) return;
    const newLobby = await createLobby(newLobbyName.trim(), currentUserId);
    setNewLobbyName("");
    setLobbys((prevLobbys) => updateLobbyList(prevLobbys, newLobby.id, newLobby));
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
