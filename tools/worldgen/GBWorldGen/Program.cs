using System;
using GBWorldGen.Models;
using GBWorldGen.Utils;

namespace GBWorldGen.Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string outputDirectory = @"D:\temp\gb";

            Block[] myMap = new Block[4]
            {
                new Block(0, 0, 0, Block.SHAPE.Box, Block.DIRECTION.East, Block.STYLE.Blue),
                new Block(1, 0, 0, Block.SHAPE.Box, Block.DIRECTION.East, Block.STYLE.Blue),
                new Block(2, 0, 0, Block.SHAPE.Box, Block.DIRECTION.East, Block.STYLE.Blue),
                new Block(3, 0, 0, Block.SHAPE.Box, Block.DIRECTION.East, Block.STYLE.Blue)
            };
            
            VoosGenerator voosGenerator = new VoosGenerator();
            string createdMap = voosGenerator.Generate(Serializer.SerializeMap(myMap), outputDirectory);
            Console.WriteLine($"Created new .voos file at '{createdMap}'.");

            Console.WriteLine("Press any key to continue...");
        }
    } 
}