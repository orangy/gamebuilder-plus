namespace GBWorldGen.Core.Models
{
    public struct Map
    {
        public int Width { get { return BlockData.GetLength(0); } }
        public int Length { get { return BlockData.GetLength(2); } }
        public int Height { get { return BlockData.GetLength(1); } }

        public int VoosWidth { get { return (int)(Width * 2.5d); } }
        public int VoosLength { get { return (int)(Length * 2.5d); } }

        public Block[,,] BlockData;

        public Map(Block[,,] blockData)
        {
            BlockData = blockData;
        }
    }
}
