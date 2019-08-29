using GBWorldGen.Core.Algorithms.Abstractions;
using GBWorldGen.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GBWorldGen.Core.Algorithms.Naturalize
{
    public class DefaultNaturalizer : WorldData, INaturalizeWorld
    {
        private int LowestY { get; set; }
        private int LowestYVisible { get; set; }
        private int HighestY { get; set; }

        public Block[] Naturalize(Block[] map)
        {
            CalcuateExtremes(map);
            map = Paint(map);
            map = FillBottom(map);

            return map;
        }

        private Block[] Paint(Block[] map)
        {
            // Get blocks that are visible
            Dictionary<Tuple<short, short>, bool> heightDict = new Dictionary<Tuple<short, short>, bool>();
            List<Block> visibleBlocks = new List<Block>();
            short lowestVisibleY = 999;

            for (int i = 0; i < map.Length; i++)
            {
                if (!heightDict.ContainsKey(Tuple.Create(map[i].X, map[i].Z)))
                {
                    // Ugly, but it works
                    Block lowest = map
                        .Where(m => m.X == map[i].X && m.Z == map[i].Z)
                        .OrderBy(m => -1 * m.Y)
                        .First();
                    visibleBlocks.Add(lowest);

                    heightDict.Add(Tuple.Create(map[i].X, map[i].Z), true);
                    if (lowest.Y >= base.MinWorldY && lowest.Y < lowestVisibleY)
                        lowestVisibleY = lowest.Y;
                }
            }

            // Paint blocks
            for (int i = 0; i < map.Length; i++)
            {
                if (visibleBlocks.IndexOf(map[i]) >= 0)
                {
                    if (map[i].Y - 6 <= lowestVisibleY) map[i].Style = Block.STYLE.Water;
                    else if (map[i].Y - 10 <= lowestVisibleY) map[i].Style = Block.STYLE.Sand;
                    else if (map[i].Y - 24 <= lowestVisibleY) map[i].Style = Block.STYLE.Grass;
                    else if (map[i].Y - 28 <= lowestVisibleY) map[i].Style = Block.STYLE.Dirt;
                    else if (map[i].Y - 30 <= lowestVisibleY) map[i].Style = Block.STYLE.GrayCraters;
                    else map[i].Style = Block.STYLE.Snow;
                }
            }

            return map;
        }

        private void CalcuateExtremes(Block[] map)
        {
            int lowest = 999;
            int highest = -999;

            for (int i = 0; i < map.Length; i++)
            {
                if (map[i].Y < lowest)
                    lowest = map[i].Y;
                if (map[i].Y > highest)
                    highest = map[i].Y;
            }

            LowestY = lowest;
            HighestY = highest;
        }

        private Block[] FillBottom(Block[] map)
        {
            // Fill
            List<Block> fillBlocks = new List<Block>();
            for (int i = 0; i < map.Length; i++)
                for (int j = map[i].Y - 1; j >= LowestY; j--)
                {
                    fillBlocks.Add(new Block
                    {
                        X = map[i].X,
                        Y = (short)j,
                        Z = map[i].Z,
                        Shape = map[i].Shape,
                        Direction = map[i].Direction,
                        Style = map[i].Style
                    });
                }

            int originalLength = map.Length;
            Array.Resize(ref map, originalLength + fillBlocks.Count);
            fillBlocks.ToArray().CopyTo(map, originalLength);

            return map;
        }
    }
}
