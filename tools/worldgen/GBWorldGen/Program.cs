using System;
using GBWorldGen.Algorithms;
using GBWorldGen.Models;
using GBWorldGen.Utils;

namespace GBWorldGen.Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            VoosGenerator voosGenerator = new VoosGenerator();
            DiamondSquare diamondSquareAlgorithm = new DiamondSquare(0, 0, 0, 2);
            string outputDirectory = @"D:\Program Files (x86)\Steam\steamapps\common\Game Builder\GameBuilderUserData\Games";

            Block[] myMap = new Block[4]
            {
                new Block(0, 0, 0, Block.SHAPE.Box, Block.DIRECTION.East, Block.STYLE.Blue),
                new Block(1, 0, 0, Block.SHAPE.Box, Block.DIRECTION.East, Block.STYLE.Blue),
                new Block(2, 0, 0, Block.SHAPE.Box, Block.DIRECTION.East, Block.STYLE.Blue),
                new Block(3, 0, 0, Block.SHAPE.Box, Block.DIRECTION.East, Block.STYLE.Blue)
            };

            //string createdMap = voosGenerator.Generate(
            //    Serializer.SerializeMap(myMap), outputDirectory);
            string createdMap = voosGenerator.Generate(
                encodedMapData: Serializer.SerializeMap(diamondSquareAlgorithm.Generate()), outputDirectory: outputDirectory);


            Console.WriteLine($"Created new .voos file at '{createdMap}'.");
            Console.WriteLine("Press any key to continue...");
        }
    } 
}