using Algorithms.Methods;
using GBWorldGen.Core.Algorithms.Abstractions;
using GBWorldGen.Core.Models;
using System;

namespace Algorithms.Generators
{
    // https://github.com/UnknownShadow200/ClassiCube/wiki/Minecraft-Classic-map-generation-algorithm
    public class DefaultGenerator : BaseGenerator, IGenerateWorld
    {
        private PerlinNoise Perlin { get; }

        public DefaultGenerator(int width, int length) : base(width, length)
        {
            Perlin = new PerlinNoise();
            YValues = new short[width * length];
        }

        public override Map Generate(params float[] values)
        {
            if (values == null) values = new float[1] { 1.0F };

            float heightResult = 0;

            float ni = 0;
            float nj = 0;

            for (int i = 1; i <= Width; i++)
            {
                for (int j = 1; j <= Length; j++)
                {
                    nj = ((float)j / Length) - 0.5F;
                    ni = ((float)i / Width) - 0.5F;

                    heightResult = LoadOptions(ni, nj, values);
                    YValues[((i - 1) * Length) + (j - 1)] = MapToWorldBounds(heightResult);
                }
            }

            return base.Generate();
        }

        private float LoadOptions(float x, float y, params float[] values)
        {
            if (values == null || values.Length < 1) return 0.0F;

            float noise1 = 0;
            float noise2 = 0;
            float noise3 = 0;

            switch (values[0])
            {
                case 1.0F:
                    // Boring
                    return Perlin.Create(x, y);
                case 2.0F:
                    {
                        // Steep cliffs, lots of water
                        int multiplier = 2;
                        if (values.Length >= 2)
                            multiplier = (int)Math.Floor(values[1]);

                        noise1 = Perlin.Create(x, y);
                        noise2 = Perlin.Create(x + Perlin.CreateOctave(x, y, multiplier * 2), y + Perlin.CreateOctave(x, y, multiplier));
                        noise3 = Perlin.Create(noise1, noise2);
                        return noise3;
                    }
                case 3.0F:
                    // One big hill with lake
                    noise1 = Perlin.Create((float)Math.Pow(2.0d, (x / 3.1F) * 2.9), (float)Math.Pow(2.0d, (y / 3.1F) * 2.9));
                    return noise1;
                case 4.0F:
                    // Oval lake with oval mountain
                    noise1 = Perlin.Create(Perlin.Create(x, y), Perlin.Create(x, y));
                    return noise1;
                case 5.0F:
                    {
                        // Many hills
                        float multiplier = 2.0F;
                        if (values.Length >= 2)
                            multiplier = values[1];

                        noise1 = Perlin.Create(
                            Perlin.Create(x * multiplier, y * multiplier),
                            Perlin.Create(y * multiplier, x * multiplier));
                        return noise1;
                    }                    
                default:
                    break;
            }

            return 0.0F;
        }

        private short MapToWorldBounds(float value)
        {
            // Apply affine transformation;
            // https://math.stackexchange.com/a/377174/476642
            float x = value;
            float a = -1.0F;
            float b = 1.0F;
            float c = MinWorldY;
            float d = MaxWorldY;

            return (short)(((x - a) * ((d - c) / (b - a))) + c);
        }
    }
}
