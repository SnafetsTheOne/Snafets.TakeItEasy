using Snafets.TakeItEasy.Domain.Game;
using Snafets.TakeItEasy.Domain.Lobby;

namespace Snafets.TakeItEasy.Application.Features.Lobby;

public interface ILobbyService
{
    Task<LobbyModel> AddLobbyAsync(string name, Guid creatorId);
    Task<TakeItEasyGame?> DeleteLobbyAndStartGameAsync(Guid lobbyId);
    Task<LobbyModel?> GetLobbyAsync(Guid lobbyId);
    Task<LobbyModel?> UpdateLobby_AddPlayerAsync(Guid lobbyId, Guid playerId);
    Task<bool> UpdateLobby_RemovePlayerAsync(Guid lobbyId, Guid playerId);
}
