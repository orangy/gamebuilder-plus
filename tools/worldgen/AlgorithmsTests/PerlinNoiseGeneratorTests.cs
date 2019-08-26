using Algorithms.Generators;
using System;
using System.Collections.Generic;
using Xunit;

namespace AlgorithmsTests
{
    public class PerlinNoiseGeneratorTests
    {
        [Fact]
        public void PerlinNoiseGenerator_Returns_Valid_Values()
        {
            PerlinNoiseGenerator generator = new PerlinNoiseGenerator();
            float scalar = 2.3F;
            int x = 2;
            int y = 15;

            double result = generator.Noise(x * scalar, y * scalar);

            Assert.InRange(result, 0.0, 1.0);
        }

        [Fact]
        public void PerlinNoiseGenerator_Returns_One_NonZero_Value()
        {
            PerlinNoiseGenerator generator = new PerlinNoiseGenerator();
            List<double> results = new List<double>();
            int size = 50;
            float scalar = 1.57F;

            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    float ni = i * scalar;
                    float nj = j * scalar;
                    results.Add(generator.Noise(ni, nj));
                }                    

            Assert.Contains(results, r => r % 1 != 0);
        }
    }
}
