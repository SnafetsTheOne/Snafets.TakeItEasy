using System;
using System.Collections.Generic;

namespace TakeItEasy.Domain
{
    public class DrawBag
    {
        public Queue<Tile> Tiles { get; set; } = new Queue<Tile>();

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
            var rng = new Random();
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
}
