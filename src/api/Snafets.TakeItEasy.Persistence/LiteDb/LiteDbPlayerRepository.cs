
using System;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.Extensions.Options;
using Snafets.TakeItEasy.Application.Features.Player;
using Snafets.TakeItEasy.Domain;

namespace Snafets.TakeItEasy.Persistence.LiteDb;

public class LiteDbPlayerRepository(IOptions<LiteDbOptions> options) : IPlayerRepository
{
    public async Task<PlayerModel> SavePlayerAsync(PlayerModel player)
    {
        return await Task.Run(() =>
        {
            using var db = new LiteDatabase(options.Value.DatabasePath);
            var col = db.GetCollection<PlayerModel>(options.Value.PlayersCollectionName);
            col.Upsert(player);
            return player;
        });
    }

    public async Task<PlayerModel?> GetPlayerAsync(Guid id)
    {
        return await Task.Run(() =>
        {
            using var db = new LiteDatabase(options.Value.DatabasePath);
            var col = db.GetCollection<PlayerModel>(options.Value.PlayersCollectionName);
            return col.FindById(id);
        });
    }

    public async Task<PlayerModel?> GetPlayerAsync(string name, string passwordHash)
    {
        return await Task.Run(() =>
        {
            using var db = new LiteDatabase(options.Value.DatabasePath);
            var col = db.GetCollection<PlayerModel>(options.Value.PlayersCollectionName);
            return col.FindOne(p => p.Name == name && p.PasswordHash == passwordHash);
        });
    }
}
