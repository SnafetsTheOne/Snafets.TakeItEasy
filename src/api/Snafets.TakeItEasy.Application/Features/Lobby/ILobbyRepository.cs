using Snafets.TakeItEasy.Domain.Lobby;

namespace Snafets.TakeItEasy.Application.Features.Lobby;

public interface ILobbyRepository
{
    Task<LobbyModel> AddLobbyAsync(string name, Guid creatorId);
    Task<LobbyModel?> UpdateLobby_AddPlayerAsync(Guid lobbyId, Guid playerId);
    Task<LobbyModel?> UpdateLobby_RemovePlayerAsync(Guid lobbyId, Guid playerId);
    Task<LobbyModel?> GetLobbyAsync(Guid lobbyId);
    Task<bool> DeleteLobbyAsync(Guid lobbyId);
    Task<IEnumerable<LobbyModel>> GetAllLobbiesAsync();
}
