using GBWorldGen.Core.Algorithms.Generators;
using GBWorldGen.Core.Algorithms.Naturalize;
using GBWorldGen.Core.Models;
using GBWorldGen.Core.Voos;
using System;
using System.Threading;

namespace GBWorldGen.Driver.Main
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //float version = 1.0f;

            //DrawTitle(version);
            //DrawMenu();

            //return;
            VoosGenerator voosGenerator = new VoosGenerator();
            PerlinNoiseGenerator perlinNoiseGenerator = new PerlinNoiseGenerator(50, 50);
            DiamondSquareGenerator diamondSquareGenerator = new DiamondSquareGenerator(8, defaultBlockStyle: Block.STYLE.GrayCraters);
            DefaultNaturalizer naturalizer = new DefaultNaturalizer();
            string outputDirectory = @"D:\Program Files (x86)\Steam\steamapps\common\Game Builder\GameBuilderUserData\Games";

            //Map myMap = diamondSquareGenerator.Generate();
            Map myMap = perlinNoiseGenerator.Generate();
            myMap = naturalizer.Naturalize(myMap);

            string createdMap = voosGenerator.Generate(myMap, outputDirectory);

            Console.WriteLine($"Created new .voos file at '{createdMap}'.");
            Console.WriteLine("Press any key to continue...");
        }

        private static void DrawTitle(float version)
        {
            Console.Title = $"Game Builder world gen (by Romans 8:28) v{version.ToString("0.0")}";

            Console.WriteLine(@"  ________                        __________      .__.__       .___                              ");
            Console.WriteLine(@" /  _____/_____    _____   ____   \______   \__ __|__|  |    __| _/___________                   ");
            Console.WriteLine(@"/   \  ___\__  \  /     \_/ __ \   |    |  _/  |  \  |  |   / __ |/ __ \_  __ \                  ");
            Console.WriteLine(@"\    \_\  \/ __ \|  Y Y  \  ___/   |    |   \  |  /  |  |__/ /_/ \  ___/|  | \/                  ");
            Console.WriteLine(@" \______  (____  /__|_|  /\___  >  |______  /____/|__|____/\____ |\___  >__|                     ");
            Console.WriteLine(@"        \/     \/      \/     \/          \/                    \/    \/                         ");
            Console.WriteLine(@"                    .__       .___                                                               ");
            Console.WriteLine(@"__  _  _____________|  |    __| _/    ____   ____   ____                                         ");
            Console.WriteLine(@"\ \/ \/ /  _ \_  __ \  |   / __ |    / ___\_/ __ \ /    \                                        ");
            Console.WriteLine(@" \     (  <_> )  | \/  |__/ /_/ |   / /_/  >  ___/|   |  \                                       ");
            Console.WriteLine(@"  \/\_/ \____/|__|  |____/\____ |   \___  / \___  >___|  /                                       ");
            Console.WriteLine(@"                               \/  /_____/      \/     \/                                        ");
            Console.WriteLine(@"    ___ __________                                        ______      ________   ______   ___    ");
            Console.WriteLine(@"   /  / \______   \ ____   _____ _____    ____   ______  /  __  \  /\ \_____  \ /  __  \  \  \   ");
            Console.WriteLine(@"  /  /   |       _//  _ \ /     \\__  \  /    \ /  ___/  >      <  \/  /  ____/ >      <   \  \  ");
            Console.WriteLine(@" (  (    |    |   (  <_> )  Y Y  \/ __ \|   |  \\___ \  /   --   \ /\ /       \/   --   \   )  ) ");
            Console.WriteLine(@"  \  \   |____|_  /\____/|__|_|  (____  /___|  /____  > \______  / \/ \_______ \______  /  /  /  ");
            Console.WriteLine(@"   \__\         \/             \/     \/     \/     \/         \/             \/      \/  /__/   ");
            Console.WriteLine(@"-------------------------------------------------------------------------------------------------");
            Console.WriteLine(@"");            
        }

        private static void DrawMenu()
        {
            TypewriterText("Create your own Game Builder world with this tool.");
            TypewriterText("An .options file can speed upBefore we begin, you can save an .options file to more easily create worlds. Would you like us to make an .options file for you?");

            Console.ReadKey();
            Environment.Exit(0);
        }

        private static void TypewriterText(string text, bool newline = true, int? autoPauseAtEnd = 1200)
        {
            for (int i = 0; i < text.Length; i++)
            {
                Console.Write(text[i]);
                Thread.Sleep(25);
            }

            if (newline)
                Console.Write(Environment.NewLine);

            if (autoPauseAtEnd.HasValue)
                Thread.Sleep(autoPauseAtEnd.Value);
        }
    }
}