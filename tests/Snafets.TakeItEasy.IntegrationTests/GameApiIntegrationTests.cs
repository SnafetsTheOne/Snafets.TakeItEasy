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
    }
}
