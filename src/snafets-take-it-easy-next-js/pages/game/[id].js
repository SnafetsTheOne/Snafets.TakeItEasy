import { useRouter } from "next/router";
import PropTypes from "prop-types";
import HoneycombBoard from "@/components/HoneycombBoard";

export default function GamePage({ game }) {
  const playerBoard = game.playerBoards?.[0];
  return (
    <div>
      <h1>Game Page</h1>
      <p>Welcome to the game with ID: {game.id}</p>
      {playerBoard && (
        <>
          <div style={{ fontWeight: 'bold', marginBottom: '1em' }}>
            Score: {playerBoard.score}
          </div>
          <HoneycombBoard tiles={playerBoard.spaces} />
        </>
      )}
    </div>
  );
}

GamePage.propTypes = {
  game: PropTypes.shape({
    id: PropTypes.oneOfType([PropTypes.string, PropTypes.number]).isRequired,
    playerBoards: PropTypes.arrayOf(PropTypes.array).isRequired,
  }).isRequired,
};

export async function getServerSideProps({ params }) {
  // Fetch game data based on the ID
  const res = await fetch(`http://localhost:5124/api/Game/${params.id}`);
  const game = await res.json();
  console.log('game: ' + JSON.stringify(game));

  return {
    props: {
      game,
    },
  };
}
