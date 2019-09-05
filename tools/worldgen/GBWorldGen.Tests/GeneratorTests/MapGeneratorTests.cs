using GBWorldGen.Core.Algorithms.Generators;
using GBWorldGen.Core.Algorithms.Generators.Abstractions;
using GBWorldGen.Core.Models.Abstractions;
using Xunit;

namespace GBWorldGen.Tests.Algorithms.Generators.GeneratorTests
{
    public class MapGeneratorTests
    {
        [Theory]
        [InlineData(20, 15)]
        [InlineData(30, 30)]
        public void MapGenerator_Generates_Correct_Sized_HeightMap(short x, short z)
        {
            BaseGenerator<short> mapGenerator = new MapGenerator(x, z, 1);
            BaseMap<short> map = mapGenerator.GenerateMap();

            Assert.True(map.MapData.Count == x * z);
        }

        [Theory]
        [InlineData(5, 5, 5)]
        public void MapGenerator_Generates_3D_Map(short x, short z, short y)
        {
            BaseGenerator<short> mapGenerator = new MapGenerator(x, z, y);
            BaseMap<short> map = mapGenerator.GenerateMap();

            Assert.True(map.MapData.Count > 10); // 10 is arbitrary; just needs to be above a few because the 3d map may not generate blocks at all points in the map
        }
    }
}
