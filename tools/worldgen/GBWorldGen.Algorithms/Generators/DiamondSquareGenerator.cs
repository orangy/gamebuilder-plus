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
        private int JitterCount { get; set; }
        private int JitterMin { get; set; }
        private int JitterMax { get; set; }
        private int FullWidth { get; set; }
        Block.STYLE DefaultBlockStyle { get; set; }

        public DiamondSquareGenerator(int x, int y, int z, int width, int jitter = 4, Block.STYLE defaultBlockStyle = Block.STYLE.Grass)
        {
            // Width can't be over 9;
            // Max map size is 500x500 = 2^9 = 512
            // (we will trim extra blocks at end)
            // however... any width over 8 freezes
            // the game, so we cap at 8 for time being
            if (width > 8)
                width = 8;

            X = x;
            Y = y;
            Z = z;
            FullWidth = (int)Math.Pow(2.0d, width) + 1;
            Width = (FullWidth - 1) / 2;
            JitterCount = jitter;

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

            return Blocks.ToArray();
        }

        private void Initialize()
        {
            IntializeJitter();

            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i].X = (short)(i % FullWidth < Width
                    ? (-1 * (Width - (i % FullWidth))) + X
                    : i % FullWidth > Width
                        ? (i % FullWidth) - Width + X
                        : X);
                Blocks[i].Y = (short)(Y + Random.Next(-2, 2));
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
        
        private void IntializeJitter()
        {
            for (int i = 1; i <= JitterCount; i++)
            {
                if (i % 2 == 1)
                    JitterMax++;
                else
                    JitterMin--;
            }
        }

        private void TrimExcess()
        {
            if (Blocks.Length > 500 * 500)
                Blocks = Blocks.Where(
                    (block, index) => index % 500 < 500 && index / 500 < 500).ToArray();
        }

        private short Jitter()
        {
            return (short)Random.Next(JitterMin, JitterMax);
        }

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
