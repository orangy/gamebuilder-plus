using Algorithms.Generators;
using Xunit;

namespace Tests.Algorithms.Generators
{
    public class PerlinNoiseGeneratorTests
    {
        [Fact]
        public void PerlinNoiseGenerator_Returning_Good_Values()
        {
            PerlinNoiseGenerator generator = new PerlinNoiseGenerator();
            Assert.InRange(generator.Perlin(0, 0, 0), 0.0, 1.0);
        }
    }
}
