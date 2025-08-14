using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using Snafets.TakeItEasy.Domain.Game;
using Snafets.TakeItEasy.Application.Features.Game;
using Microsoft.Extensions.Options;

namespace Snafets.TakeItEasy.Persistence.LiteDb;

public class LiteDbGameRepository(IOptions<LiteDbOptions> options) : IGameRepository
{
    public async Task SaveGameAsync(GameModel game)
    {
        await Task.Run(() =>
        {
            using var db = new LiteDatabase(options.Value.DatabasePath);
            var col = db.GetCollection<GameModel>(options.Value.GamesCollectionName);
            col.Upsert(game);
        });
    }

    public async Task<GameModel?> GetGameAsync(Guid id)
    {
        return await Task.Run(() =>
        {
            using var db = new LiteDatabase(options.Value.DatabasePath);
            var col = db.GetCollection<GameModel>(options.Value.GamesCollectionName);
            return col.FindById(id);
        });
    }

    public async Task<IEnumerable<GameModel>> GetAllGamesAsync(Guid playerId)
    {
        return await Task.Run(() =>
        {
            using var db = new LiteDatabase(options.Value.DatabasePath);
            var col = db.GetCollection<GameModel>(options.Value.GamesCollectionName);
            return col.Find(g => g.PlayerBoards.Select(x => x.PlayerId).Any(id => id == playerId)).ToList();
        });
    }

    public async Task DeleteGameAsync(Guid id)
    {
        await Task.Run(() =>
        {
            using var db = new LiteDatabase(options.Value.DatabasePath);
            var col = db.GetCollection<GameModel>(options.Value.GamesCollectionName);
            col.Delete(id);
        });
    }
}
