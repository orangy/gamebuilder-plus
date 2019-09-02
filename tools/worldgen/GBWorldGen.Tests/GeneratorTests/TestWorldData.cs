using GBWorldGen.Core.Algorithms;

namespace GeneratorTests
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
