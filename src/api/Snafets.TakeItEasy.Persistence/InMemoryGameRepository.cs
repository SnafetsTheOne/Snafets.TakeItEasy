using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Snafets.TakeItEasy.Application.Features.Game;
using Snafets.TakeItEasy.Domain.Game;

namespace Snafets.TakeItEasy.Persistence;

public class InMemoryGameRepository : IGameRepository
{
    private readonly Dictionary<Guid, GameModel> _store = new();

    public async Task SaveGameAsync(GameModel game)
    {
        game.UpdatedAt = DateTime.UtcNow;
        _store[game.Id] = game;
        await Task.CompletedTask;
    }

    public async Task<GameModel?> GetGameAsync(Guid id)
    {
        _store.TryGetValue(id, out var game);
        return await Task.FromResult(game);
    }

    public async Task<IEnumerable<GameModel>> GetAllGamesAsync()
    {
        return await Task.FromResult(_store.Values);
    }

    public async Task DeleteGameAsync(Guid id)
    {
        _store.Remove(id);
        await Task.CompletedTask;
    }
}
