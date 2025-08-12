using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Snafets.TakeItEasy.Domain;
using Snafets.TakeItEasy.Application.Features.Player;

namespace Snafets.TakeItEasy.Persistence.Player;

public class InMemoryPlayerRepository : IPlayerRepository
{
    private readonly Dictionary<Guid, PlayerModel> _players = new();

    public Task AddPlayerAsync(PlayerModel player)
    {
        _players[player.Id] = player;
        return Task.CompletedTask;
    }

    public Task<PlayerModel?> GetPlayerByIdAsync(Guid id)
    {
        _players.TryGetValue(id, out var player);
        return Task.FromResult(player);
    }
}

