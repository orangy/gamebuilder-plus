using GBWorldGen.Core.Algorithms.Generators.Abstractions;
using GBWorldGen.Core.Algorithms.Methods;
using GBWorldGen.Core.Models;
using GBWorldGen.Core.Models.Abstractions;
using GBWorldGen.Misc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private Stopwatch Stopwatch { get; set; }
        private string StopwatchFormatString { get; set; }

        public MapGenerator(short width, short length, short height, MapGeneratorOptions options = null)
            : base(width, length, height)
        {
            GeneratedMap = new Map(width, length, height,
                (short)(width * -0.5d), (short)(length * -0.5d), -20);
            Options = options == null ? new MapGeneratorOptions() : options;

            Random rand = new Random();
            FastNoise = new FastNoise(rand.Next(int.MinValue, int.MaxValue));
            Stopwatch = new Stopwatch();
            StopwatchFormatString = "%m'm '%s's '%fff'ms'";
        }

        public override BaseMap<short> GenerateMap()
        {
            Map generatedMap = (Map)GeneratedMap;
            Random rand = new Random();
            short blockY;
            float noise;
            short additionalCaveHeight = (short)(Options.Caves ? Options.AdditionalCaveHeight : 0);

            // Generate lakes
            List<Block> lakesToUse = new List<Block>();
            if (Options.Lakes)
            {
                Console.Write("Generating lakes");
                Stopwatch.Start();
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
                            blockY = (short)(ClampToWorld(noise));
                            lakeBlocks.Add(new Block
                            {
                                X = (short)(x + generatedMap.OriginWidth),
                                Y = blockY,
                                Z = (short)(z + generatedMap.OriginLength)
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
                    // Fill top of lake
                    for (short j = minLakeY; j <= minLakeY + height + additionalCaveHeight; j++)
                    {
                        fillLakeBlocks.Add(new Block
                        {
                            X = lakesToUse[i].X,
                            Y = j,
                            Z = lakesToUse[i].Z
                        });
                    }

                    // Fill to bottom of map
                    for (short j = (short)(minLakeY - 1); j >= Map.MINHEIGHT; j--)
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
                    blockY = (short)(0 - (minLakeY + height - lakesToUse[i].Y));
                    generatedMap.Add(new Block
                    {
                        X = lakesToUse[i].X,
                        Y = blockY,
                        Z = lakesToUse[i].Z,
                        Style = LakesBlockStyle(blockY)
                    });
                }

                Stopwatch.Stop();
                Console.WriteLine($" (Elapsed time:'{Stopwatch.Elapsed.ToString(StopwatchFormatString)}')");
                Stopwatch.Reset();
            }

            // Create flat plains
            Console.Write("Generating plains");
            Stopwatch.Start();
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
                    blockY = (short)(Clamp2DNoise(x, z, 8) + additionalCaveHeight);

                    plainsBlockX = (short)(x + generatedMap.OriginWidth);
                    plainsBlockZ = (short)(z + generatedMap.OriginLength);

                    // Don't add plains over lakes
                    if (!GeneratedMap.MapData.Any(m => m.X == plainsBlockX && m.Z == plainsBlockZ))
                    {
                        plainsBlocks.Add(new Block
                        {
                            X = (short)(x + generatedMap.OriginWidth),
                            Y = blockY,
                            Z = (short)(z + generatedMap.OriginLength),
                            Style = PlainsBlockStyle(blockY)
                        });

                        if (blockY < minPlainY) minPlainY = blockY;
                        else if (blockY > maxPlainY) maxPlainY = blockY;

                        // actor test
                        if (rand.Next(0, 100) < 7)
                        {                            
                            generatedMap.Actors.Add(
                                new Actor((short)(x + generatedMap.OriginWidth), 
                                (short)(blockY + 1),
                                (short)(z + generatedMap.OriginLength),
                                "WG_Tree"));
                        }
                    }
                }
            }
            avgPlainY = (short)((maxPlainY + minPlainY) / 2);

            // Fill bottom of plains
            List<Block> bottomPlainsBlocks = new List<Block>();
            short minPlainFillY = (short)(additionalCaveHeight == 0 ? Map.MINHEIGHT : additionalCaveHeight - 7);
            for (int i = 0; i < plainsBlocks.Count; i++)
            {
                for (short y = (short)(plainsBlocks[i].Y - 1); y >= minPlainFillY; y--)
                {
                    bottomPlainsBlocks.Add(new Block
                    {
                        X = plainsBlocks[i].X,
                        Y = y,
                        Z = plainsBlocks[i].Z,
                        Style = UnderPlainsBlockStyle(y)
                    });
                }
            }
            plainsBlocks.AddRange(bottomPlainsBlocks);
            generatedMap.MapData.AddRange(plainsBlocks);
            Stopwatch.Stop();
            Console.WriteLine($" (Elapsed time:'{Stopwatch.Elapsed.ToString(StopwatchFormatString)}')");
            Stopwatch.Reset();

            // Create hills
            if (Options.Hills)
            {
                Console.Write("Generating hills");
                Stopwatch.Start();
                List<Block> hillBlocks = new List<Block>();
                FastNoise.SetFrequency(Options.HillFrequency);
                short minHillY = Map.MAXHEIGHT;
                short hillsBlockX;
                short hillsBlockZ;
                for (int x = 0; x < Width; x++)
                {
                    for (int z = 0; z < Length; z++)
                    {
                        hillsBlockX = (short)(x + generatedMap.OriginWidth);
                        hillsBlockZ = (short)(z + generatedMap.OriginLength);

                        // Don't add hills over lakes
                        if (!lakesToUse.Any(b => b.X == hillsBlockX && b.Z == hillsBlockZ))
                        {
                            noise = FastNoise.GetPerlin(x, z);
                            if (noise > 0)
                            {
                                blockY = (short)(ClampNoise(noise, Options.HillClamp) + additionalCaveHeight);
                                hillBlocks.Add(new Block
                                {
                                    X = (short)(x + generatedMap.OriginWidth),
                                    Y = blockY,
                                    Z = (short)(z + generatedMap.OriginLength),
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
                    for (int y = hillBlocks[i].Y - 1; y >= minHillY; y--)
                    {
                        fillHillBlocks.Add(new Block
                        {
                            X = hillBlocks[i].X,
                            Y = (short)y,
                            Z = hillBlocks[i].Z,
                            Style = UnderHillsBlockStyle()
                        });
                    }
                }
                hillBlocks.AddRange(fillHillBlocks);

                // Adjust hills down
                short adjustHillY = (short)(minHillY - minPlainY);
                for (int i = 0; i < hillBlocks.Count; i++)
                {
                    hillBlocks[i].Y -= adjustHillY;
                    generatedMap.Add(hillBlocks[i]);
                }

                Stopwatch.Stop();
                Console.WriteLine($" (Elapsed time:'{Stopwatch.Elapsed.ToString(StopwatchFormatString)}')");
                Stopwatch.Reset();
            }            

            // Create mountains
            if (Options.Mountains)
            {
                Console.Write("Generating mountains");
                Stopwatch.Start();
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
                        mountainBlockX = (short)(x + generatedMap.OriginWidth);
                        mountainBlockZ = (short)(z + generatedMap.OriginLength);

                        // Don't add mountains over lakes
                        if (!lakesToUse.Any(b => b.X == mountainBlockX && b.Z == mountainBlockZ))
                        {
                            noise = FastNoise.GetCubicFractal(x, z);
                            if (noise > 0)
                            {
                                noise = (float)Math.Pow((double)noise + Options.AdditionalMountainSize, 3.2d);

                                blockY = (short)(ClampToWorld(noise) + additionalCaveHeight);
                                mountainBlocks.Add(new Block
                                {
                                    X = (short)(x + generatedMap.OriginWidth),
                                    Y = blockY,
                                    Z = (short)(z + generatedMap.OriginLength),
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
                    generatedMap.Add(mountainBlocks[i]);
                }

                Stopwatch.Stop();
                Console.WriteLine($" (Elapsed time:'{Stopwatch.Elapsed.ToString(StopwatchFormatString)}')");
                Stopwatch.Reset();
            }

            // Create caves
            if (Options.Caves)
            {
                Console.Write("Generating caves");
                Stopwatch.Start();
                FastNoise.SetFrequency(0.07F);
                FastNoise.SetInterp(FastNoise.Interp.Hermite);
                for (int x = 0; x < Width; x++)
                {
                    for (int z = 0; z < Length; z++)
                    {
                        for (int y = generatedMap.MinHeight; y < additionalCaveHeight; y++)
                        {
                            if (y == generatedMap.MinHeight)
                            {
                                generatedMap.Add(new Block
                                {
                                    X = (short)(x + generatedMap.OriginWidth),
                                    Y = (short)y,
                                    Z = (short)(z + generatedMap.OriginLength),
                                    Style = CaveBlockStyle((short)y)
                                });
                            }
                            else if (lakesToUse.Any(l => l.X == (short)(x + generatedMap.OriginWidth) && l.Y == (short)y && l.Z == (short)(z + generatedMap.OriginLength)))
                            {
                                continue;
                            }
                            else
                            {
                                if ((x == 0 || x + 1 == Width) || (z == 0 || z + 1 == Length))
                                {
                                    generatedMap.Add(new Block
                                    {
                                        X = (short)(x + generatedMap.OriginWidth),
                                        Y = (short)y,
                                        Z = (short)(z + generatedMap.OriginLength),
                                        Style = CaveBlockStyle((short)y)
                                    });
                                }
                                else
                                {
                                    noise = FastNoise.GetCubicFractal(x, z, y);
                                    if (noise > 0)
                                    {
                                        generatedMap.Add(new Block
                                        {
                                            X = (short)(x + generatedMap.OriginWidth),
                                            Y = (short)y,
                                            Z = (short)(z + generatedMap.OriginLength),
                                            Style = CaveBlockStyle((short)y)
                                        });
                                    }
                                }
                            }                        
                        }
                    }
                }

                Stopwatch.Stop();
                Console.WriteLine($" (Elapsed time:'{Stopwatch.Elapsed.ToString(StopwatchFormatString)}')");
                Stopwatch.Reset();
            }

            // Create tunnels
            if (Options.Tunnels)
            {
                Console.Write("Generating tunnels");
                Stopwatch.Start();
                List<Block> toRemoveBlocksForTunnel = new List<Block>();
                List<(double, double)> wormSpawns = new List<(double, double)>();

                for (short x = generatedMap.OriginWidth; x < generatedMap.OriginWidth + Width; x++)
                {
                    for (short z = generatedMap.OriginLength; z < generatedMap.OriginLength + Length; z++)
                    {
                        noise = FastNoise.GetCellular(x, z);

                        if (noise > 0 &&
                            Options.TunnelWormsMax > 0 &&
                            wormSpawns.All((w) => Math.Abs(w.Item1 - x) > 15 && Math.Abs(w.Item2 - z) > 15) &&
                            (((x - 15 > generatedMap.OriginWidth) && (x + 15 < generatedMap.OriginWidth + Width)) && ((z - 15 > generatedMap.OriginLength) && (z + 15 < generatedMap.OriginLength + Length))) &&
                            lakesToUse.All(m => Math.Abs(m.X - x) > 4 && Math.Abs(m.Z - z) > 4))
                        {
                            if (Options.TunnelWormsMax > 0)
                            {
                                // Spawn worm
                                List<Block> bs = new List<Block>();
                                bs = generatedMap.MapData.Where(m => m.X == x && m.Z == z).Select(b => (Block)b).ToList();
                                bs.Sort((a, b) =>
                                {
                                    if (a.Y > b.Y) return 1;
                                    if (a.Y < b.Y) return -1;
                                    return 0;
                                });
                                short wormStartY = bs.Last().Y;
                                
                                short xAdjust = 0;
                                short zAdjust = 0;
                                short yAdjust = 0;

                                for (int i = 0; i < Options.TunnelLength + (Options.Caves ? 10 : 0); i++)
                                {
                                    for (short j = 1; j <= Options.TunnelRadius; j++)
                                    {
                                        // Prevent destroying block at world boundries
                                        toRemoveBlocksForTunnel.AddRange(generatedMap.MapData
                                        .Where(g => (Math.Abs(g.X - x + xAdjust) <= j) &&
                                                    (g.X - x + xAdjust > generatedMap.OriginWidth) &&
                                                    (g.X - x + xAdjust < generatedMap.OriginWidth + Width) &&
                                                    (Math.Abs(g.Z - z + zAdjust) <= j) &&
                                                    (g.Z - z + zAdjust > generatedMap.OriginLength) &&
                                                    (g.Z - z + zAdjust < generatedMap.OriginLength + Length) &&
                                                    (g.Y == (wormStartY + yAdjust)) &&
                                                    (g.Y > Map.MINHEIGHT) &&
                                                    DoesTunnelDestroyBlock(j))
                                        .Select(g => (Block)g).ToList());
                                    }

                                    // Adjust worm
                                    xAdjust += (short)(rand.NextDouble() < 0.5d ? -1 : (rand.NextDouble() < 0.5d ? 0 : 1));
                                    zAdjust += (short)(rand.NextDouble() < 0.5d ? -1 : (rand.NextDouble() < 0.5d ? 0 : 1));
                                    yAdjust--;
                                }

                                Options.TunnelWormsMax--;
                                wormSpawns.Add((x, z));
                            }
                        }
                    }
                }

                // https://stackoverflow.com/a/22667558/1837080
                generatedMap.MapData.RemoveAll(new HashSet<Block>(toRemoveBlocksForTunnel).Contains);

                Stopwatch.Stop();
                Console.WriteLine($" (Elapsed time:'{Stopwatch.Elapsed.ToString(StopwatchFormatString)}')");
                Stopwatch.Reset();
            }

            return generatedMap;
        }

        public BaseMap<short> Generate3DPerlin()
        {
            short blockY;
            float noise;

            FastNoise.SetFrequency(0.07F);
            FastNoise.SetInterp(FastNoise.Interp.Hermite);
            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Length; z++)
                {
                    for (int y = -20; y < Height; y++)
                    {
                        noise = FastNoise.GetCubicFractal(x, z, y);

                        if (noise > 0)
                        {
                            GeneratedMap.Add(new Block
                            {
                                X = (short)(x + GeneratedMap.OriginWidth),
                                Y = (short)y,
                                Z = (short)(z + GeneratedMap.OriginLength)
                            });
                        }
                    }
                }
            }

            return GeneratedMap;
        }

        public BaseMap<short> GenerateTutorialPerlin()
        {
            short blockY;
            float noise;

            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Length; z++)
                {
                    noise = FastNoise.GetPerlin(x, z);
                    blockY = (short)(ClampToWorld(noise) + GeneratedMap.OriginHeight);

                    GeneratedMap.Add(new Block
                    {
                        X = (short)(x + GeneratedMap.OriginWidth),
                        Y = blockY,
                        Z = (short)(z + GeneratedMap.OriginLength)
                    });
                }
            }

            return GeneratedMap;
        }

        public BaseMap<short> GenerateTutorialLakes()
        {
            short blockY;
            float noise;

            // Generate lakes
            List<Block> lakesToUse = new List<Block>();
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
                blockY = (short)(0 - (minLakeY + height - lakesToUse[i].Y));
                GeneratedMap.Add(new Block
                {
                    X = lakesToUse[i].X,
                    Y = blockY,
                    Z = lakesToUse[i].Z,
                    Style = LakesBlockStyle(blockY)
                });
            }

            return GeneratedMap;
        }

        public BaseMap<short> GenerateTutorialPlain()
        {
            short blockY;
            float noise;

            // Generate lakes
            List<Block> lakesToUse = new List<Block>();
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
                blockY = (short)(0 - (minLakeY + height - lakesToUse[i].Y));
                GeneratedMap.Add(new Block
                {
                    X = lakesToUse[i].X,
                    Y = blockY,
                    Z = lakesToUse[i].Z,
                    Style = LakesBlockStyle(blockY)
                });
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
                            Style = PlainsBlockStyle(blockY)
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
                        Style = PlainsBlockStyle(y)
                    });
                }
            }
            plainsBlocks.AddRange(bottomPlainsBlocks);
            GeneratedMap.MapData.AddRange(plainsBlocks);

            return GeneratedMap;
        }

        public BaseMap<short> GenerateTutorialHills()
        {
            short blockY;
            float noise;

            // Generate lakes
            List<Block> lakesToUse = new List<Block>();
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
                blockY = (short)(0 - (minLakeY + height - lakesToUse[i].Y));
                GeneratedMap.Add(new Block
                {
                    X = lakesToUse[i].X,
                    Y = blockY,
                    Z = lakesToUse[i].Z,
                    Style = LakesBlockStyle(blockY)
                });
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
                            Style = PlainsBlockStyle(blockY)
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
                        Style = PlainsBlockStyle(y)
                    });
                }
            }
            plainsBlocks.AddRange(bottomPlainsBlocks);
            GeneratedMap.MapData.AddRange(plainsBlocks);

            // Create hills
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

        private bool DoesTunnelDestroyBlock(short distance = 1)
        {
            if (distance == 1) return true;

            Random rand = new Random();
            return rand.NextDouble() < Math.Pow(0.5d, distance - 1);
        }

        private Block.STYLE LakesBlockStyle(short y)
        {
            //return Options.Biome == MapGeneratorOptions.MapBiome.Tundra ?
            //    (y - 2 <= GeneratedMap.MinHeight ? Block.STYLE.GrayCraters : Block.STYLE.Ice) :
            //    (y - 2 <= GeneratedMap.MinHeight ? Block.STYLE.Lava : Block.STYLE.Water);
            return Options.Biome == MapGeneratorOptions.MapBiome.Tundra ? Block.STYLE.Ice : Block.STYLE.Water;
        }        

        private Block.STYLE PlainsBlockStyle(short y)
        {
            return Options.Biome == MapGeneratorOptions.MapBiome.Tundra ? Block.STYLE.Snow : Options.Biome == MapGeneratorOptions.MapBiome.Desert ? Block.STYLE.Sand : Block.STYLE.Grass;
        }

        private Block.STYLE UnderPlainsBlockStyle(short y)
        {
            if (y - 2 <= GeneratedMap.MinHeight) return Block.STYLE.Lava;
            if ((y + 2) - 22 <= GeneratedMap.MinHeight) return Block.STYLE.GrayCraters;

            return Options.Biome == MapGeneratorOptions.MapBiome.Tundra ? Block.STYLE.Snow : Options.Biome == MapGeneratorOptions.MapBiome.Desert ? Block.STYLE.Sand : Block.STYLE.Dirt;
        }

        private Block.STYLE HillsBlockStyle()
        {
            return Options.Biome == MapGeneratorOptions.MapBiome.Tundra ? Block.STYLE.Snow : Options.Biome == MapGeneratorOptions.MapBiome.Desert ? Block.STYLE.Sand : Block.STYLE.GrassStone;
        }

        private Block.STYLE UnderHillsBlockStyle()
        {
            return Options.Biome == MapGeneratorOptions.MapBiome.Tundra ? Block.STYLE.Snow : Options.Biome == MapGeneratorOptions.MapBiome.Desert ? Block.STYLE.Sand : Block.STYLE.Dirt;
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

        private Block.STYLE CaveBlockStyle(short y)
        {
            if (y == GeneratedMap.MinHeight)
                return Options.Biome == MapGeneratorOptions.MapBiome.Tundra ? Block.STYLE.Ice : Block.STYLE.Lava;

            return Options.Biome == MapGeneratorOptions.MapBiome.Desert ? Block.STYLE.Sand : Block.STYLE.GrayCraters;
        }
    }
}
