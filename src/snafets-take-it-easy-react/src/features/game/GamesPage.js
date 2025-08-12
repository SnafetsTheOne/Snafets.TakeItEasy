import { useEffect, useState } from 'react';
import { fetchAllGames } from '../../data-access/game';
import GameCard from "./GameCard";

export const GamesPage = () => {
  const [games, setGames] = useState([]);
  const isYourTurn = true; // Replace with actual logic if needed

  useEffect(() => {
    fetchAllGames().then(setGames);
  }, []);

  return (
    <div style={{ maxWidth: 480, margin: '2rem auto', padding: '0 1rem' }}>
      <h1 style={{ fontSize: '2rem', fontWeight: 600, marginBottom: '2rem', color: '#222', textAlign: 'center' }}>Games</h1>
      <div>
        {games && games.length > 0 ? (
          games.map(game => (
            <GameCard
              key={game.id}
              id={game.id}
              game={game}
              isYourTurn={isYourTurn}
            />
          ))
        ) : (
          <div style={{ color: '#888', textAlign: 'center', marginTop: '2rem' }}>No games found.</div>
        )}
      </div>
    </div>
  );
}
