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
        private Random Random { get; set; } = new Random();

        public DiamondSquare(int x, int y, int z, int width)
        {
            X = x;
            Y = y;
            Z = z;
            Width = width;

            Blocks = new Block[(int)Math.Pow(Math.Pow(Width, 2.0d) + 1, 2.0d)];
            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i].x = (short)((i % Width) - Width + X);
                Blocks[i].y = (short)Y;// (short)(Y + Random.Next(-3, 4));
                Blocks[i].z = (short)((i / Width) - Width + Z);

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

            Diamond((Blocks.Length - 1) / 2, Width);

            return Blocks;
        }

        private void Diamond(int index, int step)
        {
            int rowLength = (int)Math.Pow(Width, 2.0d) + 1;
            Blocks[index].y += Average(
                Blocks[index - step - (rowLength * step)].y,
                Blocks[index - step + (rowLength * step)].y,
                Blocks[index + step - (rowLength * step)].y,
                Blocks[index + step + (rowLength * step)].y
            );

            Square(0, step);
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
