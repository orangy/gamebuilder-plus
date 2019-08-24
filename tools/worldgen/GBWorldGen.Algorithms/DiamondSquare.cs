using System;
using GBWorldGen.Models;

namespace GBWorldGen.Algorithms
{
    public class DiamondSquare : IAlgorithm
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        private Block[] Blocks { get; set; }

        public DiamondSquare(int x, int y, int z, int width)
        {
            X = x;
            Y = y;
            Z = z;
            Width = width;

            Blocks = new Block[Width * Width];
            for (int i = 0; i < Width * Width; i++)
            {
                Blocks[i].x = (short)(i % Width);
                Blocks[i].z = 
            }
        }

        public Block[] Generate()
        {
            // Corners
            Blocks[0].y = 2;
            Blocks[Width - 1].y = 2;
            Blocks[Width * (Width - 1)]
            throw new NotImplementedException();
        }
    }
}
