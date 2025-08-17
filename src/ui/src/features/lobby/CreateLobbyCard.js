import {
  cardStyle,
  verticalContainer,
  verticalContainerItem,
  head2,
} from "../../infra/css";

export default function LobbyCard({
  user,
  handleAddLobby,
  newLobbyName,
  setNewLobbyName,
}) {
  if (user == undefined)
    return (
      <div
        style={{
          ...verticalContainerItem,
          color: "#888",
          background: "#fff",
          padding: "1.5rem",
          borderRadius: "12px",
          textAlign: "center",
          marginBottom: "2rem",
          border: "1px solid #e2e8f0",
        }}
      >
        Sign Up to Play
      </div>
    );
  return (
    <form
      style={{
        ...verticalContainerItem,
        ...cardStyle,
        alignSelf: "stretch",
        alignItems: "center",
        flexGrow: 1,
      }}
      onSubmit={handleAddLobby}
    >
      <div
        style={{
          ...verticalContainer,
          alignItems: "center",
          flexGrow: 1,
        }}
      >
        <label
          htmlFor="lobbyName"
          style={{
            ...verticalContainerItem,
            ...head2,
          }}
        >
          Lobby Name
        </label>
        <input
          id="lobbyName"
          type="text"
          value={newLobbyName}
          onChange={(e) => setNewLobbyName(e.target.value)}
          placeholder="Enter lobby name"
          style={{
            ...verticalContainerItem,
            padding: "0.875rem 1rem",
            fontSize: "1rem",
            border: "1px solid #d1d5db",
            borderRadius: "8px",
            outline: "none",
            background: "#fff",
            color: "#222",
            boxSizing: "border-box",
            minHeight: "44px",
          }}
          required
        />
        <button
          type="submit"
          className="btn btn-primary"
          style={{
            ...verticalContainerItem,
            fontSize: "1.1rem",
            fontWeight: 600,
          }}
        >
          Create Lobby
        </button>
      </div>
    </form>
  );
}
