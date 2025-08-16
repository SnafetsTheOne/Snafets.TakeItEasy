import { useNavigate } from "react-router-dom";
import {
  cardStyle,
  cardNameStyle,
  cardIdStyle,
  verticalContainer,
  verticalContainerItem,
  horizontalContainer,
  horizontalContainerItem,
} from "../../infra/css";

const turnBadgeStyle = {
  background: "#e0f7fa",
  color: "#00796b",
  borderRadius: "6px",
  padding: "0.3rem 0.7rem",
  fontSize: "0.95rem",
  fontWeight: 500,
  boxShadow: "0 1px 4px rgba(0,0,0,0.04)",
};

const completedBadgeStyle = {
  background: "#c8e6c9",
  color: "#388e3c",
  borderRadius: "6px",
  padding: "0.3rem 0.7rem",
  fontSize: "0.95rem",
  fontWeight: 500,
  boxShadow: "0 1px 4px rgba(0,0,0,0.04)",
};

export default function GameCard({ id, game }) {
  const navigate = useNavigate();

  return (
    <div
      key={game.id}
      style={{
        ...cardStyle,
        ...verticalContainerItem,
        ...horizontalContainer,
        cursor: "pointer",
        userSelect: "none",
        alignItems: "left",
        textAlign: "left",
      }}
      onClick={() => navigate(`/game/${game.id}`)}
      role="button"
      tabIndex={0}
      aria-label={`Open game ${game.name}`}
    >
      <div
        style={{
          ...horizontalContainerItem,
          ...verticalContainer,
        }}
      >
        <span
          style={{
            ...cardNameStyle,
            ...verticalContainerItem,
          }}
        >
          {game.name}
        </span>
        <span
          style={{
            ...cardIdStyle,
            ...verticalContainerItem,
          }}
        >
          ID: {game.id}
        </span>
      </div>
      <div
        style={{
          ...horizontalContainerItem,
        }}
      >
        {game.boards[0].canPlay && (
          <span style={{ ...turnBadgeStyle, whiteSpace: "nowrap" }}>
            Your Turn
          </span>
        )}
        {game.isCompleted && <span style={completedBadgeStyle}>Completed</span>}
      </div>
    </div>
  );
}
