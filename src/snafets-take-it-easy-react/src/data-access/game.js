// Centralized API calls for game
export async function fetchGameById(id) {
	const res = await fetch(`http://localhost:5124/api/Game/${id}`, {
        credentials: 'include',
    });
	if (!res.ok) throw new Error(`Failed to fetch game ${res.statusText}`);
	return await res.json();
}

export async function postPlayerMove(gameId, playerId, index) {
    const response = await fetch(`http://localhost:5124/api/Game/${gameId}/move`, {
        method: 'POST',
        credentials: 'include',       
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ PlayerId: playerId, Index: index }),
    });
    if (!response.ok) throw new Error(`Failed to post move ${response.statusText}`);
}

export async function getPlayersGames() {
    const res = await fetch(`http://localhost:5124/api/Game/`, {
        credentials: 'include',
    });
    if (!res.ok) throw new Error(`Failed to fetch games for player ${res.statusText}`);
    return await res.json();
}
