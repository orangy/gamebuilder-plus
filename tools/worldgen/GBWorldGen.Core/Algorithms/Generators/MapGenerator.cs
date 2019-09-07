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
        private MapGeneratorOptions Options { get; set; }

        public MapGenerator(short width, short length, short height, MapGeneratorOptions options = null)
            : base(width, length, height)
        {
            GeneratedMap = new Map(width, length, height,
                (short)(width * -0.5d), (short)(length * -0.5d));
            Options = options == null ? new MapGeneratorOptions() : options;

            Random rand = new Random();
            FastNoise = new FastNoise(rand.Next(int.MinValue, int.MaxValue));
        }

        public override BaseMap<short> GenerateMap()
        {
            short blockY;
            float noise;

            // Generate lakes
            List<Block> lakesToUse = new List<Block>();
            if (Options.Lakes)
            {
                Console.WriteLine("Generating lakes");
                List<Block> lakeBlocks = new List<Block>();
                short minLakeY = Map.MAXHEIGHT;
                short maxLakeY = Map.MINHEIGHT;
                FastNoise.SetInterp(FastNoise.Interp.Quintic);
                FastNoise.SetFrequency(Options.LakeFrequency);
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
                                Z = (short)(z + GeneratedMap.OriginLength)
                            });

                            if (blockY > maxLakeY) maxLakeY = blockY;
                            else if (blockY < minLakeY) minLakeY = blockY;
                        }
                    }
                }

                // "Cut" lakes out we will use
                short height = (short)Options.LakeSize;                
                for (int i = 0; i < lakeBlocks.Count; i++)
                {
                    if (lakeBlocks[i].Y <= minLakeY + height) lakesToUse.Add(lakeBlocks[i]);
                }

                // Fill lakes
                List<Block> fillLakeBlocks = new List<Block>();
                for (int i = 0; i < lakesToUse.Count; i++)
                {
                    for (short j = minLakeY; j <= minLakeY + height; j++)
                    {
                        fillLakeBlocks.Add(new Block
                        {
                            X = lakesToUse[i].X,
                            Y = j,
                            Z = lakesToUse[i].Z
                        });
                    }
                }
                lakesToUse.AddRange(fillLakeBlocks);

                //// Add sand around lakes
                //int radius = 3;
                //List<Block> lakeSandBlocks = new List<Block>();
                //for (int i = 0; i < lakesToUse.Count; i++)
                //{
                //    for (short j = 0; j < radius; j++)
                //    {
                //        // N
                //        lakeSandBlocks.Add(new Block
                //        {
                //            X = lakesToUse[i].X,
                //            Y = lakesToUse[i].Y,
                //            Z = (short)(lakesToUse[i].Z + j),
                //            Style = Block.STYLE.Sand
                //        });
                //    }
                //}

                // Place lake blocks                
                for (int i = 0; i < lakesToUse.Count; i++)
                {
                    GeneratedMap.Add(new Block
                    {
                        X = lakesToUse[i].X,
                        Y = (short)(0 - (minLakeY + height - lakesToUse[i].Y)),
                        Z = lakesToUse[i].Z,
                        Style = LakesBlockStyle()
                    });
                }
            }            

            // Create flat plains
            Console.WriteLine("Generating plains");
            List<Block> plainsBlocks = new List<Block>();
            short minPlainY = Map.MAXHEIGHT;
            short maxPlainY = Map.MINHEIGHT;
            short avgPlainY;
            short plainsBlockX;
            short plainsBlockZ;

            FastNoise.SetFrequency(Options.PlainFrequency);
            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Length; z++)
                {
                    blockY = (short)(Clamp2DNoise(x, z, 8) + GeneratedMap.OriginHeight);

                    plainsBlockX = (short)(x + GeneratedMap.OriginWidth);
                    plainsBlockZ = (short)(z + GeneratedMap.OriginLength);

                    // Don't add plains over lakes
                    if (!GeneratedMap.MapData.Any(m => m.X == plainsBlockX && m.Z == plainsBlockZ))
                    {
                        plainsBlocks.Add(new Block
                        {
                            X = (short)(x + GeneratedMap.OriginWidth),
                            Y = blockY,
                            Z = (short)(z + GeneratedMap.OriginLength),
                            Style = PlainsBlockStyle()
                        });

                        if (blockY < minPlainY) minPlainY = blockY;
                        else if (blockY > maxPlainY) maxPlainY = blockY;
                    }
                }
            }
            avgPlainY = (short)((maxPlainY + minPlainY) / 2);

            // Fill bottom of plains
            List<Block> bottomPlainsBlocks = new List<Block>();
            for (int i = 0; i < plainsBlocks.Count; i++)
            {
                for (short y = plainsBlocks[i].Y; y >= GeneratedMap.OriginHeight; y--)
                {
                    bottomPlainsBlocks.Add(new Block
                    {
                        X = plainsBlocks[i].X,
                        Y = y,
                        Z = plainsBlocks[i].Z,
                        Style = PlainsBlockStyle()
                    });
                }
            }
            plainsBlocks.AddRange(bottomPlainsBlocks);
            GeneratedMap.MapData.AddRange(plainsBlocks);

            // Create hills
            if (Options.Hills)
            {
                Console.WriteLine("Generating hills");
                List<Block> hillBlocks = new List<Block>();
                FastNoise.SetFrequency(Options.HillFrequency);
                short minHillY = Map.MAXHEIGHT;
                short hillsBlockX;
                short hillsBlockZ;
                for (int x = 0; x < Width; x++)
                {
                    for (int z = 0; z < Length; z++)
                    {
                        hillsBlockX = (short)(x + GeneratedMap.OriginWidth);
                        hillsBlockZ = (short)(z + GeneratedMap.OriginLength);

                        // Don't add hills over lakes
                        if (!lakesToUse.Any(b => b.X == hillsBlockX && b.Z == hillsBlockZ))
                        {
                            noise = FastNoise.GetPerlin(x, z);
                            if (noise > 0)
                            {
                                blockY = (short)(ClampNoise(noise, Options.HillClamp) + GeneratedMap.OriginHeight);
                                hillBlocks.Add(new Block
                                {
                                    X = (short)(x + GeneratedMap.OriginWidth),
                                    Y = blockY,
                                    Z = (short)(z + GeneratedMap.OriginLength),
                                    Style = HillsBlockStyle()
                                });

                                if (blockY < minHillY) minHillY = blockY;
                            }
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
                            Style = HillsBlockStyle()
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
            }            

            // Create mountains
            if (Options.Mountains)
            {
                Console.WriteLine("Generating mountains");
                List<Block> mountainBlocks = new List<Block>();
                FastNoise.SetFrequency(Options.MountainFrequency);
                FastNoise.SetFractalGain(0.01F);
                FastNoise.SetFractalOctaves(4);
                FastNoise.SetFractalLacunarity(3.0F);
                short minMountainY = Map.MAXHEIGHT;
                short maxMountainY = Map.MINHEIGHT;
                short mountainBlockX;
                short mountainBlockZ;
                for (int x = 0; x < Width; x++)
                {
                    for (int z = 0; z < Length; z++)
                    {
                        mountainBlockX = (short)(x + GeneratedMap.OriginWidth);
                        mountainBlockZ = (short)(z + GeneratedMap.OriginLength);

                        // Don't add mountains over lakes
                        if (!lakesToUse.Any(b => b.X == mountainBlockX && b.Z == mountainBlockZ))
                        {
                            noise = FastNoise.GetCubicFractal(x, z);
                            if (noise > 0)
                            {
                                noise = (float)Math.Pow((double)noise + Options.AdditionalMountainSize, 1.9d);

                                blockY = (short)(ClampToWorld(noise) + GeneratedMap.OriginHeight);
                                mountainBlocks.Add(new Block
                                {
                                    X = (short)(x + GeneratedMap.OriginWidth),
                                    Y = blockY,
                                    Z = (short)(z + GeneratedMap.OriginLength),
                                    Style = MountainsBlockStyle()
                                });

                                if (blockY < minMountainY) minMountainY = blockY;
                                else if (blockY > maxMountainY) maxMountainY = blockY;
                            }
                        }
                    }
                }

                // Paint mountains
                Block.STYLE[] mountainColors = MountainBlockStyleRange(minMountainY, maxMountainY);
                for (int i = 0; i < mountainBlocks.Count; i++)
                {
                    mountainBlocks[i].Style = mountainColors[mountainBlocks[i].Y - minMountainY];
                }

                // Fill mountains
                List<Block> fillMountainBlocks = new List<Block>();
                Block.STYLE mountainBlockStyle = Block.STYLE.Blue;
                for (int i = 0; i < mountainBlocks.Count; i++)
                {
                    for (int y = mountainBlocks[i].Y; y >= minMountainY; y--)
                    {
                        // Snow should fall on top of mountain,
                        // rest of blocks should be even throughout
                        mountainBlockStyle = mountainColors[mountainBlocks[i].Y - minMountainY];
                        switch (Options.Biome)
                        {
                            case MapGeneratorOptions.MapBiome.Grassland:
                                if (mountainBlockStyle == Block.STYLE.Snow && y < mountainBlocks[i].Y) mountainBlockStyle = Block.STYLE.GrayCraters;
                                break;
                            default:
                                break;
                        }

                        fillMountainBlocks.Add(new Block
                        {
                            X = mountainBlocks[i].X,
                            Y = (short)y,
                            Z = mountainBlocks[i].Z,
                            Style = mountainBlockStyle
                        });
                    }
                }
                mountainBlocks.AddRange(fillMountainBlocks);

                // Adjust mountains down
                short adjustMountainY = (short)(minMountainY - minPlainY);
                for (int i = 0; i < mountainBlocks.Count; i++)
                {
                    mountainBlocks[i].Y -= adjustMountainY;
                    GeneratedMap.Add(mountainBlocks[i]);
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
            int diff = (int)(-0.5 * levels + 3);

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
            float max = 1.0F;
            float totalRange = max - min;
            float step = totalRange / ((float)levels);
            float i = min;
            int diff = 0;

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

        private Block.STYLE LakesBlockStyle()
        {
            return Options.Biome == MapGeneratorOptions.MapBiome.Tundra ? Block.STYLE.Ice : Block.STYLE.Water;
        }

        private Block.STYLE PlainsBlockStyle()
        {
            return Options.Biome == MapGeneratorOptions.MapBiome.Tundra ? Block.STYLE.Snow : Options.Biome == MapGeneratorOptions.MapBiome.Desert ? Block.STYLE.Sand : Block.STYLE.Grass;
        }

        private Block.STYLE HillsBlockStyle()
        {
            return Options.Biome == MapGeneratorOptions.MapBiome.Tundra ? Block.STYLE.Snow : Options.Biome == MapGeneratorOptions.MapBiome.Desert ? Block.STYLE.Sand : Block.STYLE.GrassStone;
        }

        private Block.STYLE MountainsBlockStyle()
        {
            return Options.Biome == MapGeneratorOptions.MapBiome.Tundra ? Block.STYLE.Ice : Options.Biome == MapGeneratorOptions.MapBiome.Desert ? Block.STYLE.Sand : Block.STYLE.GrayCraters;
        }

        private Block.STYLE[] MountainBlockStyleRange(short min, short max)
        {
            Block.STYLE[] styleRange = new Block.STYLE[max - min + 1];

            int dataKey = 0;
            List<(double, Block.STYLE)> data = null;

            switch (Options.Biome)
            {
                case MapGeneratorOptions.MapBiome.Grassland:
                    data = new List<(double, Block.STYLE)>
                    {
                        ( 0.1d, Block.STYLE.Dirt ), // 10%
                        ( 0.7d, Block.STYLE.GrayCraters ), // 70%
                        ( 0.2d, Block.STYLE.Snow ) // 20%                
                    };
                    break;
                case MapGeneratorOptions.MapBiome.Desert:
                    data = new List<(double, Block.STYLE)>
                    {
                        ( 1.0d, Block.STYLE.Sand ) // 100%
                    };
                    break;
                case MapGeneratorOptions.MapBiome.Tundra:
                    data = new List<(double, Block.STYLE)>
                    {
                        ( 1.0d, Block.STYLE.Snow ) // 100%              
                    };
                    break;
                default:
                    break;
            }
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
    }
}
