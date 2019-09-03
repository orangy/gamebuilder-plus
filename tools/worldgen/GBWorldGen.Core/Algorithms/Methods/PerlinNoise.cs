using System;

namespace GBWorldGen.Core.Algorithms.Methods
{
    public class PerlinNoise : MethodBase
    {
        private Perlin Perlin { get; }

        public PerlinNoise()
        {
            Perlin = new Perlin();
        }

        public override float Create(float x, float z)
        {
            return Perlin.Noise(x + 0.01F, z + 0.01F);
        }

        public override float CreateOctave(float x, float z, int octaves)
        {
            float result = 0;

            if (octaves <= 0) octaves = 1;
            for (int i = 0; i < octaves; i++)
            {
                result += (1.0F / ((float)(i + 1))) * Perlin.Noise(
                    (float)(Math.Pow(2.0, i)) * (x + 0.01F), 
                    (float)(Math.Pow(2.0, i)) * (z + 0.01F));
            }

            return result;
        }

        public override float Create(float x, float y, float z)
        {
            return Perlin.Noise(x + 0.01F, y + 0.01F, z + 0.01F);
        }

        public override float CreateOctave(float x, float y, float z, int octaves)
        {
            float result = 0;

            if (octaves <= 0) octaves = 1;
            for (int i = 0; i < octaves; i++)
            {
                result += (1.0F / ((float)(i + 1))) * Perlin.Noise(
                    (float)(Math.Pow(2.0, i)) * (x + 0.01F),
                    (float)(Math.Pow(2.0, i)) * (z + 0.01F));
                result += (1.0F / ((float)(i + 1))) * Perlin.Noise(
                    (float)(Math.Pow(2.0, i)) * (x + 0.01F),
                    (float)(Math.Pow(2.0, i)) * (y + 0.01F));
                result += (1.0F / ((float)(i + 1))) * Perlin.Noise(
                    (float)(Math.Pow(2.0, i)) * (y + 0.01F),
                    (float)(Math.Pow(2.0, i)) * (z + 0.01F));
            }

            return result;
        }
    }
}
