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

        public Map Naturalize(Map map)
        {
            CalcuateExtremes(map);
            map = YAdjustMap(map);
            CalcuateExtremes(map); // Need to recalculate after moving

            if (LowestY != HighestY)
            {
                map = PaintAndFillWater(map);
                map = FillBottom(map);
            }            

            return map;
        }

        private Map YAdjustMap(Map map)
        {
            Console.WriteLine("Y-adjusting map...");

            Block[] blockData = map.BlockData;            
            short averageY = (short)(blockData.Average(b => b.Y));
            short adjustY = (short)(averageY * -1.0d);
            int tries = 10;

            while (tries > 0 &&
                (averageY + adjustY < MinWorldY ||
                averageY + adjustY > MaxWorldY ||
                LowestY + adjustY < MinWorldY ||
                HighestY + adjustY > MaxWorldY))
            {
                adjustY = (short)(adjustY / 2.0d);
                tries--;
            }
            
            // Move all blocks lower/higher more towards Y=0
            for (int i = 0; i < blockData.Length; i++)
            {
                blockData[i].Y += adjustY;

                // If still below bottom, set at bottom
                if (blockData[i].Y < MinWorldY) blockData[i].Y = (short)MinWorldY;
            }
                

            map.BlockData = blockData;
            return map;
        }

        private Map PaintAndFillWater(Map map)
        {
            Console.WriteLine("Painting map and filling in water...");

            Block[] blockData = map.BlockData;
            
            // Calculate variables
            short lowestVisibleY = (short)MinWorldY;
            short highestVisibleY = (short)MaxWorldY;
            short lowest = blockData.Min(m => m.Y);
            short highest = blockData.Max(m => m.Y);
            if (lowest >= MinWorldY)
                lowestVisibleY = lowest;
            if (highest <= MaxWorldY)
                highestVisibleY = highest;

            List<Block> fillBlocks = new List<Block>();
            Block.STYLE[] styleRange = BlockStyleRatios(highestVisibleY - lowestVisibleY);
            int maxWaterIndex = Array.FindIndex(styleRange, x => x == styleRange.First(s => s != Block.STYLE.Water)) - 1;
            int temp = 0;

            // Paint blocks
            for (int i = 0; i < blockData.Length; i++)
            {
                if (blockData[i].Y < MinWorldY || blockData[i].Y > MaxWorldY) continue;

                blockData[i].Style = styleRange[blockData[i].Y - lowestVisibleY];

                // Fill water
                if (blockData[i].Style == Block.STYLE.Water)
                {
                    temp = maxWaterIndex + lowestVisibleY - blockData[i].Y;
                    for (int j = blockData[i].Y + 1; j <= blockData[i].Y + temp; j++)
                    {
                        fillBlocks.Add(new Block
                        {
                            X = blockData[i].X,
                            Y = (short)j,
                            Z = blockData[i].Z,
                            Shape = blockData[i].Shape,
                            Direction = blockData[i].Direction,
                            Style = blockData[i].Style
                        });
                    }
                }
            }

            int originalLength = blockData.Length;
            Array.Resize(ref blockData, originalLength + fillBlocks.Count);
            fillBlocks.ToArray().CopyTo(blockData, originalLength);
            
            var group = blockData
                .GroupBy(b => b.Style)
                .Select(g => new { Style = g.Key, Sum = g.Count() });

            map.BlockData = blockData;
            return map;
        }

        private Block.STYLE[] BlockStyleRatios(int yRange)
        {
            Block.STYLE[] styleRange = new Block.STYLE[yRange + 1];

            int dataKey = 0;            
            List<(double, Block.STYLE)> data = new List<(double, Block.STYLE)>            
            {
                ( 0.1d, Block.STYLE.Water ), // 10%
                ( 0.06d, Block.STYLE.Sand ), // 6%
                ( 0.55d, Block.STYLE.Grass ), // 55%
                ( 0.08d, Block.STYLE.Dirt ), // 8%
                ( 0.14d, Block.STYLE.GrayCraters ), // 14%
                ( 0.07d, Block.STYLE.Snow ) // 7%
            };
            double runningSum = data[0].Item1;

            for (int i = 0; i < styleRange.Length; i++)
            {                
                while (((double)i / styleRange.Length) > runningSum && 
                    dataKey + 1 < data.Count)
                {
                    dataKey++;
                    runningSum = 0;
                    for (int j = 0; j <= dataKey; j++)
                        runningSum += data[j].Item1;
                }

                styleRange[i] = data[dataKey].Item2;
            }

            return styleRange;
        }

        private void CalcuateExtremes(Map map)
        {
            Console.WriteLine("Calculating map extremes...");

            Block[] blockData = map.BlockData;
            int lowest = 999;
            int highest = -999;

            for (int i = 0; i < blockData.Length; i++)
            {
                if (blockData[i].Y < lowest)
                    lowest = blockData[i].Y;
                if (blockData[i].Y > highest)
                    highest = blockData[i].Y;
            }

            LowestY = lowest;
            HighestY = highest;
        }

        private Map FillBottom(Map map)
        {
            Console.WriteLine("Filling in bottom of map...");

            // Fill
            Block[] blockData = map.BlockData;
            List<Block> fillBlocks = new List<Block>();
            for (int i = 0; i < blockData.Length; i++)
                for (int j = blockData[i].Y - 1; j >= LowestY; j--)
                {
                    fillBlocks.Add(new Block
                    {
                        X = blockData[i].X,
                        Y = (short)j,
                        Z = blockData[i].Z,
                        Shape = blockData[i].Shape,
                        Direction = blockData[i].Direction,
                        Style = blockData[i].Style
                    });
                }

            int originalLength = blockData.Length;
            Array.Resize(ref blockData, originalLength + fillBlocks.Count);
            fillBlocks.ToArray().CopyTo(blockData, originalLength);

            map.BlockData = blockData;
            return map;
        }
    }
}
