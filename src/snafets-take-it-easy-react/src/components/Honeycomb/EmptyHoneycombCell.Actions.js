
import { postPlayerMove } from '../../data-access/game';


export async function makeMoveAction(prevState) {
  await postPlayerMove(
    prevState.gameId,
    prevState.playerId,
    prevState.index
  );
  // In React, you may want to update state or trigger a re-fetch here
  return prevState;
}
