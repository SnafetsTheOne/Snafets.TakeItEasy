import { useEffect, useState } from 'react';
import { getPlayersGames } from '../../data-access/game';
import GameCard from "./GameCard";
import {
  cardStyle,
  cardNameStyle,
  cardIdStyle,
  head1,
  head2,
  verticalContainer,
  verticalContainerItem,
} from "../../infra/css";

export const GamesPage = () => {
  const [games, setGames] = useState([]);

  useEffect(() => {
      getPlayersGames().then(setGames);
  }, []);

  return (
    <div style={{ 
        ...verticalContainer,
        maxWidth: 500,
        margin: "auto",
    }}>
      <h1 style={{ 
          ...verticalContainerItem,
          ...head1
      }}>
        Games
      </h1>
      <div style={{ 
        ...verticalContainerItem, 
        ...verticalContainer }}>
        {games && games.length > 0 ? (
          <div style={{ display: "flex", flexDirection: "column", gap: "1rem" }}>
            {games.map(game => (
              <GameCard
                key={game.id}
                id={game.id}
                game={game}
              />
            ))}
          </div>
        ) : (
          <div style={{ 
            color: '#888', 
            textAlign: 'center', 
            marginTop: '2rem',
            padding: '2rem',
            background: '#fff',
            borderRadius: '12px',
            border: '1px solid #e2e8f0'
          }}>
            No games found.
          </div>
        )}
      </div>
    </div>
  );
}
