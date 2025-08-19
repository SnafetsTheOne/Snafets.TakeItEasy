using System.Linq;

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using LiteDB;
using Snafets.TakeItEasy.Domain.Lobby;
using Snafets.TakeItEasy.Application.Features.Lobby;
using Microsoft.Extensions.Options;

namespace Snafets.TakeItEasy.Persistence.LiteDb;

public class LiteDbLobbyRepository(IOptions<LiteDbOptions> options) : ILobbyRepository
{
    public async Task<LobbyModel?> GetLobbyAsync(Guid lobbyId)
    {
        return await Task.Run(() =>
        {
            using var db = new LiteDatabase(options.Value.DatabasePath);
            var col = db.GetCollection<LobbyModel>(options.Value.LobbiesCollectionName);
            return col.FindById(lobbyId);
        });
    }

    public async Task<bool> DeleteLobbyAsync(Guid lobbyId)
    {
        return await Task.Run(() =>
        {
            using var db = new LiteDatabase(options.Value.DatabasePath);
            var col = db.GetCollection<LobbyModel>(options.Value.LobbiesCollectionName);
            return col.Delete(lobbyId);
        });
    }

    public async Task<IEnumerable<LobbyModel>> GetAllLobbiesAsync()
    {
        return await Task.Run(() =>
        {
            using var db = new LiteDatabase(options.Value.DatabasePath);
            var col = db.GetCollection<LobbyModel>(options.Value.LobbiesCollectionName);
            return col.FindAll().ToList();
        });
    }

    public async Task SaveLobbyAsync(LobbyModel lobby)
    {
        await Task.Run(() =>
        {
            lobby.UpdatedAt = DateTime.UtcNow;
            using var db = new LiteDatabase(options.Value.DatabasePath);
            var col = db.GetCollection<LobbyModel>(options.Value.LobbiesCollectionName);
            col.Upsert(lobby);
        });
    }
}
