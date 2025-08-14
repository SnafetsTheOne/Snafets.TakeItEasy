using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Snafets.TakeItEasy.Domain.Game;
using Snafets.TakeItEasy.Application.Features.Game;
using System.Net;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace Snafets.TakeItEasy.IntegrationTests;

public class GameApiIntegrationTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private async Task<PlayerDto> CreatePlayerAsync(string name, string passwordHash)
    {
        var httpClient = factory.CreateClient();
        var request = new { Name = name, PasswordHash = passwordHash };
        var signupResponse = await httpClient.PostAsJsonAsync("/api/player/signup", request);
        signupResponse.EnsureSuccessStatusCode();
        var playerDto = await signupResponse.Content.ReadFromJsonAsync<Api.Contracts.Models.PlayerDto>();
        Assert.NotNull(playerDto);
        var player = new PlayerDto
        {
            Id = playerDto.Id,
            Name = playerDto.Name,
            Cookies = new CookieContainer(),
            HttpClient = httpClient
        };
        var loginResponse = await httpClient.PostAsJsonAsync("/api/player/login", request);
        var cookies = loginResponse.Headers.GetValues(HeaderNames.SetCookie);
        foreach (var cookie in cookies)
        {
            player.Cookies.SetCookies(httpClient.BaseAddress, cookie);
        }
        Assert.NotNull(player);
        Assert.Equal(name, player.Name);
        return player;
    }

    [Fact]
    public async Task NoSetup_CreatePlayersLobbyAndGame_AllSuccessfullyCreated()
    {
        // Arrange: create two players using CreatePlayerAsync
        var alice = await CreatePlayerAsync("TestPlayer1" + Guid.NewGuid(), "hash1");
        var bob = await CreatePlayerAsync("TestPlayer2" + Guid.NewGuid(), "hash2");

        var createLobbyResponse = await alice.PostAsJsonAsync("/api/lobby",
            new { Name = "Test Lobby", CreatorId = alice.Id });
        createLobbyResponse.EnsureSuccessStatusCode();
        var createdLobby = await createLobbyResponse.Content.ReadFromJsonAsync<LobbyDto>();
        var joinLobbyResponse = await bob.PostAsJsonAsync($"/api/lobby/{createdLobby!.Id}/join",
            new { PlayerId = bob.Id });
        joinLobbyResponse.EnsureSuccessStatusCode();
        var startGameResponse = await alice.PostAsJsonAsync($"/api/lobby/{createdLobby.Id}/start", new { });
        startGameResponse.EnsureSuccessStatusCode();
        var createdGame = await startGameResponse.Content.ReadFromJsonAsync<GameDto>();
        Assert.NotNull(createdGame);
        Assert.NotEqual(Guid.Empty, createdGame.Id);

        // Act: get the game by id
        var getResponse = await alice.GetAsync($"/api/game/{createdGame.Id}");
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
        var alice = await CreatePlayerAsync("TestPlayer1" + Guid.NewGuid(), "hash1");
        var bob = await CreatePlayerAsync("TestPlayer2" + Guid.NewGuid(), "hash2");
        var players = new[] { alice, bob };
        var createLobbyResponse = await alice.PostAsJsonAsync("/api/lobby",
            new { Name = "Test Lobby", CreatorId = alice.Id });
        createLobbyResponse.EnsureSuccessStatusCode();
        var createdLobby = await createLobbyResponse.Content.ReadFromJsonAsync<LobbyDto>();
        var joinLobbyResponse = await bob.PostAsJsonAsync($"/api/lobby/{createdLobby!.Id}/join",
            new { PlayerId = bob.Id });
        joinLobbyResponse.EnsureSuccessStatusCode();
        var startGameResponse = await alice.PostAsJsonAsync($"/api/lobby/{createdLobby.Id}/start", new { });
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
                var gameStateResponse = await player.GetAsync($"/api/game/{createdGame.Id}");
                gameStateResponse.EnsureSuccessStatusCode();
                var gameState = await gameStateResponse.Content.ReadFromJsonAsync<GameDto>();
                Assert.NotNull(gameState);
                Assert.NotNull(gameState.CurrentTile);
                // Find first empty space for this player
                var playerBoard = gameState.Boards?.Find(pb => pb.PlayerId == player.Id);
                Assert.NotNull(playerBoard);
                var firstEmptySpace = playerBoard.Spaces?.FirstOrDefault(s => s.PlacedTile == null);
                Assert.NotNull(firstEmptySpace);
                var moveResponse = await player.PostAsJsonAsync($"/api/game/{createdGame.Id}/move",
                    new { PlayerId = player.Id, Index = firstEmptySpace.Index });
                moveResponse.EnsureSuccessStatusCode();
            }
        }

        // Assert: Each player should have 2 placed tiles
        var stateResponse = await alice.GetAsync($"/api/game/{createdGame.Id}");
        stateResponse.EnsureSuccessStatusCode();
        var stateObj = await stateResponse.Content.ReadFromJsonAsync<GameDto>();
        Assert.NotNull(stateObj);
        foreach (var player in players)
        {
            var playerBoard = stateObj.Boards?.Find(pb => pb.PlayerId == player.Id);
            Assert.NotNull(playerBoard);
            var placedTiles = playerBoard.Spaces?.Where(s => s.PlacedTile != null).ToList();
            Assert.NotNull(placedTiles);
            Assert.True(placedTiles.Count == 2, $"Player {player.Name} should have 2 placed tiles, got {placedTiles.Count}");
        }
    }

    [Fact]
    public async Task PlayAllMovesWithSeededDrawBag_CheckScore()
    {
        var player = await CreatePlayerAsync("TestPlayer1" + Guid.NewGuid(), "hash1");

        // Get required services
        var scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        // Set DrawBag RNG to a known seed
        GameModel.Rng = new Random(420);
        var game = new GameModel(new List<Guid> { player.Id!.Value }, "Name");
        await scope.ServiceProvider.GetRequiredService<IGameRepository>().SaveGameAsync(game);

        // Play all 19 moves via the client
        for (int i = 0; i < 19; i++)
        {
            // Get latest game state
            var stateResponse = await player.GetAsync($"/api/game/{game.Id}");
            stateResponse.EnsureSuccessStatusCode();
            var state = await stateResponse.Content.ReadFromJsonAsync<GameDto>();
            var topTile = state?.CurrentTile;
            Assert.NotNull(topTile);
            var playerBoard = state?.Boards?.Find(pb => pb.PlayerId == player.Id);
            Assert.NotNull(playerBoard);
            var firstEmptySpace = playerBoard.Spaces?.FirstOrDefault(s => s.PlacedTile == null);
            Assert.NotNull(firstEmptySpace);
            var moveResponse = await player.PostAsJsonAsync($"/api/game/{game.Id}/move",
                new { PlayerId = player.Id, Index = firstEmptySpace.Index });
            moveResponse.EnsureSuccessStatusCode();
        }

        // Get final game state and check score
        var finalResponse = await player.GetAsync($"/api/game/{game.Id}");
        finalResponse.EnsureSuccessStatusCode();
        var finalState = await finalResponse.Content.ReadFromJsonAsync<GameDto>();
        var finalPlayerBoard = finalState?.Boards?.Find(pb => pb.PlayerId == player.Id);
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

public static class HttpClientExtensions
{
    public static async Task<HttpResponseMessage> GetAsync(this PlayerDto player, string url)
    {
        var uri = new Uri(player.HttpClient.BaseAddress!.AbsoluteUri + url.Substring(1), UriKind.Absolute);
        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        request.Headers.Add("Cookie", player.Cookies.GetCookieHeader(uri));
        return await player.HttpClient.SendAsync(request);
    }

    public static async Task<HttpResponseMessage> PostAsJsonAsync(this PlayerDto player, string url, object data)
    {
        var uri = new Uri(player.HttpClient.BaseAddress!.AbsoluteUri + url.Substring(1), UriKind.Absolute);
        var content = JsonContent.Create(data);
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
        {
            Content = content
        };
        requestMessage.Headers.Add("Cookie", player.Cookies.GetCookieHeader(uri));

        return await player.HttpClient.SendAsync(requestMessage);
    }
}
