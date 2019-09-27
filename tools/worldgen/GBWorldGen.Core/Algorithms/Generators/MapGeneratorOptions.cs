using GBWorldGen.Core.Algorithms.Generators.Abstractions;
using System;

namespace GBWorldGen.Core.Algorithms.Generators
{
    public class MapGeneratorOptions : BaseGeneratorOptions
    {
        public float PlainFrequency { get; set; } = 0.02F;
        public bool Lakes { get; set; } = true;
        public float LakeFrequency { get; set; } = 0.015F;
        public int LakeSize { get; set; } = 7;
        public bool Tunnels { get; set; } = true;
        public bool Hills { get; set; } = true;
        public float HillFrequency { get; set; } = 0.028F;
        public int HillClamp { get; set; } = 50;
        public bool Mountains { get; set; } = true;
        public float MountainFrequency { get; set; } = 0.04F;
        public double AdditionalMountainSize { get; set; } = 0.55D;
        public MapBiome Biome { get; set; } = MapBiome.Grassland;

        public MapGeneratorOptions()
        {

        }

        public override string ToString()
        {
            string biome = $"Biome: '{Enum.GetName(typeof(MapBiome), Biome)}'.";
            string lakes = Lakes ? $"Lake frequency: '{LakeFrequency}'. Lake size: '{LakeSize}'." : string.Empty;
            string hills = Hills ? $"Hill frequency: '{HillFrequency}'. Hill clamp: '{HillClamp}'." : string.Empty;
            string mountains = Mountains ? $"Mountain frequency: '{MountainFrequency}'. Additional mountain size: '{AdditionalMountainSize}'." : string.Empty;

            return $"{biome} Plain frequency: '{PlainFrequency}'. {lakes} {hills} {mountains}";
        }

        public enum MapBiome
        {
            Grassland = 0,
            Desert,
            Tundra
        }

        public string[] BiomeNames()
        {
            return Enum.GetNames(typeof(MapBiome));
        }

        public string EnumName(Enum e)
        {
            return Enum.GetName(e.GetType(), e);
        }
    }
}
