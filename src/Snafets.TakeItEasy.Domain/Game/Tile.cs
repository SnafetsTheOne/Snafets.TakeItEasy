namespace Snafets.TakeItEasy.Domain.Game;

public class Tile
{
    public static readonly int[] ValidVerticals = { 1, 5, 9 };
    public static readonly int[] ValidLeftDiagonals = { 2, 6, 7 };
    public static readonly int[] ValidRightDiagonals = { 3, 4, 8 };

    public int Id { get; }
    public int Vertical { get; }
    public int LeftDiagonal { get; }
    public int RightDiagonal { get; }

    public Tile(int id, int vertical, int leftDiagonal, int rightDiagonal)
    {
        Id = id;
        Vertical = vertical;
        LeftDiagonal = leftDiagonal;
        RightDiagonal = rightDiagonal;
        if (!IsValid())
        {
            throw new ArgumentException($"Invalid tile values: vertical={vertical}, leftDiagonal={leftDiagonal}, rightDiagonal={rightDiagonal}");
        }
    }

    public bool IsValid()
    {
        // Only allow values used in DrawBag
        return ValidVerticals.Contains(Vertical)
            && ValidLeftDiagonals.Contains(LeftDiagonal)
            && ValidRightDiagonals.Contains(RightDiagonal);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Tile other)
        {
            // Compare relevant properties for equality
            return this.Id == other.Id &&
                   this.Vertical == other.Vertical &&
                   this.LeftDiagonal == other.LeftDiagonal &&
                   this.RightDiagonal == other.RightDiagonal;
        }
        return false;
    }

    public override int GetHashCode()
    {
        // Combine hash codes of relevant properties
        return HashCode.Combine(Id, Vertical, LeftDiagonal, RightDiagonal);
    }
}
