using GBWorldGen.Core.Algorithms.Generators;
using GBWorldGen.Core.Algorithms.Naturalize;
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
            PerlinNoiseGenerator perlinNoiseGenerator = new PerlinNoiseGenerator(0, 0, 0, 50, 50);
            DiamondSquareGenerator diamondSquareGenerator = new DiamondSquareGenerator(0, 0, 0, 8, defaultBlockStyle: Block.STYLE.GrayCraters);
            DefaultNaturalizer naturalizer = new DefaultNaturalizer();
            string outputDirectory = @"D:\Program Files (x86)\Steam\steamapps\common\Game Builder\GameBuilderUserData\Games";

            //Map myMap = diamondSquareGenerator.Generate();
            Map myMap = perlinNoiseGenerator.Generate();
            //myMap = naturalizer.Naturalize(myMap);

            string createdMap = voosGenerator.Generate(myMap, outputDirectory);

            Console.WriteLine($"Created new .voos file at '{createdMap}'.");
            Console.WriteLine("Press any key to continue...");
        }
    }
}