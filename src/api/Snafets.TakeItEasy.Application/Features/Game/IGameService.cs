using Snafets.TakeItEasy.Domain;
using Snafets.TakeItEasy.Domain.Game;

namespace Snafets.TakeItEasy.Application.Features.Game;

/// <summary>
/// Interface for application service managing Take It Easy games.
/// </summary>
public interface IGameService
{
    Task<GameModel> CreateGameAsync(List<Guid> playerIds, string name);
    Task<GameModel?> GetGameAsync(Guid id);
    Task<GameModel?> AddPlayerMoveAsync(Guid gameId, Guid playerId, int index);
    Task<List<GameModel>> GetGamesByPlayerIdAsync(Guid playerId);
}
