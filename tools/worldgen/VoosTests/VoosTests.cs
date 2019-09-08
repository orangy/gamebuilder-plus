using GBWorldGen.Core.Algorithms.Generators;
using GBWorldGen.Core.Algorithms.Generators.Abstractions;
using GBWorldGen.Core.Models;
using GBWorldGen.Core.Models.Abstractions;
using GBWorldGen.Core.Voos;
using System;
using Xunit;

namespace GBWorldGen.Tests.Voos.VoosTests
{
    public class VoosTests
    {
        [Theory]
        [InlineData(100, 100)]
        public void Voos_Create_Map(short x, short z)
        {
            string mapName = $"CustomMap-{DateTime.Now.ToString("MM_dd_yyyy hh_mm tt")}";
            string outputDirectory = @"D:\Program Files (x86)\Steam\steamapps\common\Game Builder\GameBuilderUserData\Games";

            BaseGenerator<short> generator = new MapGenerator(x, z, 1);
            Map map = (Map)generator.GenerateMap();
            VoosGenerator voosGenerator = new VoosGenerator();
            voosGenerator.Generate(map, outputDirectory, mapName);
        }

        [Theory]
        [InlineData(100, 100)]
        public void Tutorial_Create_Perlin_Map(short x, short z)
        {
            string mapName = $"CustomMap-{DateTime.Now.ToString("MM_dd_yyyy hh_mm tt")}";
            string outputDirectory = @"D:\Program Files (x86)\Steam\steamapps\common\Game Builder\GameBuilderUserData\Games";

            MapGenerator generator = new MapGenerator(x, z, 1);
            Map map = (Map)generator.GenerateTutorialPerlin();
            VoosGenerator voosGenerator = new VoosGenerator();
            voosGenerator.Generate(map, outputDirectory, mapName);
        }

        [Theory]
        [InlineData(100, 100)]
        public void Tutorial_Create_Lakes(short x, short z)
        {
            string mapName = $"CustomMap-{DateTime.Now.ToString("MM_dd_yyyy hh_mm tt")}";
            string outputDirectory = @"D:\Program Files (x86)\Steam\steamapps\common\Game Builder\GameBuilderUserData\Games";

            MapGenerator generator = new MapGenerator(x, z, 1);
            Map map = (Map)generator.GenerateTutorialLakes();
            VoosGenerator voosGenerator = new VoosGenerator();
            voosGenerator.Generate(map, outputDirectory, mapName);
        }

        [Theory]
        [InlineData(100, 100)]
        public void Tutorial_Create_Plain(short x, short z)
        {
            string mapName = $"CustomMap-{DateTime.Now.ToString("MM_dd_yyyy hh_mm tt")}";
            string outputDirectory = @"D:\Program Files (x86)\Steam\steamapps\common\Game Builder\GameBuilderUserData\Games";

            MapGenerator generator = new MapGenerator(x, z, 1);
            Map map = (Map)generator.GenerateTutorialPlain();
            VoosGenerator voosGenerator = new VoosGenerator();
            voosGenerator.Generate(map, outputDirectory, mapName);
        }

        [Theory]
        [InlineData(100, 100)]
        public void Tutorial_Create_Hills(short x, short z)
        {
            string mapName = $"CustomMap-{DateTime.Now.ToString("MM_dd_yyyy hh_mm tt")}";
            string outputDirectory = @"D:\Program Files (x86)\Steam\steamapps\common\Game Builder\GameBuilderUserData\Games";

            MapGenerator generator = new MapGenerator(x, z, 1);
            Map map = (Map)generator.GenerateTutorialHills();
            VoosGenerator voosGenerator = new VoosGenerator();
            voosGenerator.Generate(map, outputDirectory, mapName);
        }        
    }
}
