using GBWorldGen.Core.Algorithms.Generators;
using GBWorldGen.Core.Algorithms.Transformers;
using GBWorldGen.Core.Models;
using Xunit;

namespace GeneratorTests
{
    public class PerlinNoiseGeneratorTests
    {
        [Fact]
        public void PerlinNoiseGenerator_Creates_Valid_Map()
        {
            PerlinNoiseGenerator perlinNoiseGenerator = new PerlinNoiseGenerator(20, 20);
            Map myMap = perlinNoiseGenerator.Generate();

            string serialized = Serializer.SerializeMap(myMap.BlockData);
            Assert.True(Deserializer.DeserializeMap(serialized));
        }
    }
}
