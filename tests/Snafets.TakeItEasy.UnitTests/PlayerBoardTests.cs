using Snafets.TakeItEasy.Domain;
using Snafets.TakeItEasy.Domain.Game;

namespace Snafets.TakeItEasy.UnitTests;

public class PlayerBoardTests
{
    [Fact]
    public void Constructor_InitializesSpacesAndPlayerId()
    {
        var playerId = Guid.NewGuid();
        var board = new PlayerBoard(playerId);
        Assert.Equal(playerId, board.PlayerId);
        Assert.NotNull(board.Spaces);
        Assert.Equal(19, board.Spaces.Count);
        Assert.All(board.Spaces, s => Assert.Null(s.PlacedTile));
    }

    [Fact]
    public void ContainsTile_ReturnsFalseForEmptyBoard()
    {
        var playerId = Guid.NewGuid();
        var board = new PlayerBoard(playerId);
        var tile = new Tile(1, 1, 2, 3);
        Assert.False(board.ContainsTile(tile));
    }

    [Fact]
    public void CalculateScore_VerticalLine_ReturnsNonZeroScore()
    {
        var playerId = Guid.NewGuid();
        var playerBoard = new PlayerBoard(playerId);
        var verticalIndices = PlayerBoard.TakeItEasyLines.Vertical[2];
        for (int i = 0; i < verticalIndices.Length; i++)
        {
            playerBoard.TryAddTileAtIndex(new Tile(i, 1, 2, 3), verticalIndices[i]);
        }
        int score = playerBoard.CalculateScore();
        Assert.Equal(1 * verticalIndices.Length, score);
    }

    [Fact]
    public void CalculateScore_ShouldReturnCorrectScore_ForLeftDiagonalLine()
    {
        var playerId = Guid.NewGuid();
        var playerBoard = new PlayerBoard(playerId);
        var leftDiagonalIndices = PlayerBoard.TakeItEasyLines.LeftDiagonal[2];
        for (int i = 0; i < leftDiagonalIndices.Length; i++)
        {
            playerBoard.TryAddTileAtIndex(new Tile(i, 1, 2, 3), leftDiagonalIndices[i]);
        }
        int score = playerBoard.CalculateScore();
        Assert.Equal(2 * leftDiagonalIndices.Length, score);
    }

    [Fact]
    public void CalculateScore_ShouldReturnCorrectScore_ForRightDiagonalLine()
    {
        var playerId = Guid.NewGuid();
        var playerBoard = new PlayerBoard(playerId);
        var rightDiagonalIndices = PlayerBoard.TakeItEasyLines.RightDiagonal[2];
        for (int i = 0; i < rightDiagonalIndices.Length; i++)
        {
            playerBoard.TryAddTileAtIndex(new Tile(i, 1, 2, 3), rightDiagonalIndices[i]);
        }
        int score = playerBoard.CalculateScore();
        Assert.Equal(3 * rightDiagonalIndices.Length, score);
    }

    [Fact]
    public void TryAddTileAtIndex_AddsTileCorrectly()
    {
        var playerId = Guid.NewGuid();
        var board = new PlayerBoard(playerId);
        var tile = new Tile(1, 1, 2, 3);
        bool result = board.TryAddTileAtIndex(tile, 0);
        Assert.True(result);
        Assert.Equal(tile, board.Spaces[0].PlacedTile);
        Assert.True(board.ContainsTile(tile));
    }

    [Fact]
    public void TryAddTileAtIndex_FailsForInvalidIndexOrDuplicate()
    {
        var playerId = Guid.NewGuid();
        var board = new PlayerBoard(playerId);
        var tile = new Tile(1, 1, 2, 3);
        Assert.False(board.TryAddTileAtIndex(tile, -1));
        Assert.False(board.TryAddTileAtIndex(tile, 19));
        board.TryAddTileAtIndex(tile, 0);
        Assert.False(board.TryAddTileAtIndex(tile, 0)); // already placed
        Assert.False(board.TryAddTileAtIndex(tile, 1)); // duplicate tile
    }
}
