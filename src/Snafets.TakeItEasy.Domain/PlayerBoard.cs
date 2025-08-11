namespace Snafets.TakeItEasy.Domain
{
    public class PlayerBoard
    {
        public List<BoardSpace> Spaces { get; set; }
        public Player Player { get; set; }

        public PlayerBoard(Player player)
        {
            Player = player;
            Spaces = new List<BoardSpace>(19);
        }

        /// <summary>
        /// Checks whether the specified tile is on the board.
        /// </summary>
        /// <param name="tile">The tile to check.</param>
        /// <returns>True if the tile is on the board; otherwise, false.</returns>
        public bool ContainsTile(Tile tile)
        {
            // Assuming there is a collection of tiles, e.g., Tiles or BoardSpaces
            // Replace 'Tiles' with the actual collection name if different
            return Spaces.Any(space => space.PlacedTile == tile);
        }

        public static class TakeItEasyLines
        {
            public static readonly int[][] Vertical = new[]
            {
                new[] {  7, 12, 16 },
                new[] {  3,  8, 13, 17 },
                new[] {  0,  4,  9, 14, 18 },
                new[] {  1,  5, 10, 15 },
                new[] {  2,  6, 11 }
            };
            public static readonly int[][] LeftDiagonal = new[]
            {
                new[] {  0,  1,  2 },
                new[] {  3,  4,  5,  6 },
                new[] {  7,  8,  9, 10, 11 },
                new[] { 12, 13, 14, 15 },
                new[] { 16, 17, 18 }
            };
            public static readonly int[][] RightDiagonal = new[]
            {
                new[] {  0,  3,  7 },
                new[] {  1,  4,  8, 12 },
                new[] {  2,  5,  9, 13, 16 },
                new[] {  6, 10, 14, 17 },
                new[] { 11, 15, 18 }
            };
        }

        public int CalculateScore()
        {
            int score = 0;
            score += CalculateLineTypeScore(TakeItEasyLines.Vertical, t => t.Vertical);
            score += CalculateLineTypeScore(TakeItEasyLines.LeftDiagonal, t => t.LeftDiagonal);
            score += CalculateLineTypeScore(TakeItEasyLines.RightDiagonal, t => t.RightDiagonal);
            return score;
        }

        private int CalculateLineTypeScore(int[][] lines, System.Func<Tile, int> selector)
        {
            int total = 0;
            foreach (var line in lines)
            {
                int? value = null;
                int sum = 0;
                bool valid = true;
                foreach (var idx in line)
                {
                    var tile = Spaces[idx].PlacedTile;
                    if (tile is null)
                    {
                        valid = false;
                        break;
                    }
                    int v = selector(tile);
                    if (value is null) value = v;
                    else if (value != v) { valid = false; break; }
                    sum += v;
                }
                if (valid && value is not null) total += sum;
            }
            return total;
        }
        
        /// <summary>
        /// Attempts to add a tile to the board at the specified index.
        /// Checks that the index is valid, the space is empty, and the tile is not already present on the board.
        /// </summary>
        /// <param name="tile">The tile to add.</param>
        /// <param name="index">The index to add the tile at.</param>
        /// <returns>True if the tile was added; otherwise, false.</returns>
        public bool TryAddTileAtIndex(Tile tile, int index)
        {
            if (index < 0 || index >= Spaces.Count)
                return false;
            var space = Spaces[index];
            if (space.PlacedTile is not null)
                return false;
            if (ContainsTile(tile))
                return false;
            space.PlacedTile = tile;
            return true;
        }
    }
}
