using Algorithms.Abstractions;
using GBWorldGen.Core.Models;
using System;
using System.Collections.Generic;

namespace Algorithms.Naturalize
{
    public class DefaultNaturalizer : INaturalizeWorld
    {
        private int LowestY { get; set; }
        private int HighestY { get; set; }

        public Block[] Naturalize(Block[] map)
        {
            CalcuateExtremes(map);
            map = FillBottom(map);

            for (int i = 0; i < map.Length; i++)
            {
                //if (map[i].Y <= LowestY + 2)
                //{
                //    map[i].Style = Block.STYLE.Water;
                //}

                //if (map[i].Y > HighestY - 3)
                //{
                //    map[i].Style = Block.STYLE.Snow;
                //}
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
