using Snafets.TakeItEasy.Domain.Lobby;
using Snafets.TakeItEasy.Domain.Game;
using Snafets.TakeItEasy.Application.Features.Game;

namespace Snafets.TakeItEasy.Application.Features.Lobby;

public class LobbyService : ILobbyService
{
    private readonly ILobbyRepository _repository;
    private readonly IGameService _gameService;
    private readonly INotifier _notifier;

    public LobbyService(ILobbyRepository repository, IGameService gameService, INotifier notifier)
    {
        _repository = repository;
        _gameService = gameService;
        _notifier = notifier;
    }

    public async Task<LobbyModel> CreateLobbyAsync(string name, Guid creatorId)
    {
        var lobby = new LobbyModel
        {
            Id = Guid.NewGuid(),
            Name = name,
            PlayerIds = new List<Guid> { creatorId },
            CreatedAt = DateTime.UtcNow
        };
        await _repository.SaveLobbyAsync(lobby);
        return lobby;
    }

    public async Task<LobbyModel?> UpdateLobby_AddPlayerAsync(Guid lobbyId, Guid playerId)
    {
        var lobby = await _repository.GetLobbyAsync(lobbyId);
        if (lobby == null)
        {
            return null;
        }
        if (!lobby.PlayerIds.Contains(playerId))
        {
            lobby.PlayerIds.Add(playerId);
            await _repository.SaveLobbyAsync(lobby);
        }
        foreach (var otherPlayerId in lobby.PlayerIds.Where(id => id != playerId))
        {
            await _notifier.NotifyLobbyUpdate(otherPlayerId, lobby.Id);
        }
        return lobby;
    }

    public async Task<bool> UpdateLobby_RemovePlayerAsync(Guid lobbyId, Guid playerId)
    {
        var lobby = await _repository.GetLobbyAsync(lobbyId);
        if (lobby == null)
        {
            return false;
        }
        if (lobby.PlayerIds.Contains(playerId))
        {
            lobby.PlayerIds.Remove(playerId);
            if (lobby.PlayerIds.Count == 0)
            {
                await _repository.DeleteLobbyAsync(lobbyId);
                return true;
            }
            await _repository.SaveLobbyAsync(lobby);
        }
        foreach (var otherPlayerId in lobby.PlayerIds.Where(id => id != playerId))
        {
            await _notifier.NotifyLobbyUpdate(otherPlayerId, lobby.Id);
        }
        return true;
    }

    public async Task<LobbyModel?> GetLobbyAsync(Guid lobbyId)
    {
        return await _repository.GetLobbyAsync(lobbyId);
    }

    public async Task<GameModel?> DeleteLobbyAndStartGameAsync(Guid lobbyId, Guid playerId)
    {
        var lobby = await _repository.GetLobbyAsync(lobbyId);
        if (lobby == null || lobby.PlayerIds.Count == 0)
            return null;

        var game = await _gameService.CreateGameAsync(lobby.PlayerIds, lobby.Name);
        await _repository.DeleteLobbyAsync(lobbyId);
        foreach (var otherPlayerId in lobby.PlayerIds.Where(id => id != playerId))
        {
            await _notifier.NotifyGameStartUpdate(otherPlayerId, lobbyId, game.Id);
        }
        return game;
    }

    public async Task<IEnumerable<LobbyModel>> GetAllLobbiesAsync()
    {
        return await _repository.GetAllLobbiesAsync();
    }

    public async Task<bool> DeleteLobbyAsync(Guid lobbyId, Guid playerId)
    {
        var lobby = await _repository.GetLobbyAsync(lobbyId);
        if (lobby == null)
        {
            return false;
        }
        var result = await _repository.DeleteLobbyAsync(lobbyId);
        if (result)
        {
            foreach (var otherPlayerId in lobby.PlayerIds.Where(id => id != playerId))
            {
                await _notifier.NotifyLobbyUpdate(otherPlayerId, lobbyId);
            }
        }
        return result;
    }
}
