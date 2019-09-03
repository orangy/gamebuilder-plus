namespace GBWorldGen.Core.Algorithms.Methods
{
    public abstract class MethodBase
    {
        public abstract float Create(float x, float z);
        public abstract float CreateOctave(float x, float z, int octaves);
        public abstract float Create(float x, float y, float z);
        public abstract float CreateOctave(float x, float y, float z, int octaves);
    }
}
