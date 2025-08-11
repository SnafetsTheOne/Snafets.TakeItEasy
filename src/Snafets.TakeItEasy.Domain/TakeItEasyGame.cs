using System.Collections.Generic;

namespace TakeItEasy.Domain
{
    /// <summary>
    /// Represents a player's board with 19 spaces arranged in a honeycomb pattern.
    /// </summary>
    public class PlayerBoard
    {
        public List<BoardSpace> Spaces { get; set; } = new List<BoardSpace>(19);
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
        public Queue<Tile> Tiles { get; set; } = new Queue<Tile>();
    }

    /// <summary>
    /// Represents the Take It Easy game domain model.
    /// </summary>
    public class TakeItEasyGame
    {
        public List<PlayerBoard> PlayerBoards { get; set; } = new List<PlayerBoard>();
        public DrawBag CallerBag { get; set; } = new DrawBag();
        public List<Tile> TileSet { get; set; } = new List<Tile>();
    }
}