// Centralized API calls for lobby
export async function fetchAllLobbies() {
    const res = await fetch(`http://localhost:5124/api/Lobby`);
    if (!res.ok) throw new Error(`Failed to fetch lobbies ${res.statusText}`);
    return await res.json();
}

export async function getLobbyById(id) {
    const res = await fetch(`http://localhost:5124/api/Lobby/${id}`);
    if (!res.ok) throw new Error(`Failed to fetch lobby ${res.statusText}`);
    return await res.json();
}

export async function createLobby(name, creatorId) {
    const response = await fetch(`http://localhost:5124/api/Lobby`, {
        method: 'POST',
        credentials: 'include',       
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ Name: name, CreatorId: creatorId }),
    });
    if (!response.ok) throw new Error(`Failed to create lobby ${response.statusText}`);
    return await response.json();
}

export async function joinLobby(lobbyId, playerId) {
    const response = await fetch(`http://localhost:5124/api/Lobby/${lobbyId}/join`, {
        method: 'POST',
        credentials: 'include',       
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ PlayerId: playerId }),
    });
    if (!response.ok) throw new Error(`Failed to join lobby ${response.statusText}`);
    return await response.json();
}

export async function leaveLobby(lobbyId, playerId) {
    const response = await fetch(`http://localhost:5124/api/Lobby/${lobbyId}/leave`, {
        method: 'POST',
        credentials: 'include',       
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ PlayerId: playerId }),
    });
    if (!response.ok) throw new Error(`Failed to leave lobby ${response.statusText}`);
}

export async function deleteLobby(lobbyId) {
    const response = await fetch(`/api/lobby/${lobbyId}`, {
        method: 'DELETE',
        credentials: 'include',       
        headers: {
            'Content-Type': 'application/json',
        },
    });
    if (!response.ok) throw new Error(`Failed to delete lobby ${response.statusText}`);
}

export async function startGameFromLobby(lobbyId) {
    const response = await fetch(`http://localhost:5124/api/Lobby/${lobbyId}/start`, {
        method: 'POST',
        credentials: 'include',       
    });
    if (!response.ok) throw new Error(`Failed to start game from lobby ${response.statusText}`);
    return await response.json();
}
