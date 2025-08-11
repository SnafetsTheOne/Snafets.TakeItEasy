import Link from 'next/link';
import { fetchAllGames } from '@/data-access/game';

export default async function GamesPage() {
  const games = await fetchAllGames();

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
