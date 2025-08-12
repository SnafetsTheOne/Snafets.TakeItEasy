"use server";

import { postPlayerMove } from '@/data-access/game';
import { revalidatePath } from 'next/cache';

export async function makeMoveAction(prevState) {
  await postPlayerMove(
    prevState.gameId,
    prevState.playerId,
    prevState.index
  );
  revalidatePath(`/game/${prevState.gameId}`);
  return prevState;
}
