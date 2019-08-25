using GBWorldGen.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GBWorldGen.Core.Algorithms
{
    public class DiamondSquareGenerator : IGenerateWorld
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        private Block[] Blocks;
        private bool[] BlocksSet { get; set; }
        private Random Random { get; set; } = new Random();
        private int FullWidth { get; set; }
        Block.STYLE DefaultBlockStyle { get; set; }

        public DiamondSquareGenerator(int x, int y, int z, int width, Block.STYLE defaultBlockStyle = Block.STYLE.Grass)
        {
            // Width can't be over 9;
            // Max map size is 500x500 = 2^9 = 512
            // (we will trim extra blocks at end)
            if (width > 9)
                width = 9;

            X = x;
            Y = y;
            Z = z;
            FullWidth = (int)Math.Pow(2.0d, width) + 1;
            Width = (FullWidth - 1) / 2;            

            Blocks = new Block[FullWidth * FullWidth];
            BlocksSet = new bool[FullWidth * FullWidth];
            DefaultBlockStyle = defaultBlockStyle;

            Initialize();
        }

        public Block[] Generate()
        {
            // Corners
            Blocks[0].Y = Jitter();
            Blocks[FullWidth - 1].Y = Jitter();
            Blocks[FullWidth * (FullWidth - 1)].Y = Jitter();
            Blocks[FullWidth * FullWidth - 1].Y = Jitter();
            BlocksSet[0] = true;
            BlocksSet[FullWidth - 1] = true;
            BlocksSet[FullWidth * (FullWidth - 1)] = true;
            BlocksSet[FullWidth * FullWidth - 1] = true;

            bool[] dontSetAgain = new bool[FullWidth * FullWidth];
            int span = Width;

            while (span > 0)
            {
                // Diamond
                for (int i = 0; i < Blocks.Length; i++)
                    if (i % FullWidth < FullWidth - 1 &&
                        i / FullWidth < FullWidth - 1 &&
                        BlocksSet[i] && !dontSetAgain[i])
                    {
                        Diamond(i + (FullWidth * span) + span, span);
                        BlocksSet[i + (FullWidth * span) + span] = true;
                        dontSetAgain[i + (FullWidth * span) + span] = true;
                    }

                for (int i = 0; i < dontSetAgain.Length; i++)
                    dontSetAgain[i] = false;

                // Square
                for (int i = 0; i < Blocks.Length; i++)
                    if ((i - span >= 0 &&
                        BlocksSet[i - span] &&
                        !dontSetAgain[i - span] &&
                        ((i - span) / FullWidth) == (i / FullWidth)) ||

                        (i + span < Blocks.Length &&
                        BlocksSet[i + span] &&
                        !dontSetAgain[i + span] &&
                        ((i + span) / FullWidth) == (i / FullWidth)))
                    {
                        Square(i, span);
                        BlocksSet[i] = true;
                        dontSetAgain[i] = true;
                    }

                if (span > 0)
                    for (int i = 0; i < dontSetAgain.Length; i++)
                        dontSetAgain[i] = false;

                span /= 2;
            }

            TrimExcess();
            FillBottom();

            return Blocks.ToArray();
        }

        private void Initialize()
        {
            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i].X = (short)(i % FullWidth < Width
                    ? (-1 * (Width - (i % FullWidth))) + X
                    : i % FullWidth > Width
                        ? (i % FullWidth) - Width + X
                        : X);
                Blocks[i].Y = (short)(Y + Jitter());
                Blocks[i].Z = (short)(i / FullWidth < Width
                    ? (-1 * (Width - (i / FullWidth))) + Z
                    : i / FullWidth > Width
                        ? (i / FullWidth) - Width + Z
                        : Z);

                Blocks[i].Shape = Block.SHAPE.Box;
                Blocks[i].Direction = Block.DIRECTION.East;
                Blocks[i].Style = DefaultBlockStyle;
            }
        }

        private void FillBottom()
        {
            int smallestY = 0;
            for (int i = 0; i < Blocks.Length; i++)
                if (Blocks[i].Y < smallestY)
                    smallestY = Blocks[i].Y;

            // Fill
            List<Block> fillBlocks = new List<Block>();
            for (int i = 0; i < Blocks.Length; i++)
                for (int j = Blocks[i].Y - 1; j >= smallestY; j--)
                {
                    fillBlocks.Add(new Block
                    {
                        X = Blocks[i].X,
                        Y = (short)j,
                        Z = Blocks[i].Z,
                        Shape = Blocks[i].Shape,
                        Direction = Blocks[i].Direction,
                        Style = Blocks[i].Style
                    });
                }

            int originalLength = Blocks.Length;
            Array.Resize(ref Blocks, originalLength + fillBlocks.Count);
            fillBlocks.ToArray().CopyTo(Blocks, originalLength);
        }

        private void TrimExcess()
        {
            if (Blocks.Length > 500 * 500)
                Blocks = Blocks.Where(
                    (block, index) => index % 500 < 500 && index / 500 < 500).ToArray();
        }

        private short Jitter() => (short)Random.Next(-2, 2);

        private void Diamond(int index, int step)
        {
            Blocks[index].Y += Average(
                Blocks[index - step - (FullWidth * step)].Y,
                Blocks[index - step + (FullWidth * step)].Y,
                Blocks[index + step - (FullWidth * step)].Y,
                Blocks[index + step + (FullWidth * step)].Y
            );
        }

        private void Square(int index, int step)
        {
            List<short> totals = new List<short>();
            if (index % FullWidth != 0 && index - step >= 0)
                totals.Add(Blocks[index - step].Y);

            if (index % FullWidth != (FullWidth - 1) && index + step < Blocks.Length)
                totals.Add(Blocks[index + step].Y);

            if (index / FullWidth != 0 && index - (FullWidth * step) >= 0)
                totals.Add(Blocks[index - (FullWidth * step)].Y);

            if (index / FullWidth != (FullWidth - 1) && index + (FullWidth * step) < Blocks.Length)
                totals.Add(Blocks[index + (FullWidth * step)].Y);

            Blocks[index].Y += Average(totals.ToArray());
        }

        private short Average(params short[] values)
        {
            short total = 0;

            for (int i = 0; i < values.Length; i++)
                total += values[i];

            return (short)(total / values.Length);
        }
    }
}
