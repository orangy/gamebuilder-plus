using GBWorldGen.Core.Algorithms.Methods;
using System;
using Xunit;

namespace GBWorldGen.Tests.Methods.MethodsTests
{
    public class FastNoiseTests
    {
        [Fact]
        public void FastNoise_GetPerlin_Returns_Valid_Values()
        {
            Random random = new Random();
            FastNoise noise = new FastNoise();

            for (int i = 0; i < 1000; i++)
            {
                float x = (float)(random.NextDouble() * 25);
                float z = (float)(random.NextDouble() * 25);
                float value = noise.GetPerlin(x, z);

                Assert.InRange(value, -1, 1);
            }
        }
    }
}
