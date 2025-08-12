using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Snafets.TakeItEasy.Application.Features.Lobby;
using Snafets.TakeItEasy.Domain.Lobby;

namespace Snafets.TakeItEasy.Persistence;

public class InMemoryLobbyRepository : ILobbyRepository
{
    private readonly ConcurrentDictionary<Guid, LobbyModel> _lobbies = new();

    public Task<LobbyModel> AddLobbyAsync(string name, Guid creatorId)
    {
        var lobby = new LobbyModel { Id = Guid.NewGuid(), Name = name };
        lobby.PlayerIds.Add(creatorId);
        _lobbies[lobby.Id] = lobby;
        return Task.FromResult(lobby);
    }

    public Task<LobbyModel?> UpdateLobby_AddPlayerAsync(Guid lobbyId, Guid playerId)
    {
        if (_lobbies.TryGetValue(lobbyId, out var lobby))
        {
            if (!lobby.PlayerIds.Contains(playerId))
                lobby.PlayerIds.Add(playerId);
            return Task.FromResult<LobbyModel?>(lobby);
        }
        return Task.FromResult<LobbyModel?>(null);
    }

    public Task<bool> UpdateLobby_RemovePlayerAsync(Guid lobbyId, Guid playerId)
    {
        if (_lobbies.TryGetValue(lobbyId, out var lobby))
        {
            lobby.PlayerIds.Remove(playerId);
            if (lobby.PlayerIds.Count == 0)
            {
                _lobbies.TryRemove(lobbyId, out _);
            }
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
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
}
