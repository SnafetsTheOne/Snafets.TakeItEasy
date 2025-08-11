using Snafets.TakeItEasy.Domain;

namespace Snafets.TakeItEasy.Application
{
    /// <summary>
    /// Application service for managing Take It Easy games.
    /// </summary>
    public class GameService
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
            if (game == null)
                return false;

            var playerBoard = game.PlayerBoards?.Find(pb => pb.Player != null && pb.Player.Id == playerId);
            if (playerBoard == null)
                return false;

            var tile = game.TileSet?.Find(t => t.Id == tileId);
            if (tile == null)
                return false;

            var topTile = game.CallerBag?.PeekTopTile();
            if (topTile == null || topTile.Id != tileId)
                return false;

            return playerBoard.TryAddTileAtIndex(tile, index);
        }
    }
}
