using System.Collections.Generic;

namespace TakeItEasy.Domain
{
    /// <summary>
    /// Represents a player in the Take It Easy game.
    /// </summary>
    public class Player
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a player's board with 19 spaces arranged in a honeycomb pattern.
    /// </summary>
    public class PlayerBoard
    {
        /// <summary>
        /// Calculates the score for the player board.
        /// For each line type (Vertical, LeftDiagonal, RightDiagonal),
        /// if all tiles are placed and all values are the same, sum and add to total score.
        /// </summary>
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
                    if (tile == null)
                    {
                        valid = false;
                        break;
                    }
                    int v = selector(tile);
                    if (value == null) value = v;
                    else if (value != v) { valid = false; break; }
                    sum += v;
                }
                if (valid && value != null) total += sum;
            }
            return total;
        }
    
        public List<BoardSpace> Spaces { get; set; }
        public Player Player { get; set; }

        public PlayerBoard(Player player)
        {
            Player = player;
            Spaces = new List<BoardSpace>(19);
        }

        // Indices laid out row-major:
        //
        //    0  1  2
        //   3  4  5  6
        //  7  8  9 10 11
        //   12 13 14 15
        //    16 17 18
        public static class TakeItEasyLines
        {
            // 5 lines each direction (lengths 3,4,5,4,3)
            // Ordered left→right / top→bottom where it makes sense.

            // Vertical (columns)
            public static readonly int[][] Vertical = new[]
            {
                new[] {  7, 12, 16 },          // left
                new[] {  3,  8, 13, 17 },
                new[] {  0,  4,  9, 14, 18 },  // center
                new[] {  1,  5, 10, 15 },
                new[] {  2,  6, 11 }           // right
            };

            // Left Diagonal (upper-left → lower-right)
            public static readonly int[][] LeftDiagonal = new[]
            {
                new[] {  0,  1,  2 },
                new[] {  3,  4,  5,  6 },
                new[] {  7,  8,  9, 10, 11 },
                new[] { 12, 13, 14, 15 },
                new[] { 16, 17, 18 }
            };

            // Right Diagonal (upper-right → lower-left)
            public static readonly int[][] RightDiagonal = new[]
            {
                new[] {  0,  3,  7 },
                new[] {  1,  4,  8, 12 },
                new[] {  2,  5,  9, 13, 16 },
                new[] {  6, 10, 14, 17 },
                new[] { 11, 15, 18 }
            };
        }
    }

    /// <summary>
    /// Represents a space on the player board.
    /// </summary>
    public class BoardSpace
    {       
        public int Index { get; set; }
        public Tile? PlacedTile { get; set; }
    }

    /// <summary>
    /// Represents a hexagonal tile with three distinct numbers:
    /// Vertical, LeftDiagonal, and RightDiagonal.
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// Unique identifier for the tile.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Number for the vertical line.
        /// </summary>
        public int Vertical { get; set; }

        /// <summary>
        /// Number for the left diagonal line.
        /// </summary>
        public int LeftDiagonal { get; set; }

        /// <summary>
        /// Number for the right diagonal line.
        /// </summary>
        public int RightDiagonal { get; set; }
    }

    /// <summary>
    /// Represents the draw bag used to randomly draw and announce tiles.
    /// </summary>
    public class DrawBag
    {
        /// <summary>
        /// Queue of tiles available for drawing.
        /// </summary>
        public Queue<Tile> Tiles { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawBag"/> class,
        /// generating all possible tiles based on the allowed line numbers.
        /// </summary>
        public DrawBag()
        {
            var tileList = new List<Tile>();
            int id = 1;
            int[] verticals = { 1, 5, 9 };
            int[] leftDiagonals = { 2, 6, 7 };
            int[] rightDiagonals = { 3, 4, 8 };

            foreach (var v in verticals)
            {
                foreach (var l in leftDiagonals)
                {
                    foreach (var r in rightDiagonals)
                    {
                        tileList.Add(new Tile
                        {
                            Id = id++,
                            Vertical = v,
                            LeftDiagonal = l,
                            RightDiagonal = r
                        });
                    }
                }
            }

            // Shuffle the tiles
            var rng = new System.Random();
            int n = tileList.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = tileList[k];
                tileList[k] = tileList[n];
                tileList[n] = value;
            }

            Tiles = new Queue<Tile>(tileList);
        }
    }

    /// <summary>
    /// Represents the Take It Easy game domain model.
    /// </summary>
    public class TakeItEasyGame
    {
        public Guid Id { get; set; }
        public List<PlayerBoard> PlayerBoards { get; set; }
        public DrawBag CallerBag { get; set; }
        public List<Tile> TileSet { get; set; }

        /// <summary>
        /// Creates a new TakeItEasyGame for the given players.
        /// </summary>
        public TakeItEasyGame(List<Player> players)
        {
            Id = Guid.NewGuid();
            PlayerBoards = new List<PlayerBoard>();
            foreach (var player in players)
            {
                PlayerBoards.Add(new PlayerBoard(player));
            }
            CallerBag = new DrawBag();
            TileSet = new List<Tile>();
        }
    }
}