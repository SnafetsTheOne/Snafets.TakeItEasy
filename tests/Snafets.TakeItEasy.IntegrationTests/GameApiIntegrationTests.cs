using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Snafets.TakeItEasy.Domain.Game;
using Snafets.TakeItEasy.Application.Features.Game;
using Snafets.TakeItEasy.Domain;

namespace Snafets.TakeItEasy.IntegrationTests
{
    public class GameApiIntegrationTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task CreateGame_ThenGetGame_ReturnsCreatedGame()
        {
            // Arrange: create a new game with two players
            var players = new List<PlayerModel>
            {
                new PlayerModel { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Alice" },
                new PlayerModel { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Name = "Bob" }
            };
            var playersJson = System.Text.Json.JsonSerializer.Serialize(players);
            var createResponse = await _client.PostAsync(
                "/api/game/create",
                new StringContent(playersJson, System.Text.Encoding.UTF8, "application/json")
            );
            createResponse.EnsureSuccessStatusCode();
            var createdGame = await createResponse.Content.ReadFromJsonAsync<TakeItEasyGameDetail>();
            Assert.NotNull(createdGame);
            Assert.NotEqual(Guid.Empty, createdGame.Id);

            // Act: get the game by id
            var getResponse = await _client.GetAsync($"/api/game/{createdGame.Id}");
            getResponse.EnsureSuccessStatusCode();
            var fetchedGame = await getResponse.Content.ReadFromJsonAsync<TakeItEasyGameDetail>();

            // Assert: the fetched game matches the created game
            Assert.NotNull(fetchedGame);
            Assert.Equal(createdGame.Id, fetchedGame.Id);
        }

        [Fact]
        public async Task TwoPlayers_TwoMovesEach_ShouldUpdateGameStateCorrectly()
        {
            var client = _client;
            // Create a new game with two players
            var player1Id = Guid.NewGuid();
            var player2Id = Guid.NewGuid();
            var players = new[] {
                new PlayerModel { Id = player1Id, Name = "Alice" },
                new PlayerModel { Id = player2Id, Name = "Bob" }
            };
            var playersJson = System.Text.Json.JsonSerializer.Serialize(players);
            var createResponse = await client.PostAsync(
                "/api/game/create",
                new StringContent(playersJson, System.Text.Encoding.UTF8, "application/json")
            );
            createResponse.EnsureSuccessStatusCode();
            var createdGame = await createResponse.Content.ReadFromJsonAsync<TakeItEasyGameDetail>();
            Assert.NotNull(createdGame);
            Assert.NotNull(createdGame.Id);

            // Each player plays 2 moves
            for (int move = 0; move < 2; move++)
            {
                foreach (var player in players)
                {
                    // Fetch latest game state to get the current top tile
                    var gameStateResponse = await client.GetAsync($"/api/game/{createdGame.Id}");
                    gameStateResponse.EnsureSuccessStatusCode();
                    var gameState = await gameStateResponse.Content.ReadFromJsonAsync<TakeItEasyGameDetail>();
                    Assert.NotNull(gameState);
                    var topTile = gameState.CallerBag?.Tiles?[0];
                    Assert.NotNull(topTile);
                    // Find first empty space for this player
                    var playerBoard = gameState.PlayerBoards?.Find(pb => pb.Player?.Id == player.Id);
                    Assert.NotNull(playerBoard);
                    var firstEmptySpace = playerBoard.Spaces?.FirstOrDefault(s => s.PlacedTile == null);
                    Assert.NotNull(firstEmptySpace);
                    var moveResponse = await client.PostAsync(
                        $"/api/game/move?gameId={createdGame.Id}&playerId={player.Id}&index={move}&tileId={topTile.Id}&spaceIndex={firstEmptySpace.Index}",
                        null
                    );
                    moveResponse.EnsureSuccessStatusCode();
                }
            }

            // Assert: Each player should have 2 placed tiles
            var stateResponse = await client.GetAsync($"/api/game/{createdGame.Id}");
            stateResponse.EnsureSuccessStatusCode();
            var stateObj = await stateResponse.Content.ReadFromJsonAsync<TakeItEasyGameDetail>();
            Assert.NotNull(stateObj);
            foreach (var player in players)
            {
                var playerBoard = stateObj.PlayerBoards?.Find(pb => pb.Player?.Id == player.Id);
                Assert.NotNull(playerBoard);
                var placedTiles = playerBoard.Spaces?.Where(s => s.PlacedTile != null).ToList();
                Assert.NotNull(placedTiles);
                Assert.True(placedTiles.Count == 2, $"Player {player.Name} should have 2 placed tiles, got {placedTiles.Count}");
            }
        }

        [Fact]
        public async Task PlayAllMovesWithSeededDrawBag_CheckScore()
        {

            // Get required services
            var scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IGameRepository>();

            // Create game instance for one player
            var player = new PlayerModel { Id = Guid.NewGuid(), Name = "TestPlayer" };

            // Set DrawBag RNG to a known seed
            DrawBag.Rng = new Random(420);
            var game = new TakeItEasyGame(new List<PlayerModel> { player });
            await repo.SaveGameAsync(game);

            // Play all 19 moves via the client
            for (int i = 0; i < 19; i++)
            {
                // Get latest game state
                var stateResponse = await _client.GetAsync($"/api/game/{game.Id}");
                stateResponse.EnsureSuccessStatusCode();
                var state = await stateResponse.Content.ReadFromJsonAsync<TakeItEasyGameDetail>();
                var topTile = state?.CallerBag?.Tiles?[0];
                Assert.NotNull(topTile);
                var playerBoard = state?.PlayerBoards?.Find(pb => pb.Player?.Id == player.Id);
                Assert.NotNull(playerBoard);
                var firstEmptySpace = playerBoard.Spaces?.FirstOrDefault(s => s.PlacedTile == null);
                Assert.NotNull(firstEmptySpace);
                var moveResponse = await _client.PostAsync($"/api/game/move?gameId={game.Id}&playerId={player.Id}&index={firstEmptySpace.Index}&tileId={topTile.Id}", null);
                moveResponse.EnsureSuccessStatusCode();
            }

            // Get final game state and check score
            var finalResponse = await _client.GetAsync($"/api/game/{game.Id}");
            finalResponse.EnsureSuccessStatusCode();
            var finalState = await finalResponse.Content.ReadFromJsonAsync<TakeItEasyGameDetail>();
            var finalPlayerBoard = finalState?.PlayerBoards?.Find(pb => pb.Player?.Id == player.Id);
            Assert.NotNull(finalPlayerBoard);

            // Calculate expected score (deterministic for seed 42)
            // For now, just assert the board is full and score is > 0
            var placedTiles = finalPlayerBoard.Spaces?.Where(s => s.PlacedTile != null).ToList();
            Assert.NotNull(placedTiles);
            Assert.Equal(19, placedTiles.Count);
            // If you know the expected score for seed 42, set it here:
            Assert.Equal(51, finalPlayerBoard.Score);
        }
    }
}
