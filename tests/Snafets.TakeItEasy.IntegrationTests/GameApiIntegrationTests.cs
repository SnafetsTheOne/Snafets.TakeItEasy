using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Snafets.TakeItEasy.Domain;

namespace Snafets.TakeItEasy.IntegrationTests
{
    public class GameApiIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public GameApiIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateGame_ThenGetGame_ReturnsCreatedGame()
        {
            // Arrange: create a new game with two players
            var players = new List<Player>
            {
                new Player { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), Name = "Alice" },
                new Player { Id = Guid.Parse("00000000-0000-0000-0000-000000000002"), Name = "Bob" }
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
        public async Task PlayThreeMoves_ShouldUpdateGameStateCorrectly()
        {
            // Arrange
            var client = _client;
            // Create a new game with one player
            var playerId = Guid.NewGuid();
            var players = new[] { new Player { Id = playerId, Name = "player1" } };
            var playersJson = System.Text.Json.JsonSerializer.Serialize(players);
            var createResponse = await client.PostAsync(
                "/api/game/create",
                new StringContent(playersJson, System.Text.Encoding.UTF8, "application/json")
            );
            createResponse.EnsureSuccessStatusCode();
            var createdGame = await createResponse.Content.ReadFromJsonAsync<TakeItEasyGameDetail>();
            Assert.NotNull(createdGame);
            Assert.NotNull(createdGame.Id);

            // Play 3 moves using the top tile from the draw bag, updating after each move
            for (int i = 0; i < 3; i++)
            {
                // Fetch latest game state to get the current top tile
                var gameStateResponse = await client.GetAsync($"/api/game/{createdGame.Id}");
                gameStateResponse.EnsureSuccessStatusCode();
                var gameState = await gameStateResponse.Content.ReadFromJsonAsync<TakeItEasyGameDetail>();
                Assert.NotNull(gameState);
                var topTile = gameState.CallerBag?.Tiles?[0];
                Assert.NotNull(topTile);
                var moveResponse = await client.PostAsync(
                    $"/api/game/move?gameId={createdGame.Id}&playerId={playerId}&index={i}&tileId={topTile.Id}",
                    null
                );
                moveResponse.EnsureSuccessStatusCode();
            }

            // Assert: Get game state and check moves
            var stateResponse = await client.GetAsync($"/api/game/{createdGame.Id}");
            stateResponse.EnsureSuccessStatusCode();
            var stateObj = await stateResponse.Content.ReadFromJsonAsync<TakeItEasyGameDetail>();
            Assert.NotNull(stateObj);
            var playerBoard = stateObj.PlayerBoards?.Find(pb => pb.Player?.Id == playerId);
            Assert.NotNull(playerBoard);
            var placedTiles = playerBoard.Spaces?.Where(s => s.PlacedTile != null).ToList();
            Assert.NotNull(placedTiles);
            Assert.True(placedTiles.Count >= 3, $"Expected at least 3 placed tiles, got {placedTiles.Count}");
        }
    }
}
