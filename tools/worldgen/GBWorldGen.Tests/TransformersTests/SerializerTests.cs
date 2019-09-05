using GBWorldGen.Core.Algorithms.Generators;
using GBWorldGen.Core.Algorithms.Generators.Abstractions;
using GBWorldGen.Core.Algorithms.Transformers;
using GBWorldGen.Core.Algorithms.Transformers.Abstractions;
using GBWorldGen.Core.Models;
using GBWorldGen.Core.Models.Abstractions;
using Xunit;

namespace GBWorldGen.Tests.Transformers.TransformersTests
{
    public class SerializerTests
    {
        [Theory]
        [InlineData(5, 5)]
        public void Serializer_Serializes_All_Data_Properly(short x, short z)
        {
            BaseGenerator<short> mapGenerator = new MapGenerator(x, z, 1);
            BaseMap<short> map = mapGenerator.GenerateMap();

            BaseSerializer<short> serializer = new Serializer();
            string serialized = serializer.Serialize(map);

            BaseDeserializer<short> deserializer = new Deserializer();
            BaseMap<short> newMap = deserializer.Deserialize(serialized);

            // Validate
            Assert.Equal(map.MapData.Count, newMap.MapData.Count);
            for (int i = 0; i < map.MapData.Count; i++)
            {
                Assert.Equal((Block)map.MapData[i], (Block)newMap.MapData[i]);
            }
        }
    }
}
