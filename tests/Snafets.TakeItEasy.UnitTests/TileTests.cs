using Snafets.TakeItEasy.Domain;

namespace Snafets.TakeItEasy.UnitTests;

public class TileTests
{
    [Theory]
    [InlineData(1, 1, 2, 3, true)]
    [InlineData(2, 5, 6, 4, true)]
    [InlineData(3, 9, 7, 8, true)]
    [InlineData(4, 0, 2, 3, false)] // invalid vertical
    [InlineData(5, 1, 0, 3, false)] // invalid left diagonal
    [InlineData(6, 1, 2, 0, false)] // invalid right diagonal
    [InlineData(7, 2, 2, 3, false)] // invalid vertical
    [InlineData(8, 1, 8, 3, false)] // invalid left diagonal
    [InlineData(9, 1, 2, 9, false)] // invalid right diagonal
    public void IsValid_ReturnsExpected(int id, int vertical, int leftDiagonal, int rightDiagonal, bool expected)
    {
        if (expected)
        {
            var tile = new Tile(id, vertical, leftDiagonal, rightDiagonal);
            Assert.True(tile.IsValid());
        }
        else
        {
            Assert.Throws<ArgumentException>(() => new Tile(id, vertical, leftDiagonal, rightDiagonal));
        }
    }

    [Fact]
    public void Equals_ReturnsTrueForSameValues()
    {
        var tile1 = new Tile(1, 1, 2, 3);
        var tile2 = new Tile(1, 1, 2, 3);
        Assert.True(tile1.Equals(tile2));
    }

    [Fact]
    public void Equals_ReturnsFalseForDifferentValues()
    {
        var tile1 = new Tile(1, 1, 2, 3);
        var tile2 = new Tile(2, 5, 6, 4);
        Assert.False(tile1.Equals(tile2));
    }

    [Fact]
    public void GetHashCode_IsConsistentForSameValues()
    {
        var tile1 = new Tile(1, 1, 2, 3);
        var tile2 = new Tile(1, 1, 2, 3);
        Assert.Equal(tile1.GetHashCode(), tile2.GetHashCode());
    }
}
