using GBWorldGen.Core.Algorithms.Generators.Abstractions;
using GBWorldGen.Core.Models;
using GBWorldGen.Core.Models.Abstractions;

namespace Algorithms.Generators
{
    public class MapGenerator : BaseGenerator<short>
    {        
        public MapGenerator(short width, short length, short height)
            : base(width, length, height)
        {
            GeneratedMap = new Map(width, length, height);
            Noise2D = Generate2DNoise;
        }

        public override BaseMap<short> GenerateMap()
        {
            if (Height == default)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int z = 0; z < Length; z++)
                    {
                        GeneratedMap.add
                    }
                }
            }
            else
            {

            }
            throw new System.NotImplementedException();
        }

        public float Generate2DNoise(float x, float z)
        {

        }
    }
}
