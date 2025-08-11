namespace Snafets.TakeItEasy.Domain
{
    public class DrawBag
    {
        public Queue<Tile> Tiles { get; set; }

        public static Random Rng { get; set; }

        public DrawBag()
        {
            var tileList = new List<Tile>();
            int id = 1;
            foreach (var v in Tile.ValidVerticals)
            {
                foreach (var l in Tile.ValidLeftDiagonals)
                {
                    foreach (var r in Tile.ValidRightDiagonals)
                    {
                        tileList.Add(new Tile(id++, v, l, r));
                    }
                }
            }

            // Shuffle the tiles
            int n = tileList.Count;
            while (n > 1)
            {
                n--;
                int k = Rng.Next(n + 1);
                var value = tileList[k];
                tileList[k] = tileList[n];
                tileList[n] = value;
            }

            Tiles = new Queue<Tile>(tileList);
        }
        /// <summary>
        /// Returns the tile at the front of the draw bag without removing it.
        /// </summary>
        public Tile? PeekTopTile()
        {
            if (Tiles.Count > 0)
                return Tiles.Peek();
            return null;
        }
    }
}
