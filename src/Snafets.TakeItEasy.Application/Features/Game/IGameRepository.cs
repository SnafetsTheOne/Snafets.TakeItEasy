using Snafets.TakeItEasy.Domain.Game;

namespace Snafets.TakeItEasy.Application.Features.Game
{
    public interface IGameRepository
    {
        Task SaveGameAsync(TakeItEasyGame game);
        Task<TakeItEasyGame?> LoadGameAsync(Guid id);
        Task<IEnumerable<TakeItEasyGame>> GetAllGamesAsync();
        Task DeleteGameAsync(Guid id);
    }
}
