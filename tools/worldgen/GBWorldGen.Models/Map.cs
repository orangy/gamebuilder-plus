namespace GBWorldGen.Core.Models
{
    public struct Map
    {
        public int Width;
        public int Length;
        public Block[] BlockData;

        public Map(int width, int length, Block[] blockData)
        {
            Width = width;
            Length = length;
            BlockData = blockData;
        }
    }
}
