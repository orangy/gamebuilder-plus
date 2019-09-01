using Algorithms.Generators;
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
            float version = 1.0f;

            DrawTitle(version);
            DrawMenu();

            return;
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
            // DRAW MENU
            TypewriterText("Create your own Game Builder world with this tool.");
            TypewriterText("What kind of map would you like to create?", autoPauseAtEnd: 0);
            TypewriterText("------------------------------------------", autoPauseAtEnd: 0);
            TypewriterText("1. Default (boring)", autoPauseAtEnd: 0);
            TypewriterText("2. Steep cliffs, lots of water", autoPauseAtEnd: 0);
            TypewriterText("3. One big hill with lake", autoPauseAtEnd: 0);
            TypewriterText("4. Oval lake with oval mountain", autoPauseAtEnd: 0);
            TypewriterText("5. Many hills", autoPauseAtEnd: 0);
            TypewriterText("6. Exit", 2, autoPauseAtEnd: 0);
            TypewriterText("> ", newlines: 0, autoPauseAtEnd: 0);

            ConsoleKeyInfo key = Console.ReadKey();
            string line = string.Empty;
            if (key.Key == ConsoleKey.D5)
                Environment.Exit(0);
            TypewriterText("", 2, autoPauseAtEnd: 0);
            TypewriterText("(To chose any default values, simply hit 'Enter')");

            // GET MAP GEN OPTIONS
            int itemp;
            float ftemp;

            int width = 100;
            int length = 100;
            string mapName = $"CustomMap-{DateTime.Now.ToString("MM_dd_yyyy hh_mm tt")}";
            string mapDesc = "Build with love by Romans 8:28";
            string outputDirectory = @"D:\Program Files (x86)\Steam\steamapps\common\Game Builder\GameBuilderUserData\Games"; //Directory.GetCurrentDirectory();

            try
            {
                TypewriterText("How wide would you like your map to be (100 is default (worlds crash when this is less than 30))? > ", newlines: 0, autoPauseAtEnd: 0);
                line = Console.ReadLine();
                if (int.TryParse(line, out itemp))
                    width = itemp;

                TypewriterText("How long would you like your map to be (100 is default (worlds crash when this is less than 30))? > ", newlines: 0, autoPauseAtEnd: 0);
                line = Console.ReadLine();
                if (int.TryParse(line, out itemp))
                    length = itemp;

                TypewriterText($"Would you like to give your map a name ('{mapName}' is default)? > ", newlines: 0, autoPauseAtEnd: 0);
                line = Console.ReadLine();
                if (!string.IsNullOrEmpty(line))
                    mapName = line;

                TypewriterText($"Would you like to give your map a description ('{mapDesc}' is default)? > ", newlines: 0, autoPauseAtEnd: 0);
                line = Console.ReadLine();
                if (!string.IsNullOrEmpty(line))
                    mapDesc = line;

                TypewriterText($"Where would you like us to save your map ('{outputDirectory}' is default)? > ", newlines: 0, autoPauseAtEnd: 0);
                line = Console.ReadLine();
                if (!string.IsNullOrEmpty(line))
                    outputDirectory = line;

                float[] opts = new float[2];
                opts[0] = key.KeyChar;
                if (key.Key == ConsoleKey.D2)
                {
                    TypewriterText("What would you like your multiplier to be (2.0 is default)? > ", newlines: 0, autoPauseAtEnd: 0);
                    line = Console.ReadLine();

                    if (float.TryParse(line, out ftemp))
                        opts[1] = ftemp;
                }
                else if (key.Key == ConsoleKey.D5)
                {
                    TypewriterText("What would you like your multiplier to be (2.0 is default)? > ", newlines: 0, autoPauseAtEnd: 0);
                    line = Console.ReadLine();

                    if (float.TryParse(line, out ftemp))
                        opts[1] = ftemp;
                }

                TypewriterText("", 2);


                // CREATE MAP
                Map myMap = new Map();
                VoosGenerator voosGenerator = new VoosGenerator();
                BaseGenerator generator = new DefaultGenerator(width, length);
                myMap = generator.Generate(opts);
                DefaultNaturalizer naturalizer = new DefaultNaturalizer();
                myMap = naturalizer.Naturalize(myMap);
                voosGenerator.Generate(myMap, outputDirectory, mapName, mapDesc);
            }
            catch (Exception ex)
            {
                TypewriterText("Oops! An error occurred.", autoPauseAtEnd: 0);
                TypewriterText("Please send an entire copy of your console window to Romans 8:28 on the discord server to debug and fix this.", newlines: 2, autoPauseAtEnd: 0);
                TypewriterText($"{ex.Message}", autoPauseAtEnd: 0);
                TypewriterText($"{ex.StackTrace}", autoPauseAtEnd: 0);

                Console.ReadKey();
                Environment.Exit(0);
            }

            TypewriterText("Your map was successfully created!");
            TypewriterText("Have a nice day!");
            Console.ReadKey();
            Environment.Exit(0);
        }

        private static void TypewriterText(string text, int? newlines = 1, int? autoPauseAtEnd = 1200)
        {
            for (int i = 0; i < text.Length; i++)
            {
                Console.Write(text[i]);
                Thread.Sleep(10);
            }

            if (newlines.HasValue)
                for (var i = 0; i < newlines; i++)
                    Console.Write(Environment.NewLine);

            if (autoPauseAtEnd.HasValue)
                Thread.Sleep(autoPauseAtEnd.Value);
        }
    }
}