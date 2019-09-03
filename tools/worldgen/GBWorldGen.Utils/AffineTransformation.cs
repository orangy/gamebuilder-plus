namespace GBWorldGen.Misc.Utils
{
    public static class AffineTransformation
    {
        public static short MapToWorld(float value, float min, float max)
        {
            // Apply affine transformation;
            // https://math.stackexchange.com/a/377174/476642
            float x = value;
            float a = -1.0F;
            float b = 1.0F;
            float c = min;
            float d = max;

            return (short)(((x - a) * ((d - c) / (b - a))) + c);
        }
    }
}
