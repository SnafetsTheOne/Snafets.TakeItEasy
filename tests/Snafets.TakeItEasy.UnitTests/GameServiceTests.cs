using Snafets.TakeItEasy.Application;
using Snafets.TakeItEasy.Domain;
using Snafets.TakeItEasy.Persistence;

namespace Snafets.TakeItEasy.UnitTests
{
    public class GameServiceTests
    {
        [Fact]
        public async Task CreateGameAsync_SetsAllPropertiesCorrectly()
        {
            // Arrange
            var players = new List<Player>
            {
                new Player { Id = Guid.NewGuid(), Name = "Alice" },
                new Player { Id = Guid.NewGuid(), Name = "Bob" }
            };
            var repo = new InMemoryGameRepository();
            var service = new GameService(repo);

            // Act
            var game = await service.CreateGameAsync(players);

            // Assert
            Assert.NotNull(game);
            Assert.NotEqual(Guid.Empty, game.Id);
            Assert.NotNull(game.PlayerBoards);
            Assert.Equal(players.Count, game.PlayerBoards.Count);
            foreach (var pb in game.PlayerBoards)
            {
                Assert.NotNull(pb.Player);
                Assert.Contains(pb.Player, players);
                Assert.NotNull(pb.Spaces);
                Assert.Equal(19, pb.Spaces.Count);
            }
            Assert.NotNull(game.CallerBag);
            Assert.NotNull(game.CallerBag.Tiles);
            Assert.Equal(27, game.CallerBag.Tiles.Count); // 3 verticals x 3 left diagonals x 3 right diagonals = 27
        }
        private GameService CreateServiceWithPlayers(out List<Player> players)
        {
            players = new List<Player>
            {
                new Player { Id = Guid.NewGuid(), Name = "Alice" },
                new Player { Id = Guid.NewGuid(), Name = "Bob" }
            };
            var repo = new InMemoryGameRepository();
            return new GameService(repo);
        }

        [Fact]
        public async Task GetGameAsync_ReturnsCreatedGame()
        {
            var service = CreateServiceWithPlayers(out var players);
            var game = await service.CreateGameAsync(players);
            var loaded = await service.GetGameAsync(game.Id);
            Assert.NotNull(loaded);
            Assert.Equal(game.Id, loaded.Id);
        }

        [Fact]
        public async Task GetAllGamesAsync_ReturnsAllCreatedGames()
        {
            var service = CreateServiceWithPlayers(out var players);
            var game1 = await service.CreateGameAsync(players);
            var game2 = await service.CreateGameAsync(players);
            var allGames = await service.GetAllGamesAsync();
            Assert.Contains(allGames, g => g.Id == game1.Id);
            Assert.Contains(allGames, g => g.Id == game2.Id);
            Assert.Equal(2, allGames.Count);
        }

        [Fact]
        public async Task LoadGameForPlayerAsync_ReturnsGamesForPlayer()
        {
            var service = CreateServiceWithPlayers(out var players);
            var game1 = await service.CreateGameAsync(players);
            var game2 = await service.CreateGameAsync(players);
            var gamesForAlice = await service.LoadGameForPlayerAsync(players[0].Id);
            Assert.All(gamesForAlice, g => Assert.Contains(g.PlayerBoards, pb => pb.Player.Id == players[0].Id));
        }

        [Fact]
        public async Task AddPlayerMoveAsync_AddsMoveAndAdvancesDrawBagCorrectly()
        {
            var service = CreateServiceWithPlayers(out var players);
            var game = await service.CreateGameAsync(players);
            var gameId = game.Id;
            var playerId = players[0].Id;
            var topTile = game.CallerBag.PeekTopTile();
            Assert.NotNull(topTile);
            int index = 0;
            // Add move for first player
            var result = await service.AddPlayerMoveAsync(gameId, playerId, index, topTile.Id);
            Assert.True(result);
            // Add move for second player
            var result2 = await service.AddPlayerMoveAsync(gameId, players[1].Id, index, topTile.Id);
            Assert.True(result2);
            // After both players have placed, draw bag should advance
            var newTopTile = game.CallerBag.PeekTopTile();
            Assert.NotEqual(topTile.Id, newTopTile.Id);
        }

        [Fact]
        public async Task AddPlayerMoveAsync_FailsForInvalidTileOrIndex()
        {
            var service = CreateServiceWithPlayers(out var players);
            var game = await service.CreateGameAsync(players);
            var gameId = game.Id;
            var playerId = players[0].Id;
            var topTile = game.CallerBag.PeekTopTile();
            Assert.NotNull(topTile);
            // Invalid index
            var result = await service.AddPlayerMoveAsync(gameId, playerId, -1, topTile.Id);
            Assert.False(result);
            // Invalid tile id
            var result2 = await service.AddPlayerMoveAsync(gameId, playerId, 0, -999);
            Assert.False(result2);
        }

        [Fact]
        public async Task AddPlayerMoveAsync_DoesNotAdvanceDrawBagIfNotAllPlayersPlaced()
        {
            var service = CreateServiceWithPlayers(out var players);
            var game = await service.CreateGameAsync(players);
            var gameId = game.Id;
            var playerId = players[0].Id;
            var topTile = game.CallerBag.PeekTopTile();
            Assert.NotNull(topTile);
            int index = 0;
            // Add move for only one player
            var result = await service.AddPlayerMoveAsync(gameId, playerId, index, topTile.Id);
            Assert.True(result);
            // Draw bag should NOT advance yet
            var stillTopTile = game.CallerBag.PeekTopTile();
            Assert.Equal(topTile.Id, stillTopTile.Id);
        }
    }
}
