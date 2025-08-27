import HoneycombBoard from "./Honeycomb/HoneycombBoard";
import {
  cardStyle,
  verticalContainer,
  verticalContainerItem,
  horizontalContainer,
  horizontalContainerItem,
} from "../../infra/css";

export function GameBoard({
  board,
  currentTile,
  gameId,
  reloadGame,
  isUser,
}) {
  console.log("window.height", window.innerHeight)
  console.log("window.width", window.innerWidth)
  const mobile = window.innerWidth < window.innerHeight;
  const size = mobile ? 35 : 40;
  const padding = mobile ? "1rem" : "1.5rem";
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
            padding: padding,
          }}
        >
          <span
            style={{
              ...verticalContainerItem,
              fontSize: "0.9rem",
              color: "#888",
              fontWeight: 400,
              marginBottom: 4,
              textAlign: "center",
              alignSelf: "center",
            }}
          >
            Score
          </span>
          <span
            style={{
              ...verticalContainerItem,
              fontSize: "2rem",
              fontWeight: 700,
              textAlign: "center",
              alignSelf: "center",
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
            padding: padding,
          }}
        >
          {currentTile ? (
            <HoneycombBoard
              radius={0}
              size={size}
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
          justifyContent: "flex-end",
          padding: padding,
        }}
      >
        <HoneycombBoard
          radius={2}
          size={size}
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
