using GBWorldGen.Core.Algorithms.Abstractions;
using GBWorldGen.Core.Models;
using System;

namespace GBWorldGen.Core.Algorithms.Generators
{
    public class Base2DGenerator : WorldData, IGenerateWorld
    {
        protected int Width { get; set; }
        protected int Length { get; set; }
        private Block.STYLE DefaultBlockStyle { get; }
        protected short[,] YValues { get; set; }
        protected BaseGeneratorOptions Options { get; set; }

        public Base2DGenerator(int width, int length, BaseGeneratorOptions options = null, Block.STYLE defaultBlockStyle = Block.STYLE.Grass)
        {
            Width = width;
            Length = length;
            DefaultBlockStyle = defaultBlockStyle;
            YValues = new short[Width, Length];

            Options = options != null ? options : new BaseGeneratorOptions();
        }

        public virtual Map Generate()
        {
            Console.WriteLine("Generating map...");

            short originX = (short)(Width * -0.5d);
            short originZ = (short)(Length * -0.5d);
            Block[,,] blocks = new Block[Width, 1, Length];

            for (int x = 0; x < YValues.GetLength(0); x++)
            {
                for (int z = 0; z < YValues.GetLength(1); z++)
                {
                    blocks[x, 0, z] = new Block
                    {
                        X = (short)(x + originX),
                        Y = YValues[x, z],
                        Z = (short)(z + originZ),
                        Shape = Block.SHAPE.Box,
                        Direction = Block.DIRECTION.East,
                        Style = DefaultBlockStyle
                    };
                }
            }

            return new Map
            {
                BlockData = blocks
            };
        }
    }
}
