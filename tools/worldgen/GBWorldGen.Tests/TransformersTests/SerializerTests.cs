using GBWorldGen.Core.Algorithms.Generators;
using GBWorldGen.Core.Algorithms.Transformers;
using GBWorldGen.Core.Models;
using GBWorldGen.Core.Voos;
using Xunit;

namespace TransformersTests
{
    public class SerializerTests
    {
        [Theory]
        [InlineData(100, 100)]
        public void Serializer_Serializes_All_Data_Properly(int x, int z)
        {
            // CREATE MAP
            Map myMap = new Map();
            Block[] before = null;
            Block[] after = null;
            BaseGenerator generator = new DefaultGenerator(x, z);
            myMap = generator.Generate();
            before = myMap.BlockData;

            string serialized = Serializer.SerializeMap(before);
            after = Deserializer.DeserializeMap(serialized);
            bool valid = true;

            for (int i = 0; i < before.Length; i++)
                if (!before[i].Equals(after[i])) valid = false;

            Assert.True(valid);
        }
    }
}
