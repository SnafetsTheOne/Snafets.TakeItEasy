using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Snafets.TakeItEasy.Domain.Game;
using Snafets.TakeItEasy.Application.Features.Game;
using Snafets.TakeItEasy.Domain;
using Snafets.TakeItEasy.Application.Features.Player;

namespace Snafets.TakeItEasy.IntegrationTests;

public class GameApiIntegrationTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    private async Task<PlayerModel> CreatePlayerAsync(string name, string passwordHash)
    {
        var request = new { Name = name, PasswordHash = passwordHash };
        var response = await _client.PostAsJsonAsync("/api/player/signup", request);
        response.EnsureSuccessStatusCode();
        var player = await response.Content.ReadFromJsonAsync<PlayerModel>();
        Assert.NotNull(player);
        Assert.Equal(name, player.Name);
        return player;
    }

    [Fact]
    public async Task NoSetup_CreatePlayersLobbyAndGame_AllSuccessfullyCreated()
    {
        // Arrange: create two players using CreatePlayerAsync
        var alice = await CreatePlayerAsync("Alice", "hash1");
        var bob = await CreatePlayerAsync("Bob", "hash2");
        var players = new List<PlayerModel> { alice, bob };
        var createLobbyResponse = await _client.PostAsJsonAsync("/api/lobby",
            new { Name = "Test Lobby", CreatorId = alice.Id });
        createLobbyResponse.EnsureSuccessStatusCode();
        var createdLobby = await createLobbyResponse.Content.ReadFromJsonAsync<LobbyDto>();
        var joinLobbyResponse = await _client.PostAsJsonAsync($"/api/lobby/{createdLobby!.Id}/join",
            new { PlayerId = bob.Id });
        joinLobbyResponse.EnsureSuccessStatusCode();
        var startGameResponse = await _client.PostAsJsonAsync($"/api/lobby/{createdLobby.Id}/start", new { });
        startGameResponse.EnsureSuccessStatusCode();
        var createdGame = await startGameResponse.Content.ReadFromJsonAsync<GameDto>();
        Assert.NotNull(createdGame);
        Assert.NotEqual(Guid.Empty, createdGame.Id);

        // Act: get the game by id
        var getResponse = await _client.GetAsync($"/api/game/{createdGame.Id}");
        getResponse.EnsureSuccessStatusCode();
        var fetchedGame = await getResponse.Content.ReadFromJsonAsync<GameDto>();

        // Assert: the fetched game matches the created game
        Assert.NotNull(fetchedGame);
        Assert.Equal(createdGame.Id, fetchedGame.Id);
    }

    [Fact]
    public async Task NoSetup_CreatePlayersLobbyAndGame_CanPlay()
    {
        // Create two players using CreatePlayerAsync
        var alice = await CreatePlayerAsync("Alice", "hash1");
        var bob = await CreatePlayerAsync("Bob", "hash2");
        var players = new[] { alice, bob };
        var createLobbyResponse = await _client.PostAsJsonAsync("/api/lobby",
            new { Name = "Test Lobby", CreatorId = alice.Id });
        createLobbyResponse.EnsureSuccessStatusCode();
        var createdLobby = await createLobbyResponse.Content.ReadFromJsonAsync<LobbyDto>();
        var joinLobbyResponse = await _client.PostAsJsonAsync($"/api/lobby/{createdLobby!.Id}/join",
            new { PlayerId = bob.Id });
        joinLobbyResponse.EnsureSuccessStatusCode();
        var startGameResponse = await _client.PostAsJsonAsync($"/api/lobby/{createdLobby.Id}/start", new { });
        startGameResponse.EnsureSuccessStatusCode();
        var createdGame = await startGameResponse.Content.ReadFromJsonAsync<GameDto>();
        Assert.NotNull(createdGame);
        Assert.NotNull(createdGame.Id);

        // Each player plays 2 moves
        for (int move = 0; move < 2; move++)
        {
            foreach (var player in players)
            {
                // Fetch latest game state to get the current top tile
                var gameStateResponse = await _client.GetAsync($"/api/game/{createdGame.Id}");
                gameStateResponse.EnsureSuccessStatusCode();
                var gameState = await gameStateResponse.Content.ReadFromJsonAsync<GameDto>();
                Assert.NotNull(gameState);
                var topTile = gameState.NextTile;
                Assert.NotNull(topTile);
                // Find first empty space for this player
                var playerBoard = gameState.PlayerBoards?.Find(pb => pb.PlayerId == player.Id);
                Assert.NotNull(playerBoard);
                var firstEmptySpace = playerBoard.Spaces?.FirstOrDefault(s => s.PlacedTile == null);
                Assert.NotNull(firstEmptySpace);
                var moveResponse = await _client.PostAsJsonAsync($"/api/game/{createdGame.Id}/move",
                    new { PlayerId = player.Id, Index = firstEmptySpace.Index }
                );
                moveResponse.EnsureSuccessStatusCode();
            }
        }

        // Assert: Each player should have 2 placed tiles
        var stateResponse = await _client.GetAsync($"/api/game/{createdGame.Id}");
        stateResponse.EnsureSuccessStatusCode();
        var stateObj = await stateResponse.Content.ReadFromJsonAsync<GameDto>();
        Assert.NotNull(stateObj);
        foreach (var player in players)
        {
            var playerBoard = stateObj.PlayerBoards?.Find(pb => pb.PlayerId == player.Id);
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
        var player = await scope.ServiceProvider.GetRequiredService<IPlayerRepository>().AddPlayerAsync(new PlayerModel
        {
            Id = Guid.NewGuid(),
            Name = "TestPlayer",
            PasswordHash = "Hash"
        });
        // Set DrawBag RNG to a known seed
        DrawBag.Rng = new Random(420);
        var game = new GameModel(new List<Guid> { player.Id }, "Name");
        await scope.ServiceProvider.GetRequiredService<IGameRepository>().SaveGameAsync(game);

        // Play all 19 moves via the client
        for (int i = 0; i < 19; i++)
        {
            // Get latest game state
            var stateResponse = await _client.GetAsync($"/api/game/{game.Id}");
            stateResponse.EnsureSuccessStatusCode();
            var state = await stateResponse.Content.ReadFromJsonAsync<GameDto>();
            var topTile = state?.NextTile;
            Assert.NotNull(topTile);
            var playerBoard = state?.PlayerBoards?.Find(pb => pb.PlayerId == player.Id);
            Assert.NotNull(playerBoard);
            var firstEmptySpace = playerBoard.Spaces?.FirstOrDefault(s => s.PlacedTile == null);
            Assert.NotNull(firstEmptySpace);
            var moveResponse = await _client.PostAsJsonAsync($"/api/game/{game.Id}/move",
                new { PlayerId = player.Id, Index = firstEmptySpace.Index }
            );
            moveResponse.EnsureSuccessStatusCode();
        }

        // Get final game state and check score
        var finalResponse = await _client.GetAsync($"/api/game/{game.Id}");
        finalResponse.EnsureSuccessStatusCode();
        var finalState = await finalResponse.Content.ReadFromJsonAsync<GameDto>();
        var finalPlayerBoard = finalState?.PlayerBoards?.Find(pb => pb.PlayerId == player.Id);
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
