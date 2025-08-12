import { useNavigate } from "react-router-dom";

const cardStyle = { background: '#fff', borderRadius: '10px', boxShadow: '0 2px 8px rgba(0,0,0,0.07)', padding: '1.5rem', marginBottom: '1.2rem', display: 'flex', alignItems: 'center', justifyContent: 'space-between', transition: 'box-shadow 0.2s', cursor: 'pointer', };

const nameStyle = { fontSize: '1.4rem', fontWeight: 700, color: '#222', };

const countStyle = { fontSize: '1rem', color: '#00796b', background: '#e0f7fa', borderRadius: '6px', padding: '0.3rem 0.7rem', fontWeight: 500, marginLeft: '1.2rem', };

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
      style={cardStyle}
      onClick={handleCardClick}
      role="button"
      tabIndex={0}
      aria-label={`Open lobby ${lobby.name}`}
    >
      <span style={nameStyle}>{lobby.name}</span>
      <span style={countStyle}>{lobby.playerIds.length} Players</span>
      {joined ? (
        <span
          style={{ marginLeft: "1.2rem", color: "#388e3c", fontWeight: 500 }}
        >
          Joined
        </span>
      ) : (
        <button
          style={{ marginLeft: "1.2rem", padding: "0.3rem 0.9rem", borderRadius: 6, border: "none", background: "#00796b", color: "#fff", fontWeight: 500, cursor: "pointer", fontSize: "1rem", }}
          disabled={!loggedIn}
          onClick={() => handleJoin(lobby.id)}
        >
          Join
        </button>
      )}
    </div>
  );
}
