"use server";

// Centralized API calls for game
export async function fetchAllGames() {
	const res = await fetch(`http://localhost:5124/api/Game`);
	if (!res.ok) throw new Error('Failed to fetch games');
	return await res.json();
}

export async function fetchGameById(id) {
	const res = await fetch(`http://localhost:5124/api/Game/${id}`);
	if (!res.ok) throw new Error('Failed to fetch game');
	return await res.json();
}

export async function postPlayerMove(gameId, playerId, index) {
    const response = await fetch(`http://localhost:5124/api/Game/move?gameId=${gameId}&playerId=${playerId}&index=${index}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({}),
    });
    if (!response.ok) throw new Error('Failed to post move');
}
