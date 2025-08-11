using Snafets.TakeItEasy.Domain;

namespace Snafets.TakeItEasy.Application
{
    /// <summary>
    /// Application service for managing Take It Easy games.
    /// </summary>
    public class GameService : IGameService
    {
        private readonly IGameRepository _repository;

        public GameService(IGameRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Creates a new game for the given players and adds it to the repository.
        /// </summary>
        public async Task<TakeItEasyGame> CreateGameAsync(List<Player> players)
        {
            var game = new TakeItEasyGame(players);
            await _repository.SaveGameAsync(game);
            return game;
        }

        public async Task<List<TakeItEasyGame>> GetAllGamesAsync()
        {
            // Assuming repository has a method to get all games
            var games = await _repository.GetAllGamesAsync();
            return games.ToList();
        }

        public async Task<List<TakeItEasyGame>> LoadGameForPlayerAsync(Guid playerId)
        {
            var allGames = await _repository.GetAllGamesAsync();
            return allGames.Where(g => g.PlayerBoards.Any(pb => pb.Player.Id == playerId)).ToList();
        }

        /// <summary>
        /// Gets a game by its unique ID.
        /// </summary>
        public async Task<TakeItEasyGame?> GetGameAsync(Guid id)
        {
            return await _repository.LoadGameAsync(id);
        }

        /// <summary>
        /// Adds a move for the given player in the specified game.
        /// </summary>
        /// <param name="game">The game instance.</param>
        /// <param name="player">The player making the move.</param>
        /// <param name="index">The index on the board.</param>
        /// <param name="tile">The tile to place.</param>
        /// <returns>True if the move was successful; otherwise, false.</returns>
        public async Task<bool> AddPlayerMoveAsync(Guid gameId, Guid playerId, int index, int tileId)
        {
            var game = await _repository.LoadGameAsync(gameId);
            if (game is null)
                return false;

            var playerBoard = game.PlayerBoards.Find(pb => pb.Player.Id == playerId);
            if (playerBoard is null)
                return false;

            var topTile = game.CallerBag.PeekTopTile();
            if (topTile is null || topTile.Id != tileId)
                return false;

            var moveResult = playerBoard.TryAddTileAtIndex(topTile, index);
            // Advance the draw bag if all players have placed the top tile
            game.TryAdvanceDrawBagIfAllPlayersPlacedTopTile();
            return moveResult;
        }
    }
}
