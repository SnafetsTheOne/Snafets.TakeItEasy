using Snafets.TakeItEasy.Domain.Game;
using Snafets.TakeItEasy.Domain.Lobby;

namespace Snafets.TakeItEasy.Application.Features.Lobby;

public interface ILobbyService
{
    Task<LobbyModel> CreateLobbyAsync(string name, Guid creatorId);
    Task<GameModel?> DeleteLobbyAndStartGameAsync(Guid lobbyId, Guid playerId);
    Task<LobbyModel?> GetLobbyAsync(Guid lobbyId);
    Task<LobbyModel?> UpdateLobby_AddPlayerAsync(Guid lobbyId, Guid playerId);
    Task<bool> UpdateLobby_RemovePlayerAsync(Guid lobbyId, Guid playerId);
    Task<IEnumerable<LobbyModel>> GetAllLobbiesAsync();
    Task<bool> DeleteLobbyAsync(Guid lobbyId, Guid playerId);
}
