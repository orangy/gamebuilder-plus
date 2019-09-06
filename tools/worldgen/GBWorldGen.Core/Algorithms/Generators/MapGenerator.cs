using GBWorldGen.Core.Algorithms.Generators.Abstractions;
using GBWorldGen.Core.Algorithms.Methods;
using GBWorldGen.Core.Models;
using GBWorldGen.Core.Models.Abstractions;
using GBWorldGen.Misc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GBWorldGen.Core.Algorithms.Generators
{
    /// <summary>
    /// A map generator for the world of Game Builder.
    /// </summary>
    public class MapGenerator : BaseGenerator<short>
    {        
        private FastNoise FastNoise { get; set; }

        public MapGenerator(short width, short length, short height)
            : base(width, length, height)
        {
            GeneratedMap = new Map(width, length, height,
                (short)(width * -0.5d), (short)(length * -0.5d));

            Random rand = new Random();
            FastNoise = new FastNoise(rand.Next(int.MinValue, int.MaxValue));
        }

        public override BaseMap<short> GenerateMap()
        {
            if (Height == 1)
            {
                short blockY;
                float noise;

                // Create flat plains
                short minPlainY = Map.MAXHEIGHT;
                short maxPlainY = Map.MINHEIGHT;
                short avgPlainY;
                FastNoise.SetFrequency(0.02F);
                for (int x = 0; x < Width; x++)
                {
                    for (int z = 0; z < Length; z++)
                    {
                        blockY = (short)(Clamp2DNoise(x, z, 8) + GeneratedMap.OriginHeight);
                        GeneratedMap.Add(new Block
                        {
                            X = (short)(x + GeneratedMap.OriginWidth),
                            Y = blockY,
                            Z = (short)(z + GeneratedMap.OriginLength)
                        });

                        if (blockY < minPlainY) minPlainY = blockY;
                        else if (blockY > maxPlainY) maxPlainY = blockY;
                    }
                }
                avgPlainY = (short)((maxPlainY + minPlainY) / 2);

                // Create hills
                List<Block> hillBlocks = new List<Block>();
                FastNoise.SetFrequency(0.05F);                
                short minHillY = Map.MAXHEIGHT;
                for (int x = 0; x < Width; x++)
                {
                    for (int z = 0; z < Length; z++)
                    {
                        noise = FastNoise.GetCubicFractal(x, z);
                        if (noise > 0)
                        {
                            noise = (float)Math.Pow((double)noise, 1.6d);

                            blockY = (short)(ClampToWorld(noise) + GeneratedMap.OriginHeight);
                            hillBlocks.Add(new Block
                            {
                                X = (short)(x + GeneratedMap.OriginWidth),
                                Y = blockY,
                                Z = (short)(z + GeneratedMap.OriginLength),
                                Style = Block.STYLE.GrassStone
                            });

                            if (blockY < minHillY) minHillY = blockY;
                        }                        
                    }
                }

                // Fill hills
                List<Block> fillHillBlocks = new List<Block>();
                for (int i = 0; i < hillBlocks.Count; i++)
                {
                    for (int y = hillBlocks[i].Y; y >= minHillY; y--)
                    {
                        fillHillBlocks.Add(new Block
                        {
                            X = hillBlocks[i].X,
                            Y = (short)y,
                            Z = hillBlocks[i].Z,
                            Style = Block.STYLE.GrassStone
                        });
                    }
                }
                hillBlocks.AddRange(fillHillBlocks);

                // Adjust hills down
                short adjustHillY = (short)(minHillY - minPlainY);
                for (int i = 0; i < hillBlocks.Count; i++)
                {
                    hillBlocks[i].Y -= adjustHillY;
                    GeneratedMap.Add(hillBlocks[i]);
                }

                // Generate lakes   
                List<Block> lakeBlocks = new List<Block>();
                FastNoise.SetInterp(FastNoise.Interp.Quintic);
                FastNoise.SetFrequency(0.015F);
                short minLakeY = Map.MAXHEIGHT;
                short maxLakeY = Map.MINHEIGHT;
                for (int x = 0; x < Width; x++)
                {
                    for (int z = 0; z < Length; z++)
                    {
                        noise = FastNoise.GetPerlin(x, z);

                        if (noise < 0)
                        {
                            blockY = (short)(ClampToWorld(noise) + GeneratedMap.OriginHeight);
                            lakeBlocks.Add(new Block
                            {
                                X = (short)(x + GeneratedMap.OriginWidth),
                                Y = blockY,
                                Z = (short)(z + GeneratedMap.OriginLength),
                                Style = Block.STYLE.Water
                            });

                            if (blockY > maxLakeY) maxLakeY = blockY;
                            else if (blockY < minLakeY) minLakeY = blockY;
                        }
                    }
                }

                // "Cut" lakes out we will use
                short height = 7;
                List<Block> lakesToUse = new List<Block>();
                for (int i = 0; i < lakeBlocks.Count; i++)
                {
                    if (lakeBlocks[i].Y <= minLakeY + height) lakesToUse.Add(lakeBlocks[i]);
                }

                // Place lake blocks                
                for (int i = 0; i < lakesToUse.Count; i++)
                {
                    ((Block)GeneratedMap.MapData.First(b =>
                    b.X == lakesToUse[i].X &&
                    b.Z == lakesToUse[i].Z &&
                    b.Y <= avgPlainY)).Style = Block.STYLE.Water;
                }
            }
            else
            {
                float noise;

                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        for (int z = 0; z < Length; z++)
                        {
                            noise = Generate3DNoise(x, z, y);

                            if (noise > 0)
                            {
                                GeneratedMap.Add(new Block
                                {
                                    X = (short)(x + GeneratedMap.OriginWidth),
                                    Y = (short)(y + GeneratedMap.OriginHeight),
                                    Z = (short)(z + GeneratedMap.OriginLength)
                                });
                            }                                                       
                        }
                    }
                }
            }

            return GeneratedMap;
        }

        public int Clamp2DNoise(float x, float z, int levels)
        {
            float noise = FastNoise.GetPerlin(x, z);            
            float min = -1.0F;
            float max = 1.0F;
            float totalRange = max - min;
            float step = totalRange / ((float)levels);
            float i = min;
            int diff = -1 * levels + 1;

            while (i <= max)
            {
                if (noise <= i + step) return diff;

                i += step;
                diff++;
            }

            return diff;
        }

        public int ClampNoise(float noise, int levels)
        {
            float min = -1.0F;
            float max = 0F;
            float totalRange = max - min;
            float step = totalRange / ((float)levels);
            float i = min;
            int diff = -1 * levels + 1;

            while (i <= max)
            {
                if (noise <= i + step) return diff;

                i += step;
                diff++;
            }

            return diff;
        }

        public override float Generate2DNoise(float x, float z)
        {
            return ClampToWorld(FastNoise.GetPerlin(x, z));
        }

        public override float Generate3DNoise(float x, float z, float y)
        {
            return FastNoise.GetPerlin(x, z, y);
        }

        private float ClampToWorld(float input)
        {
            return AffineTransformation.MapToWorld(input, GeneratedMap.MinHeight, GeneratedMap.MaxHeight);
        }
    }
}
