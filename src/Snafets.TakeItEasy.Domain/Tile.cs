namespace Snafets.TakeItEasy.Domain
{
    public class Tile
    {
        public int Id { get; set; }
        public int Vertical { get; set; }
        public int LeftDiagonal { get; set; }
        public int RightDiagonal { get; set; }
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
}
