using GBWorldGen.Core.Algorithms.Abstractions;
using GBWorldGen.Core.Models;
using System;

namespace GBWorldGen.Core.Algorithms.Generators
{
    public class Base3DGenerator : WorldData, IGenerateWorld
    {
        protected int Width { get; set; }
        protected int Length { get; set; }
        protected int Height { get; set; }
        private Block.STYLE DefaultBlockStyle { get; }
        protected BaseGeneratorOptions Options { get; set; }

        public Base3DGenerator(int width, int length, int height, BaseGeneratorOptions options = null, Block.STYLE defaultBlockStyle = Block.STYLE.Grass)
        {
            Width = width;
            Length = length;
            Height = height;
            DefaultBlockStyle = defaultBlockStyle;

            Options = options != null ? options : new BaseGeneratorOptions();
        }

        public virtual Map Generate()
        {
            Console.WriteLine("Generating map...");

            int maxY = Height - MinWorldY > MaxWorldY - MinWorldY 
                ? MaxWorldY - MinWorldY
                : Height;
            short originX = (short)(Width * -0.5d);
            short originZ = (short)(Length * -0.5d);
            Block[,,] blocks = new Block[Width, maxY, Length];

            for (int y = MinWorldY; y < (MinWorldY + Height); y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int z = 0; z < Length; z++)
                    {
                        blocks[x, (y - MinWorldY), z] = new Block
                        {
                            X = (short)(x + originX),
                            Y = (short)y,
                            Z = (short)(z + originZ),
                            Shape = Block.SHAPE.Box,
                            Direction = Block.DIRECTION.East,
                            Style = DefaultBlockStyle
                        };
                    }
                }
            }

            return new Map
            {
                BlockData = blocks
            };
        }
    }
}
