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

const badgeStyle = {
  borderRadius: "6px",
  padding: "0.3rem 0.7rem",
  fontSize: "0.95rem",
  fontWeight: 500,
  boxShadow: "0 1px 4px rgba(0,0,0,0.04)",
  alignSelf: "flex-start",
};

export default function GameCard({ id, game }) {
  const navigate = useNavigate();
  console.log(game);

  return (
    <div
      key={game.id}
      style={{
        ...verticalContainerItem,
        ...horizontalContainer,
        ...cardStyle,
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
          <span style={{ 
            ...badgeStyle, 
            whiteSpace: "nowrap",
            background: "#e0f7fa",
            color: "#00796b",
           }}>
            Your Turn
          </span>
        )}
        {game.isCompleted && (
          <span
            style={{
              ...badgeStyle,
              background: "#c8e6c9",
              color: "#388e3c",
            }}
          >
            Completed
          </span>
        )}
      </div>
    </div>
  );
}
