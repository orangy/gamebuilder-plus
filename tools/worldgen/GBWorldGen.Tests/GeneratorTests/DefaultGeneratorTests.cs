using GBWorldGen.Core.Algorithms.Generators;
using GBWorldGen.Core.Models;
using Xunit;

namespace GeneratorTests
{
    public class DefaultGeneratorTests
    {
        [Theory]
        [InlineData(10, 10)]
        public void DefaultGeneratorTests_Creates_Valid_Map(int x, int y)
        {
            BaseGenerator defaultGenerator = new DefaultGenerator(x, y);
            Map myMap = defaultGenerator.Generate();

            Assert.True(false);
        }
    }
}
