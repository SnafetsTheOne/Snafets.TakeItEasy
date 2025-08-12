

import { useNavigate } from "react-router-dom";

const cardStyle = {
  background: "#fff",
  borderRadius: "10px",
  boxShadow: "0 2px 8px rgba(0,0,0,0.07)",
  padding: "1.5rem",
  marginBottom: "1.2rem",
  display: "flex",
  alignItems: "center",
  justifyContent: "space-between",
  transition: "box-shadow 0.2s",
};

const nameStyle = {
  fontSize: "1.6rem",
  fontWeight: 700,
  color: "#222",
  letterSpacing: "0.02em",
  display: "block",
};

const idStyle = {
  fontSize: "0.8rem",
  color: "#bbb",
  marginLeft: "1.2rem",
  fontWeight: 400,
  letterSpacing: "0.01em",
  display: "block",
  marginTop: "0.2rem",
};

const turnBadgeStyle = {
  background: "#e0f7fa",
  color: "#00796b",
  borderRadius: "6px",
  padding: "0.3rem 0.7rem",
  fontSize: "0.95rem",
  fontWeight: 500,
  marginLeft: "1.2rem",
  boxShadow: "0 1px 4px rgba(0,0,0,0.04)",
};

const clickableCardStyle = {
  ...cardStyle,
  cursor: "pointer",
  userSelect: "none",
};

export default function GameCard({ id, game, isYourTurn }) {
  const navigate = useNavigate();

  return (
    <button
      key={game.id}
      style={{
        ...clickableCardStyle,
        border: "none",
        outline: "none",
        width: "100%",
        textAlign: "left",
        padding: "0",
        background: "none",
      }}
      onClick={() => navigate(`/game/${game.id}`)}
      aria-label={`Open game ${game.name}`}
    >
      <div
        style={{
          ...cardStyle,
          margin: 0,
          boxShadow: "none",
          padding: "1.5rem",
        }}
      >
        <span style={nameStyle}>{game.name}</span>
        <span style={idStyle}>ID: {game.id}</span>

        {isYourTurn && <span style={turnBadgeStyle}>Your Turn</span>}
      </div>
    </button>
  );
}
