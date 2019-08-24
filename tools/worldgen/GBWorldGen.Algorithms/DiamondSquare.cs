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
        private bool[] BlocksSet { get; set; }
        private Random Random { get; set; } = new Random();
        private int FullWidth { get; }

        public DiamondSquare(int x, int y, int z, int width)
        {
            X = x;
            Y = y;
            Z = z;
            Width = width;
            FullWidth = (int)Math.Pow(Width, 2.0d) + 1;

            Blocks = new Block[FullWidth * FullWidth];
            BlocksSet = new int[FullWidth * FullWidth];
            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i].x = (short)(i % FullWidth < Width
                    ? (-1 * (Width - (i % FullWidth))) + X
                    : i % FullWidth > Width
                        ? (i % FullWidth) - Width + X
                        : X);                    
                Blocks[i].y = (short)(Y + Random.Next(-3, 4));
                Blocks[i].z = (short)(i / FullWidth < Width
                    ? (-1 * (Width - (i / FullWidth))) + Z
                    : i / FullWidth > Width
                        ? (i / FullWidth) - Width + Z
                        : Z);                    

                Blocks[i].shape = Block.SHAPE.Box;
                Blocks[i].direction = Block.DIRECTION.East;
                Blocks[i].style = Block.STYLE.Blue;
            }
        }

        public Block[] Generate()
        {
            // Corners
            Blocks[0].y = (short)Random.Next(-3, 4);
            Blocks[Width - 1].y = (short)Random.Next(-3, 4);
            Blocks[Width * (Width - 1)].y = (short)Random.Next(-3, 4);
            Blocks[Width * Width - 1].y = (short)Random.Next(-3, 4);
            BlocksSet[0] = true;
            BlocksSet[Width - 1] = true;
            BlocksSet[Width * (Width - 1)] = true;
            BlocksSet[Width * Width - 1] = true;

            int span = Width;

            // Diamond
            for (int i = 0; i < Blocks.Length; i++)
            {
                if (i % FullWidth < FullWidth - 1 &&
                    i / FullWidth < FullWidth - 1 &&
                    BlocksSet[i])
                {
                    Diamond(i + (FullWidth * span) + span, span);
                    BlocksSet[i + (FullWidth * span) + span] = true;
                }
            }

            // Square
            for (int i = 0; i < Blocks.Length; i++)
            {
                if ((i - span >= 0 && BlocksSet[i - span]) ||
                    (i + span < Blocks.Length && BlocksSet[i + span]) ||
                    )
            }

            return Blocks;
        }

        private void Diamond(int index, int step)
        {            
            Blocks[index].y += Average(
                Blocks[index - step - (FullWidth * step)].y,
                Blocks[index - step + (FullWidth * step)].y,
                Blocks[index + step - (FullWidth * step)].y,
                Blocks[index + step + (FullWidth * step)].y
            );
        }

        private void Square(int index, int step)
        {

        }

        private short Average(params short[] values)
        {
            short total = 0;

            for (int i = 0; i < values.Length; i++)
                total += values[i];

            return total;
        }
    }
}
