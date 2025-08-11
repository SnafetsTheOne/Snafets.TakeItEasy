import React from 'react';
import Link from 'next/link';

export default function Game({ games }) {
  return (
    <div>
      <h1>Game Page</h1>
      <p>Welcome to the game!</p>
      <ul>
        {games.map(game => (
          <li key={game.id}>
            <Link href={`/game/${game.id}`}>
              {`Game ID: ${game.id}`}
            </Link>
          </li>
        ))}
      </ul>
    </div>
  );
}

export async function getServerSideProps() {
  // Fetch all games
  const res = await fetch(`http://localhost:5124/api/Game`);
  const games = await res.json();

  return {
    props: {
      games,
    },
  };
}
