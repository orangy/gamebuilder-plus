using GBWorldGen.Core.Algorithms.Generators.Abstractions;
using System;

namespace GBWorldGen.Core.Algorithms.Generators
{
    public class MapGeneratorOptions : BaseGeneratorOptions
    {
        public bool Actors { get; set; } = false;
        public float PlainFrequency { get; set; } = 0.02F;
        public bool Lakes { get; set; } = true;
        public float LakeFrequency { get; set; } = 0.015F;
        public int LakeSize { get; set; } = 7;        
        public bool Hills { get; set; } = true;
        public float HillFrequency { get; set; } = 0.028F;
        public int HillClamp { get; set; } = 50;
        public bool Mountains { get; set; } = true;
        public float MountainFrequency { get; set; } = 0.04F;
        public double AdditionalMountainSize { get; set; } = 0.55D;
        public bool Caves { get; set; } = true;
        public int AdditionalCaveHeight { get; set; } = 25;
        public bool Tunnels { get; set; } = true;
        public int TunnelWormsMax { get; set; } = 8;
        public int TunnelRadius { get; set; } = 2;
        public int TunnelLength { get; set; } = 19;
        public MapBiome Biome { get; set; } = MapBiome.Grassland;

        public MapGeneratorOptions()
        {

        }

        public override string ToString()
        {
            string biome = $"Biome: '{Enum.GetName(typeof(MapBiome), Biome)}'.";
            string actors = Actors ? $"Actors: '{Actors}'." : string.Empty;
            string lakes = Lakes ? $"Lake frequency: '{LakeFrequency}'. Lake size: '{LakeSize}'." : string.Empty;
            string hills = Hills ? $"Hill frequency: '{HillFrequency}'. Hill clamp: '{HillClamp}'." : string.Empty;
            string mountains = Mountains ? $"Mountain frequency: '{MountainFrequency}'. Additional mountain size: '{AdditionalMountainSize}'." : string.Empty;
            string caves = Caves ? $"Caves: '{Caves}'. Additional cave height: '{AdditionalCaveHeight}'." : string.Empty;
            string tunnels = Tunnels ? $"Tunnels (max): '{TunnelWormsMax}'. Tunnel radius: '{TunnelRadius}'. Tunnel Length: '{TunnelLength}'." : string.Empty;

            return $"{biome} Plain frequency: '{PlainFrequency}'. {actors} {lakes} {hills} {mountains} {caves} {tunnels}";
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
