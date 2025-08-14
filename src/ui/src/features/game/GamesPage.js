import { useEffect, useState } from 'react';
import { getPlayersGames } from '../../data-access/game';
import GameCard from "./GameCard";

export const GamesPage = () => {
  const [games, setGames] = useState([]);

  useEffect(() => {
      getPlayersGames().then(setGames);
  }, []);

  return (
    <div className="container" style={{ 
      maxWidth: 600, 
      margin: '2rem auto', 
      padding: '1rem' 
    }}>
      <h1 style={{ 
        fontSize: '2.5rem', 
        fontWeight: 600, 
        marginBottom: '2rem', 
        color: '#222', 
        textAlign: 'center' 
      }}>
        Games
      </h1>
      <div>
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
