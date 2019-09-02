using GBWorldGen.Core.Algorithms.Methods;
using System;
using Xunit;

namespace MethodsTests
{
    public class PerlinNoiseTests
    {
        [Fact]
        public void PerlinNoiseReturns_Valid_Values()
        {
            Random random = new Random();
            PerlinNoise perlin = new PerlinNoise();

            for (int i = 0; i < 100; i++)
            {
                float x = (float)(random.NextDouble() * 25);
                float y = (float)(random.NextDouble() * 25);
                float value = perlin.Create(x, y);

                Assert.InRange(value, -1, 1);
            }
        }
    }
}