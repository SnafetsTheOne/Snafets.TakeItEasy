using Snafets.TakeItEasy.Domain;

namespace Snafets.TakeItEasy.Application
{
    public interface IGameRepository
    {
        Task SaveGameAsync(Snafets.TakeItEasy.Domain.TakeItEasyGame game);
        Task<TakeItEasyGame?> LoadGameAsync(Guid id);
        Task<IEnumerable<TakeItEasyGame>> GetAllGamesAsync();
        Task DeleteGameAsync(Guid id);
    }
}
