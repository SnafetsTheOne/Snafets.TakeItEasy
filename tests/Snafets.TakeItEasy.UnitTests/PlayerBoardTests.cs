using Snafets.TakeItEasy.Domain;
using Snafets.TakeItEasy.Domain.Game;

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
    public void CalculateScore_VerticalLine_ReturnsNonZeroScore()
    {
        // Arrange
        var player = new Player { Id = Guid.NewGuid(), Name = "TestPlayer" };
        var playerBoard = new PlayerBoard(player);

        var verticalIndices = PlayerBoard.TakeItEasyLines.Vertical[2];

        for (int i = 0; i < verticalIndices.Length; i++)
        {
            playerBoard.TryAddTileAtIndex(new Tile(i, 1, 2, 3), verticalIndices[i]);
        }
        // Act
        int score = playerBoard.CalculateScore();

        // Assert
        Assert.Equal(1 * verticalIndices.Length, score);
    }
    
    [Fact]
    public void CalculateScore_ShouldReturnCorrectScore_ForLeftDiagonalLine()
    {
        // Arrange
        var player = new Player { Id = Guid.NewGuid(), Name = "TestPlayer" };
        var playerBoard = new PlayerBoard(player);
        // Fill the longest left diagonal line: indices {0, 4, 9, 14, 18}
        var leftDiagonalIndices = PlayerBoard.TakeItEasyLines.LeftDiagonal[2];
        // All tiles have leftDiagonal=2
        for (int i = 0; i < leftDiagonalIndices.Length; i++)
        {
            playerBoard.TryAddTileAtIndex(new Tile(i, 1, 2, 3), leftDiagonalIndices[i]);
        }
        // Act
        int score = playerBoard.CalculateScore();
        // Assert
        Assert.Equal(2 * leftDiagonalIndices.Length, score);
    }

    [Fact]
    public void CalculateScore_ShouldReturnCorrectScore_ForRightDiagonalLine()
    {
        // Arrange
        var player = new Player { Id = Guid.NewGuid(), Name = "TestPlayer" };
        var playerBoard = new PlayerBoard(player);
        // Fill the longest right diagonal line: indices {2, 5, 9, 13, 16}
        var rightDiagonalIndices = PlayerBoard.TakeItEasyLines.RightDiagonal[2];
        // All tiles have rightDiagonal=3
        for (int i = 0; i < rightDiagonalIndices.Length; i++)
        {
            playerBoard.TryAddTileAtIndex(new Tile(i, 1, 2, 3), rightDiagonalIndices[i]);
        }
        // Act
        int score = playerBoard.CalculateScore();
        // Assert
        Assert.Equal(3 * rightDiagonalIndices.Length, score);
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
