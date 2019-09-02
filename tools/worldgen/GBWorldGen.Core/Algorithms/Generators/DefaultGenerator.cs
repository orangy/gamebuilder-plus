using GBWorldGen.Core.Algorithms.Abstractions;
using GBWorldGen.Core.Algorithms.Methods;
using GBWorldGen.Core.Models;
using System;

namespace GBWorldGen.Core.Algorithms.Generators
{   
    public class DefaultGenerator : BaseGenerator, IGenerateWorld
    {
        private PerlinNoise Perlin { get; }

        public DefaultGenerator(int width, int length, BaseGeneratorOptions options = null) : base(width, length, options)
        {
            Perlin = new PerlinNoise();
            YValues = new short[width * length];
        }

        public override Map Generate()
        {            
            float heightResult = 0;

            float ni = 0;
            float nj = 0;

            for (int i = 1; i <= Width; i++)
            {
                for (int j = 1; j <= Length; j++)
                {
                    nj = ((float)j / Length);
                    ni = ((float)i / Width);

                    heightResult = LoadOptions(ni, nj);
                    YValues[((i - 1) * Length) + (j - 1)] = MapToWorldBounds(heightResult);
                }
            }

            return base.Generate();
        }

        private float LoadOptions(float x, float y)
        {
            float octave = Perlin.CreateOctave(
                Options.HillFrequency * x,
                Options.HillFrequency * y, Options.Rigidness);

            if (Options.Pull != 1.0F)
                octave = (float)Math.Pow(octave, Options.Pull);

            return octave;
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

    public class BaseGeneratorOptions
    {
        public float HillFrequency { get; set; } = 1.0F;
        public int Rigidness { get; set; } = 1;
        public float Pull { get; set; } = 1.0F;

        public BaseGeneratorOptions()
        {

        }

        public BaseGeneratorOptions(float hillFrequency, int rigidness, float pull)
        {
            HillFrequency = hillFrequency;
            Rigidness = rigidness;
            Pull = pull;
        }
    }
}
