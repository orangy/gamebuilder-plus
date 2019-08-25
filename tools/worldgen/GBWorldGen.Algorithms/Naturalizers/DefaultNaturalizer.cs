using Algorithms.Abstractions;
using GBWorldGen.Core.Models;

namespace Algorithms.Naturalize
{
    public class DefaultNaturalizer : INaturalizeWorld
    {
        private int LowestY { get; set; }
        private int HighestY { get; set; }

        public Block[] Naturalize(Block[] map)
        {
            CalcuateExtremes(map);

            for (int i = 0; i < map.Length; i++)
            {
                if (map[i].Y <= LowestY + 2)
                {
                    map[i].Style = Block.STYLE.Water;
                }

                if (map[i].Y > HighestY - 3)
                {
                    map[i].Style = Block.STYLE.Snow;
                }
            }

            return map;
        }
        
        private void CalcuateExtremes(Block[] map)
        {
            int lowest = 0;
            int highest = 0;

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
    }
}
