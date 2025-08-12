using Snafets.TakeItEasy.Domain.Game;

namespace Snafets.TakeItEasy.Application.Features.Game;

public interface IGameRepository
{
    Task SaveGameAsync(GameModel game);
    Task<GameModel?> LoadGameAsync(Guid id);
    Task<IEnumerable<GameModel>> GetAllGamesAsync();
    Task DeleteGameAsync(Guid id);
}
