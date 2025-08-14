using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Snafets.TakeItEasy.Application.Features.Lobby;
using Snafets.TakeItEasy.Domain.Lobby;

namespace Snafets.TakeItEasy.Persistence;

public class InMemoryLobbyRepository : ILobbyRepository
{
    private readonly ConcurrentDictionary<Guid, LobbyModel> _lobbies = new();

    public Task SaveLobbyAsync(LobbyModel lobby)
    {
        lobby.UpdatedAt = DateTime.UtcNow;
        _lobbies[lobby.Id] = lobby;
        return Task.CompletedTask;
    }

    public Task<LobbyModel?> GetLobbyAsync(Guid lobbyId)
    {
        _lobbies.TryGetValue(lobbyId, out var lobby);
        return Task.FromResult(lobby);
    }

    public Task<bool> DeleteLobbyAsync(Guid lobbyId)
    {
        return Task.FromResult(_lobbies.TryRemove(lobbyId, out _));
    }

    public Task<IEnumerable<LobbyModel>> GetAllLobbiesAsync()
    {
        return Task.FromResult(_lobbies.Values as IEnumerable<LobbyModel>);
    }
}
