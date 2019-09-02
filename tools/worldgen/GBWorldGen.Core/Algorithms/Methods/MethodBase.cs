namespace GBWorldGen.Core.Algorithms.Methods
{
    public abstract class MethodBase
    {
        public abstract float Create(float x, float y);
        public abstract float CreateOctave(float x, float y, int octaves);
    }
}
