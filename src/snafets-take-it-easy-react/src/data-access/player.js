"use server";

// Centralized API calls for player
export async function signupPlayer(name, passwordHash) {
    const response = await fetch(`http://localhost:5124/api/Player/signup`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ Name: name, PasswordHash: passwordHash }),
    });
    if (!response.ok) throw new Error('Failed to signup player');
    return await response.json();
}

export async function getPlayerById(id) {
    const response = await fetch(`http://localhost:5124/api/Player/${id}`);
    if (!response.ok) throw new Error('Failed to fetch player');
    return await response.json();
}

export async function signinPlayer(name, passwordHash) {
    const response = await fetch(`http://localhost:5124/api/Player/signin`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ Name: name, PasswordHash: passwordHash }),
    });
    if (!response.ok) throw new Error('Failed to signin player');
    return await response.json();
}
