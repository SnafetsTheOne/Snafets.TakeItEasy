using Snafets.TakeItEasy.Domain.Game;

namespace Snafets.TakeItEasy.Application.Features.Game;

public interface IGameRepository
{
    Task SaveGameAsync(GameModel game);
    Task<GameModel?> GetGameAsync(Guid id);
    Task<IEnumerable<GameModel>> GetAllGamesAsync(Guid playerId);
    Task DeleteGameAsync(Guid id);
}
