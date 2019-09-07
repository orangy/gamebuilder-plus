using GBWorldGen.Core.Algorithms.Abstractions;
using GBWorldGen.Core.Models;
using System;
using System.Collections.Generic;

namespace GBWorldGen.Core.Algorithms.Generators
{
    public class Base2DGenerator : WorldData, IGenerateWorld
    {
        protected int Width { get; set; }
        protected int Length { get; set; }
        private Block.STYLE DefaultBlockStyle { get; }
        protected short[,] YValues { get; set; }
        //protected BaseGeneratorOptions Options { get; set; }

        public Base2DGenerator(int width, int length, /*BaseGeneratorOptions options = null,*/ Block.STYLE defaultBlockStyle = Block.STYLE.Grass)
        {
            Width = width;
            Length = length;
            DefaultBlockStyle = defaultBlockStyle;
            YValues = new short[Width, Length];

            //Options = options != null ? options : new BaseGeneratorOptions();
        }

        public virtual Map Generate()
        {
            Console.WriteLine("Generating map...");

            short originX = (short)(Width * -0.5d);
            short originZ = (short)(Length * -0.5d);            
            int minY = 999;
            int maxY = -999;
            for (int i = 0; i < YValues.GetLength(0); i++)
            {
                for (int j = 0; j < YValues.GetLength(1); j++)
                {
                    if (YValues[i, j] < minY) minY = YValues[i, j];
                    if (YValues[i, j] > maxY) maxY = YValues[i, j];
                }
            }
            Block[,,] blocks = new Block[Width, (maxY - minY + 1), Length];
            Dictionary<(int, int), int> indexCounter = new Dictionary<(int, int), int>();

            for (int x = 0; x < YValues.GetLength(0); x++)
            {
                for (int z = 0; z < YValues.GetLength(1); z++)
                {
                    if (!indexCounter.ContainsKey((x + originX, z + originZ))) indexCounter[(x + originX, z + originZ)] = 0;

                    blocks[x, indexCounter[(x + originX, z + originZ)], z] = new Block
                    {
                        X = (short)(x + originX),
                        Y = YValues[x, z],
                        Z = (short)(z + originZ),
                        Shape = Block.SHAPE.Box,
                        Direction = Block.DIRECTION.East,
                        Style = DefaultBlockStyle
                    };
                    indexCounter[(x + originX, z + originZ)]++;
                }
            }

            return null;
            //return new Map
            //{
            //    BlockData = blocks
            //};
        }
    }
}
