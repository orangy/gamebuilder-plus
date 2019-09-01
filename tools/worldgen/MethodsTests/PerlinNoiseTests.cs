using Algorithms.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MethodsTests
{
    public class PerlinNoiseTests
    {
        [Fact]
        public void PerlinNoiseReturns_Valid_Values()
        {
            PerlinNoise perlin = new PerlinNoise();
            List<float> values = new List<float>();

            for (int i = 0; i < 10000; i++)
                values.Add(perlin.Create(i, i+2));

            float min = values.Min();
            float max = values.Max();

            Assert.InRange(min, 0, 1);
        }
    }
}
