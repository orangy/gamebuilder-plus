using GBWorldGen.Core.Algorithms;

namespace GBWorldGen.Tests.Algorithms.Generators.GeneratorTests
{
    public class TestWorldData : WorldData
    {
        public TestWorldData()
        {

        }

        public bool IsValidY(short y)
            => y >= MinWorldY && y <= MaxWorldY;
    }
}
