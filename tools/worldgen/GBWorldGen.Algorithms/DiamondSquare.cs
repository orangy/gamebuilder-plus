using System;
using GBWorldGen.Models;

namespace GBWorldGen.Algorithms
{
    public class DiamondSquare : IAlgorithm
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public DiamondSquare(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public Block[] Generate()
        {
            throw new NotImplementedException();
        }
    }
}
