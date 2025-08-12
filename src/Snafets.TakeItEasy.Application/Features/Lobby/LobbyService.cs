using Snafets.TakeItEasy.Domain.Lobby;
using Snafets.TakeItEasy.Domain.Game;
using Snafets.TakeItEasy.Application.Features.Game;

namespace Snafets.TakeItEasy.Application.Features.Lobby;

public class LobbyService : ILobbyService
{
    private readonly ILobbyRepository _repository;
    private readonly IGameService _gameService;

    public LobbyService(ILobbyRepository repository, IGameService gameService)
    {
        _repository = repository;
        _gameService = gameService;
    }

    public async Task<LobbyModel> CreateLobbyAsync(string name, Guid creatorId)
    {
        return await _repository.AddLobbyAsync(name, creatorId);
    }

    public async Task<LobbyModel?> UpdateLobby_AddPlayerAsync(Guid lobbyId, Guid playerId)
    {
        return await _repository.UpdateLobby_AddPlayerAsync(lobbyId, playerId);
    }

    public async Task<bool> UpdateLobby_RemovePlayerAsync(Guid lobbyId, Guid playerId)
    {
        return await _repository.UpdateLobby_RemovePlayerAsync(lobbyId, playerId);
    }

    public async Task<LobbyModel?> GetLobbyAsync(Guid lobbyId)
    {
        return await _repository.GetLobbyAsync(lobbyId);
    }

    public async Task<GameModel?> DeleteLobbyAndStartGameAsync(Guid lobbyId)
    {
        var lobby = await _repository.GetLobbyAsync(lobbyId);
        if (lobby == null || lobby.PlayerIds.Count == 0)
            return null;

        var game = await _gameService.CreateGameAsync(lobby.PlayerIds, lobby.Name);
        await _repository.DeleteLobbyAsync(lobbyId);
        return game;
    }

    public async Task<IEnumerable<LobbyModel>> GetAllLobbiesAsync()
    {
        return await _repository.GetAllLobbiesAsync();
    }
}
