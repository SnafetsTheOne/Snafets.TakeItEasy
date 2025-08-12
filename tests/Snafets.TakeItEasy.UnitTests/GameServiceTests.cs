using Snafets.TakeItEasy.Application.Features.Game;
using Snafets.TakeItEasy.Persistence;

namespace Snafets.TakeItEasy.UnitTests;

public class GameServiceTests
{
    [Fact]
    public async Task CreateGameAsync_SetsAllPropertiesCorrectly()
    {
        // Arrange
        var playerIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var repo = new InMemoryGameRepository();
        var service = new GameService(repo);

        // Act
        var game = await service.CreateGameAsync(playerIds);

        // Assert
        Assert.NotNull(game);
        Assert.NotEqual(Guid.Empty, game.Id);
        Assert.NotNull(game.PlayerBoards);
        Assert.Equal(playerIds.Count, game.PlayerBoards.Count);
        foreach (var pb in game.PlayerBoards)
        {
            Assert.Contains(pb.PlayerId, playerIds);
            Assert.NotNull(pb.Spaces);
            Assert.Equal(19, pb.Spaces.Count);
        }
        Assert.NotNull(game.CallerBag);
        Assert.NotNull(game.CallerBag.Tiles);
        Assert.Equal(27, game.CallerBag.Tiles.Count); // 3 verticals x 3 left diagonals x 3 right diagonals = 27
    }
    private static GameService CreateServiceWithPlayerIds(out List<Guid> playerIds)
    {
        playerIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var repo = new InMemoryGameRepository();
        return new GameService(repo);
    }

    [Fact]
    public async Task GetGameAsync_ReturnsCreatedGame()
    {
        var service = CreateServiceWithPlayerIds(out var playerIds);
        var game = await service.CreateGameAsync(playerIds);
        var loaded = await service.GetGameAsync(game.Id);
        Assert.NotNull(loaded);
        Assert.Equal(game.Id, loaded.Id);
    }

    [Fact]
    public async Task GetAllGamesAsync_ReturnsAllCreatedGames()
    {
        var service = CreateServiceWithPlayerIds(out var playerIds);
        await service.CreateGameAsync(playerIds);
        await service.CreateGameAsync(playerIds);
        var allGames = await service.GetAllGamesAsync();
        Assert.Equal(2, allGames.Count);
    }

    [Fact]
    public async Task LoadGameForPlayerAsync_ReturnsGamesForPlayer()
    {
        var service = CreateServiceWithPlayerIds(out var playerIds);
        await service.CreateGameAsync(playerIds);
        await service.CreateGameAsync(playerIds);
        var gamesForAlice = await service.LoadGameForPlayerAsync(playerIds[0]);
        foreach (var game in gamesForAlice)
        {
            Assert.Contains(game.PlayerBoards, pb => pb.PlayerId == playerIds[0]);
        }
    }

    [Fact]
    public async Task AddPlayerMoveAsync_AddsMoveAndAdvancesDrawBagCorrectly()
    {
        var service = CreateServiceWithPlayerIds(out var playerIds);
        var game = await service.CreateGameAsync(playerIds);
        var gameId = game.Id;
        var playerId = playerIds[0];
        var topTile = game.CallerBag.PeekTopTile();
        Assert.NotNull(topTile);
        int index = 0;
        // Add move for first player
        var result = await service.AddPlayerMoveAsync(gameId, playerId, index);
        Assert.True(result);
        // Add move for second player
        var result2 = await service.AddPlayerMoveAsync(gameId, playerIds[1], index);
        Assert.True(result2);
        // After both players have placed, draw bag should advance
        var newTopTile = game.CallerBag.PeekTopTile();
        Assert.NotEqual(topTile.Id, newTopTile?.Id);
    }

    [Fact]
    public async Task AddPlayerMoveAsync_FailsForInvalidTileOrIndex()
    {
        var service = CreateServiceWithPlayerIds(out var playerIds);
        var game = await service.CreateGameAsync(playerIds);
        var gameId = game.Id;
        var playerId = playerIds[0];
        var topTile = game.CallerBag.PeekTopTile();
        Assert.NotNull(topTile);
        // Invalid index
        var result = await service.AddPlayerMoveAsync(gameId, playerId, -1);
        Assert.False(result);
    }

    [Fact]
    public async Task AddPlayerMoveAsync_DoesNotAdvanceDrawBagIfNotAllPlayersPlaced()
    {
        var service = CreateServiceWithPlayerIds(out var playerIds);
        var game = await service.CreateGameAsync(playerIds);
        var gameId = game.Id;
        var playerId = playerIds[0];
        var topTile = game.CallerBag.PeekTopTile();
        Assert.NotNull(topTile);
        int index = 0;
        // Add move for only one player
        var result = await service.AddPlayerMoveAsync(gameId, playerId, index);
        Assert.True(result);
        // Draw bag should NOT advance yet
        var stillTopTile = game.CallerBag.PeekTopTile();
        Assert.Equal(topTile.Id, stillTopTile?.Id);
    }
}
