using System.Collections.Generic;

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

        public static List<Block> ToList(Block[,,] blocks)
        {
            List<Block> blockList = new List<Block>(
                blocks.GetLength(0) *
                blocks.GetLength(1) *
                blocks.GetLength(2));

            for (int x = 0; x < blocks.GetLength(0); x++)
                for (int y = 0; y < blocks.GetLength(1); y++)
                    for (int z = 0; z < blocks.GetLength(2); z++)
                    {
                        blockList.Add(blocks[x, y, z]);
                    }

            return blockList;
        }

        public static Block[,,] ToBlock3DArray(List<Block> blocks, int width, int length, int height, int minWorldY, int maxWorldY)
        {
            short originX = (short)(width * 0.5d);
            short originZ = (short)(length * 0.5d);
            Block[,,] returnArray = new Block[width, height, length];

            // Necessary to get the y-indecees right
            blocks.Sort((a, b) =>
            {
                if (a.X > b.X) return 1;
                if (a.X < b.X) return -1;
                if (a.Z > b.Z) return 1;
                if (a.Z < b.Z) return -1;
                if (a.Y > b.Y) return 1;
                if (a.Y < b.Y) return -1;

                return 0;
            });

            int xtemp = 0;
            int ztemp = 0;
            int yIndex = 0;

            for (int i = 0; i < blocks.Count; i++)
            {
                if (xtemp != blocks[i].X + originX ||
                    ztemp != blocks[i].Z + originZ) yIndex = 0;
                xtemp = blocks[i].X + originX;
                ztemp = blocks[i].Z + originZ;

                returnArray[xtemp, yIndex, ztemp] = blocks[i];
                yIndex++;
            }

            return returnArray;
        }
    }
}
