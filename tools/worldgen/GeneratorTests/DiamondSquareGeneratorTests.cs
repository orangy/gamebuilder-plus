using GBWorldGen.Core.Algorithms.Generators;
using GBWorldGen.Core.Algorithms.Transformers;
using GBWorldGen.Core.Models;
using Xunit;

namespace GeneratorTests
{
    public class DiamondSquareGeneratorTests
    {
        [Fact]
        public void DiamondSquareGenerator_Creates_Valid_Map()
        {
            DiamondSquareGenerator diamondSquareGenerator = new DiamondSquareGenerator(8);
            Map myMap = diamondSquareGenerator.Generate();

            string serialized = Serializer.SerializeMap(myMap.BlockData);
            Assert.True(Deserializer.DeserializeMap(serialized));
        }
    }
}
