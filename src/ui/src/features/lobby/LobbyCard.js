import { useNavigate } from "react-router-dom";
import {
  cardStyle,
  cardNameStyle,
  cardIdStyle,
  horizontalContainer,
  horizontalContainerItem,
  verticalContainer,
  verticalContainerItem,
} from "../../infra/css";

export default function LobbyCard({ lobby, currentUserId, handleJoin }) {
  const loggedIn = currentUserId != null;
  const joined = loggedIn && lobby.playerIds.includes(currentUserId);
  const navigate = useNavigate();

  const handleCardClick = (e) => {
    // Prevent navigation if Join button is clicked
    if (e.target.tagName === "BUTTON") return;
    navigate(`/lobby/${lobby.id}`);
  };
  return (
    <div
      key={lobby.id}
      style={{
        ...cardStyle,
        ...verticalContainerItem,
        ...verticalContainer,
        cursor: "pointer",
        userSelect: "none",
        alignItems: "left",
        textAlign: "left",
      }}
      onClick={handleCardClick}
      role="button"
      tabIndex={0}
      aria-label={`Open lobby ${lobby.name}`}
    >
      <div
        style={{
          ...verticalContainerItem,
          ...horizontalContainer,
        }}
      >
        <span
          style={{
            ...cardNameStyle,
            ...verticalContainerItem,
            flex: 1,
          }}
        >
          {lobby.name}
        </span>

        <span
          style={{
            ...horizontalContainerItem,
            fontSize: "1rem",
            color: "#00796b",
            background: "#eeeeee",
            borderRadius: "6px",
            padding: "0.3rem 0.7rem",
            fontWeight: 500,
            marginLeft: "1.2rem",
          }}
        >
          {lobby.playerIds.length}{" "}
          {lobby.playerIds.length == 1 ? "Player" : "Players"}
        </span>
        <div style={{ ...horizontalContainerItem }}>
          {joined ? (
            <span
              style={{
                marginLeft: "1.2rem",
                color: "#388e3c",
                fontWeight: 500,
              }}
            >
              Joined
            </span>
          ) : (
            <button
              style={{
                marginLeft: "1.2rem",
                padding: "0.3rem 0.9rem",
                borderRadius: 6,
                border: "none",
                background: "#00796b",
                color: "#fff",
                fontWeight: 500,
                cursor: "pointer",
                fontSize: "1rem",
              }}
              disabled={!loggedIn}
              onClick={() => handleJoin(lobby.id)}
            >
              Join
            </button>
          )}
        </div>
      </div>

      <span
        style={{
          ...verticalContainerItem,
          ...cardIdStyle,
          textAlign: "left",
        }}
      >
        ID: {lobby.id}
      </span>
    </div>
  );
}
