using GBWorldGen.Core.Algorithms.Generators;
using GBWorldGen.Core.Models;
using Xunit;

namespace GeneratorTests
{
    public class DefaultGeneratorTests
    {
        [Theory]
        [InlineData(100, 100)]
        public void DefaultGeneratorTests_Creates_Valid_YBlocks(int x, int z)
        {
            TestWorldData testWorldData = new TestWorldData();
            BaseGenerator defaultGenerator = new DefaultGenerator(x, z);
            Map myMap = defaultGenerator.Generate();
            bool valid = true;

            for (int i = 0; i < myMap.BlockData.Length; i++)
                if (!testWorldData.IsValidY(myMap.BlockData[i].Y))
                    valid = false;

            Assert.True(valid);
        }

        [Theory]
        [InlineData(100, 100)]
        public void DefaultGeneratorTests_Creates_Appropriate_Sized_Map(int x, int z)
        {
            TestWorldData testWorldData = new TestWorldData();
            BaseGenerator defaultGenerator = new DefaultGenerator(x, z);
            Map myMap = defaultGenerator.Generate();
            bool valid = true;

            if (myMap.BlockData.Length != (x * z)) valid = false;
            Assert.True(valid);
        }
    }
}
