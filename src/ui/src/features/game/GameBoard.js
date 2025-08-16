import HoneycombStandaloneCell from "./Honeycomb/HoneycombStandaloneCell";
import HoneycombBoard from "./Honeycomb/HoneycombBoard";
import {
  cardStyle,
  cardNameStyle,
  cardIdStyle,
  head1,
  head2,
  verticalContainer,
  verticalContainerItem,
  horizontalContainer,
  horizontalContainerItem,
} from "../../infra/css";

export function GameBoard({
  title,
  board,
  currentTile,
  gameId,
  reloadGame,
  isUser,
}) {
  console.log(board);
  currentTile = board.canPlay ? currentTile : null;
  return (
    <div
      style={{
        ...verticalContainer,
        ...verticalContainerItem,
      }}
    >
      <div
        style={{
          ...verticalContainerItem,
          ...horizontalContainer,
          paddingBottom: "1rem",
        }}
      >
        {/* Score */}
        <div
          style={{
            ...cardStyle,
            ...horizontalContainerItem,
            ...verticalContainer,
            width: 80,
            height: 80,
            fontWeight: 600,
            fontSize: "1.1rem",
            color: "#222",
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            justifyContent: "center",
          }}
        >
          <span
            style={{
              ...verticalContainerItem,
              fontSize: "0.9rem",
              color: "#888",
              fontWeight: 400,
              marginBottom: 4,
            }}
          >
            Score
          </span>
          <span
            style={{
              ...verticalContainerItem,
              fontSize: "2rem",
              fontWeight: 700,
            }}
          >
            {board?.score != null ? board.score : "-"}
          </span>
        </div>

        {/* Next Cell Preview (no text) */}
        <div
          style={{
            ...cardStyle,
            ...horizontalContainerItem,
            width: 80,
            height: 80,
            borderRadius: 10,
            alignItems: "center",
            justifyContent: "center",
          }}
        >
          {currentTile ? (
            <HoneycombBoard           
              radius={0}
              tiles={[{ placedTile: currentTile }]}
              playerId={board.playerId}
              gameId={gameId}
              canPlay={isUser ? board.canPlay : false}
              reloadGame={isUser ? reloadGame : () => {}}
            />
          ) : (
            <span style={{ color: "#bbb", fontSize: "1.2rem" }}>—</span>
          )}
        </div>
      </div>

      {/* Game Board below */}
      <div
        style={{
          ...cardStyle,
          ...verticalContainerItem,
          height: 320,
          width: 300,
          justifyContent: "flex-end",
        }}
      >
        <HoneycombBoard
          radius={2}
          tiles={board.spaces}
          playerId={board.playerId}
          gameId={gameId}
          canPlay={isUser ? board.canPlay : false}
          reloadGame={isUser ? reloadGame : () => {}}
        />
      </div>
    </div>
  );
}
