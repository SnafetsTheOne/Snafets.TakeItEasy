import { useEffect, useState } from "react";
import { useRealtime } from "../../infra/RealtimeProvider";
import { getPlayersGames, fetchGameById } from "../../data-access/game";
import GameCard from "./GameCard";
import {
  cardStyle,
  cardNameStyle,
  cardIdStyle,
  head1,
  head2,
  horizontalContainer,
  horizontalContainerItem,
  verticalContainer,
  verticalContainerItem,
} from "../../infra/css";

export const GamesPage = () => {
  const [games, setGames] = useState([]);
  const [showCompleted, setShowCompleted] = useState(false);
  const realtime = useRealtime();

  useEffect(() => {
    getPlayersGames(showCompleted).then(setGames);

    // Listen for gameStart
    const offStartGame = realtime.on("gameStart", async ({gameId}) => {
      const updatedGame = await fetchGameById(gameId);
      setGames(prevGames => [updatedGame, ...prevGames]);
    });

    // Listen for gameUpdate
    const offUpdateGame = realtime.on("gameUpdate", async (gameId) => {
      try {
        const updatedGame = await fetchGameById(gameId);
        setGames(prevGames => {
          const idx = prevGames.findIndex(g => g.id === updatedGame.id);
          if (idx !== -1) {
            const updatedList = [...prevGames];
            updatedList[idx] = updatedGame;
            return updatedList;
          } else {
            return [updatedGame, ...prevGames];
          }
        });
      } catch (e) {
        // Optionally handle error (e.g., show notification)
        console.error("Failed to fetch updated game", e);
      }
    });

    return () => {
      offStartGame();
      offUpdateGame();
    };
  }, [showCompleted, realtime]);

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
          ...head1,
        }}
      >
        Games
      </h1>
      <div style={{ ...verticalContainerItem, ...horizontalContainer, alignItems: "center", }}>
        <label
          style={{
            ...horizontalContainerItem,
            fontSize: "1.5rem",
            textAlign: "right",
          }}
        >
          Show completed games
        </label>
        <input
          type="checkbox"
          checked={showCompleted}
          onChange={(e) => setShowCompleted(e.target.checked)}
          style={{ ...horizontalContainerItem, scale: 1.5 }}
        />
      </div>
      <div
        style={{
          ...verticalContainerItem,
          ...verticalContainer,
        }}
      >
        {games && games.length > 0 ? (
          <div
            style={{ display: "flex", flexDirection: "column", gap: "1rem" }}
          >
            {games.map((game) => (
              <GameCard key={game.id} id={game.id} game={game} />
            ))}
          </div>
        ) : (
          <div
            style={{
              color: "#888",
              textAlign: "center",
              marginTop: "2rem",
              padding: "2rem",
              background: "#fff",
              borderRadius: "12px",
              border: "1px solid #e2e8f0",
            }}
          >
            No games found.
          </div>
        )}
      </div>
    </div>
  );
};
