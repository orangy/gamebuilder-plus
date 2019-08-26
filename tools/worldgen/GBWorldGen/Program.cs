using Algorithms.Generators;
using Algorithms.Naturalize;
using GBWorldGen.Core.Algorithms;
using GBWorldGen.Core.Models;
using GBWorldGen.Core.Voos;
using System;

namespace GBWorldGen.Driver.Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            VoosGenerator voosGenerator = new VoosGenerator();
            DiamondSquareGenerator diamondSquareGenerator = new DiamondSquareGenerator(0, 0, 0, 8, defaultBlockStyle: Block.STYLE.GrayCraters);
            PerlinNoiseGenerator perlinNoiseGenerator = new PerlinNoiseGenerator(0, 0, 0, 20, 20);
            DefaultNaturalizer naturalizer = new DefaultNaturalizer();
            string outputDirectory = @"D:\Program Files (x86)\Steam\steamapps\common\Game Builder\GameBuilderUserData\Games";

            //Block[] myMap = diamondSquareGenerator.Generate();
            Block[] myMap = perlinNoiseGenerator.Generate();
            myMap = naturalizer.Naturalize(myMap);

            string createdMap = voosGenerator.Generate(myMap, outputDirectory);

            Console.WriteLine($"Created new .voos file at '{createdMap}'.");
            Console.WriteLine("Press any key to continue...");
        }
    }
}