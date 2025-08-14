using Snafets.TakeItEasy.Domain.Lobby;

namespace Snafets.TakeItEasy.Application.Features.Lobby;

public interface ILobbyRepository
{
    Task<LobbyModel?> GetLobbyAsync(Guid lobbyId);
    Task<bool> DeleteLobbyAsync(Guid lobbyId);
    Task<IEnumerable<LobbyModel>> GetAllLobbiesAsync();
    Task SaveLobbyAsync(LobbyModel lobby);
}
