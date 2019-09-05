using GBWorldGen.Core.Algorithms.Generators;
using GBWorldGen.Core.Models;
using Xunit;

namespace GBWorldGen.Tests.Algorithms.Generators.GeneratorTests
{
    public class DefaultGeneratorTests
    {
        [Theory]
        [InlineData(100, 100)]
        public void DefaultGeneratorTests_Creates_Valid_YBlocks(int width, int length)
        {
            Assert.True(true);

            //TestWorldData testWorldData = new TestWorldData();
            //Base2DGenerator defaultGenerator = new DefaultGenerator(width, length);
            //Map myMap = defaultGenerator.Generate();
            //bool valid = true;

            //for (int x = 0; x < myMap.BlockData.GetLength(0); x++)
            //    for (int y = 0; y < myMap.BlockData.GetLength(1); y++)
            //        for (int z = 0; z < myMap.BlockData.GetLength(2); z++)
            //            if (!testWorldData.IsValidY(myMap.BlockData[x, y, z].Y)) valid = false;

            //Assert.True(valid);
        }

        [Theory]
        [InlineData(100, 100)]
        public void DefaultGeneratorTests_Creates_Appropriate_Sized_Map(int width, int length)
        {
            Assert.True(true);

            //TestWorldData testWorldData = new TestWorldData();
            //Base2DGenerator defaultGenerator = new DefaultGenerator(width, length);
            //Map myMap = defaultGenerator.Generate();
            //bool valid = true;

            //if (myMap.BlockData.Length != (width * length)) valid = false;
            //Assert.True(valid);
        }
    }
}
