using Snafets.TakeItEasy.Domain;

namespace Snafets.TakeItEasy.UnitTests;

public class PlayerBoardTests
{
    [Fact]
    public void Constructor_InitializesSpacesAndPlayer()
    {
        var player = new Player { Id = Guid.NewGuid(), Name = "TestPlayer" };
        var board = new PlayerBoard(player);
        Assert.Equal(player, board.Player);
        Assert.NotNull(board.Spaces);
        Assert.Equal(19, board.Spaces.Count);
        Assert.All(board.Spaces, s => Assert.Null(s.PlacedTile));
    }

    [Fact]
    public void ContainsTile_ReturnsFalseForEmptyBoard()
    {
        var player = new Player { Id = Guid.NewGuid(), Name = "TestPlayer" };
        var board = new PlayerBoard(player);
        var tile = new Tile(1, 1, 2, 3);
        Assert.False(board.ContainsTile(tile));
    }

    [Fact]
    public void CalculateScore_WithMultipleTilesSet_ReturnsNonZeroScore()
    {
        // Arrange
        var player = new Player { Id = Guid.NewGuid(), Name = "TestPlayer" };
        var playerBoard = new PlayerBoard(player);
        // Set multiple tiles (example positions and values)
        playerBoard.TryAddTileAtIndex(new Tile(1, 1, 3, 2), 0);
        playerBoard.TryAddTileAtIndex(new Tile(2, 5, 3, 2), 1);
        playerBoard.TryAddTileAtIndex(new Tile(3, 9, 3, 2), 2);

        // Act
        int score = playerBoard.CalculateScore();

        // Assert
        Assert.Equal(9, score);
    }

    [Fact]
    public void TryAddTileAtIndex_AddsTileCorrectly()
    {
        var player = new Player { Id = Guid.NewGuid(), Name = "TestPlayer" };
        var board = new PlayerBoard(player);
        var tile = new Tile(1, 1, 2, 3);
        bool result = board.TryAddTileAtIndex(tile, 0);
        Assert.True(result);
        Assert.Equal(tile, board.Spaces[0].PlacedTile);
        Assert.True(board.ContainsTile(tile));
    }

    [Fact]
    public void TryAddTileAtIndex_FailsForInvalidIndexOrDuplicate()
    {
        var player = new Player { Id = Guid.NewGuid(), Name = "TestPlayer" };
        var board = new PlayerBoard(player);
        var tile = new Tile(1, 1, 2, 3);
        Assert.False(board.TryAddTileAtIndex(tile, -1));
        Assert.False(board.TryAddTileAtIndex(tile, 19));
        board.TryAddTileAtIndex(tile, 0);
        Assert.False(board.TryAddTileAtIndex(tile, 0)); // already placed
        Assert.False(board.TryAddTileAtIndex(tile, 1)); // duplicate tile
    }
}
