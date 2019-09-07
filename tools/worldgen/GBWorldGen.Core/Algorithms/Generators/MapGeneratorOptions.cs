using GBWorldGen.Core.Algorithms.Generators.Abstractions;

namespace GBWorldGen.Core.Algorithms.Generators
{
    public class MapGeneratorOptions : BaseGeneratorOptions
    {
        public float LakeFrequency { get; set; } = 0.015F;
        public int LakeSize { get; set; } = 7;
        public float PlainFrequency { get; set; } = 0.02F;
        public float HillFrequency { get; set; } = 0.028F;
        public int HillClamp { get; set; } = 50;
        public float MountainFrequency { get; set; } = 0.05F;
        public double AdditionalMountainSize { get; set; } = 0.55D;

        public MapGeneratorOptions()
        {

        }

        public override string ToString()
        {
            return $"Lake frequency: '{LakeFrequency}'. Lake size: '{LakeSize}'. Plain frequency: '{PlainFrequency}'. Hill frequency: '{HillFrequency}'. Hill clamp: '{HillClamp}'. Mountain frequency: '{MountainFrequency}'. Additional mountain size: '{AdditionalMountainSize}'.";
        }
    }
}
