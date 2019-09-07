using GBWorldGen.Core.Algorithms.Abstractions;
using GBWorldGen.Core.Algorithms.Methods;
using GBWorldGen.Core.Models;
using GBWorldGen.Misc.Utils;
using System;

namespace GBWorldGen.Core.Algorithms.Generators.Abstractions
{   
    public class DefaultGenerator : Base2DGenerator, IGenerateWorld
    {
        private PerlinNoise Perlin { get; }

        public DefaultGenerator(int width, int length, BaseGeneratorOptions options = null) : base(width, length/*, options*/)
        {
            Perlin = new PerlinNoise();
        }

        public override Map Generate()
        {            
            float heightResult = 0;

            float ni = 0;
            float nj = 0;

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    nj = ((float)j / Length);
                    ni = ((float)i / Width);

                    heightResult = LoadOptions(ni, nj);
                    YValues[i, j] = AffineTransformation.MapToWorld(heightResult, MinWorldY, MaxWorldY);
                }
            }

            return base.Generate();
        }

        private float LoadOptions(float x, float y)
        {
            return x * y;
            //float octave = Perlin.CreateOctave(
            //    Options.HillFrequency * x,
            //    Options.HillFrequency * y, Options.Rigidness);

            //if (Options.Pull != 1.0F)
            //    octave = (float)Math.Pow(octave, Options.Pull);

            //return octave;
        }
    }

    //public class BaseGeneratorOptions
    //{
    //    public float HillFrequency { get; set; } = 1.0F;
    //    public int Rigidness { get; set; } = 1;
    //    public float Pull { get; set; } = 1.0F;

    //    public BaseGeneratorOptions()
    //    {

    //    }

    //    public BaseGeneratorOptions(float hillFrequency, int rigidness, float pull)
    //    {
    //        HillFrequency = hillFrequency;
    //        Rigidness = rigidness;
    //        Pull = pull;
    //    }
    //}
}
