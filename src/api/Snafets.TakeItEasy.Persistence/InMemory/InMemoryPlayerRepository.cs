using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Snafets.TakeItEasy.Domain;
using Snafets.TakeItEasy.Application.Features.Player;
using System.Collections.Concurrent;

namespace Snafets.TakeItEasy.Persistence.Player;

public class InMemoryPlayerRepository : IPlayerRepository
{
    private readonly ConcurrentDictionary<Guid, PlayerModel> _players = new();

    public Task<PlayerModel> SavePlayerAsync(PlayerModel player)
    {
        player.UpdatedAt = DateTime.UtcNow;
        _players[player.Id] = player;
        return Task.FromResult(player);
    }

    public Task<PlayerModel?> GetPlayerAsync(Guid id)
    {
        _players.TryGetValue(id, out var player);
        return Task.FromResult(player);
    }

    public Task<PlayerModel?> GetPlayerAsync(string name, string passwordHash)
    {
        foreach (var player in _players.Values)
        {
            if (player.Name == name && player.PasswordHash == passwordHash)
                return Task.FromResult<PlayerModel?>(player);
        }
        return Task.FromResult<PlayerModel?>(null);
    }
}

