using GBWorldGen.Core.Algorithms.Generators.Abstractions;
using GBWorldGen.Core.Algorithms.Methods;
using GBWorldGen.Core.Models;
using GBWorldGen.Core.Models.Abstractions;
using GBWorldGen.Misc.Utils;

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
            FastNoise = new FastNoise();
        }

        public override BaseMap<short> GenerateMap()
        {
            if (Height == 1)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int z = 0; z < Length; z++)
                    {
                        GeneratedMap.Add(new Block
                        {
                            X = (short)(x + GeneratedMap.OriginWidth),
                            Y = (short)(Generate2DNoise((float)x, (float)z) + GeneratedMap.OriginHeight),
                            Z = (short)(z + GeneratedMap.OriginLength)
                        });
                    }
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

        public override float Generate2DNoise(float x, float z)
        {
            return AffineTransformation.MapToWorld(FastNoise.GetPerlin(x, z),
                GeneratedMap.MinHeight, GeneratedMap.MaxHeight);
        }

        public override float Generate3DNoise(float x, float z, float y)
        {
            return FastNoise.GetPerlin(x, z, y);
        }
    }
}
