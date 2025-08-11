using Snafets.TakeItEasy.Domain;

namespace Snafets.TakeItEasy.Application
{
    /// <summary>
    /// Interface for application service managing Take It Easy games.
    /// </summary>
    public interface IGameService
    {
        Task<TakeItEasyGame> CreateGameAsync(List<Player> players);
        Task<TakeItEasyGame?> GetGameAsync(Guid id);
        Task<bool> AddPlayerMoveAsync(Guid gameId, Guid playerId, int index);
        Task<List<TakeItEasyGame>> GetAllGamesAsync();
        Task<List<TakeItEasyGame>> LoadGameForPlayerAsync(Guid playerId);
    }
}
