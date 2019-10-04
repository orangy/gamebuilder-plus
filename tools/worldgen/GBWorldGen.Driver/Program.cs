using GBWorldGen.Core.Algorithms.Generators;
using GBWorldGen.Core.Algorithms.Generators.Abstractions;
using GBWorldGen.Core.Models;
using GBWorldGen.Core.Voos;
using GBWorldGen.Main;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;

namespace GBWorldGen.Driver.Main
{
    public class Program
    {
        private static bool Fast { get; set; }
        private static bool Romans828 { get; set; }

        public static void Main(string[] args)
        {
            if (args != null && args.Length > 0)
            {
                if (Array.Exists(args, a => a.Contains("fast", StringComparison.OrdinalIgnoreCase))) Fast = true;
                if (Array.Exists(args, a => a.Contains("romans828", StringComparison.OrdinalIgnoreCase))) Romans828 = true;
            }

            float version = 2.3f;

            DrawTitle(version);
            DrawMenu();
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
            TypewriterText("Create your own Game Builder world with this tool!");
            TypewriterText("What kind of map would you like to create?", autoPauseAtEnd: 0);
            TypewriterText("------------------------------------------", autoPauseAtEnd: 0);
            TypewriterText("1. Default", autoPauseAtEnd: 0);
            TypewriterText("2. Custom", autoPauseAtEnd: 0);
            TypewriterText("3. Exit", autoPauseAtEnd: 0);
            TypewriterText("> ", newlines: 0, autoPauseAtEnd: 0);

            string line = string.Empty;
            string choice = Console.ReadLine();
            if (line.Contains("3", StringComparison.OrdinalIgnoreCase))
                Environment.Exit(0);
            TypewriterText("", 2, autoPauseAtEnd: 0);
            TypewriterText("(To choose any default values, simply hit 'Enter')", autoPauseAtEnd: 0);

            // GET MAP GEN OPTIONS
            int itemp;
            float ftemp;
            double dtemp;

            try
            {
                ProgramOptions programOptions = new ProgramOptions();
                string programOptionsString = string.Empty;
                string programOptionsFilepath = Path.Combine(Directory.GetCurrentDirectory(), ProgramOptions.FILENAME);
                if (File.Exists(programOptionsFilepath))
                {
                    using (StreamReader streamReader = new StreamReader(Path.Combine(Directory.GetCurrentDirectory(), ProgramOptions.FILENAME)))
                    {
                        programOptionsString = streamReader.ReadToEnd();
                    }
                    programOptions = JsonConvert.DeserializeObject<ProgramOptions>(programOptionsString);
                }
                

                int width = (programOptions != null && programOptions.Width != default(int)) ? programOptions.Width : 250; // in .voos units
                int length = (programOptions != null && programOptions.Length != default(int)) ? programOptions.Length : 250;
                MapGeneratorOptions mapOptions = new MapGeneratorOptions();
                string mapName = $"CustomMap-{DateTime.Now.ToString("MM_dd_yyyy hh_mm tt")}";
                string mapDesc = string.Empty;
                string outputDirectory = Romans828 ? @"D:\Program Files (x86)\Steam\steamapps\common\Game Builder\GameBuilderUserData\Games" : (programOptions != null && !string.IsNullOrEmpty(programOptions.SaveTo)) ? programOptions.SaveTo : Directory.GetCurrentDirectory();

            
                TypewriterText($"How wide would you like your map to be [{width}=DEFAULT])? > ", newlines: 0, autoPauseAtEnd: 0);
                line = Console.ReadLine();
                if (int.TryParse(line, out itemp))
                    width = itemp;

                TypewriterText($"How deep would you like your map to be [{length}=DEFAULT])? > ", newlines: 0, autoPauseAtEnd: 0);
                line = Console.ReadLine();
                if (int.TryParse(line, out itemp))
                    length = itemp;

                TypewriterText($"Would you like to give your map a name ['{mapName}'=DEFAULT])? > ", newlines: 0, autoPauseAtEnd: 0);
                line = Console.ReadLine();
                if (!string.IsNullOrEmpty(line))
                    mapName = line;

                TypewriterText($"Would you like to give your map a description? > ", newlines: 0, autoPauseAtEnd: 0);
                line = Console.ReadLine();
                if (!string.IsNullOrEmpty(line))
                    mapDesc = line;

                TypewriterText($"Where would you like us to save your map ['{outputDirectory}'=DEFAULT]? > ", newlines: 0, autoPauseAtEnd: 0);
                line = Console.ReadLine();
                if (!string.IsNullOrEmpty(line))
                    outputDirectory = line;                

                string[] biomeNames = mapOptions.BiomeNames();
                for (int i = 0; i < biomeNames.Length; i++)
                {
                    biomeNames[i] = $"'{biomeNames[i][0]}'{biomeNames[i].Substring(1)}={Enum.Parse(mapOptions.Biome.GetType(), biomeNames[i])}";
                }
                TypewriterText($"What would you like your biome to be [{mapOptions.EnumName(mapOptions.Biome)}=DEFAULT ({string.Join(',', biomeNames)})]? > ", newlines: 0, autoPauseAtEnd: 0);
                line = Console.ReadLine();

                if (!string.IsNullOrEmpty(line))
                {
                    if (line.Contains("g", StringComparison.OrdinalIgnoreCase)) mapOptions.Biome = MapGeneratorOptions.MapBiome.Grassland;
                    else if (line.Contains("d", StringComparison.OrdinalIgnoreCase)) mapOptions.Biome = MapGeneratorOptions.MapBiome.Desert;
                    else if (line.Contains("t", StringComparison.OrdinalIgnoreCase)) mapOptions.Biome = MapGeneratorOptions.MapBiome.Tundra;
                }

                if (choice.Contains("2", StringComparison.OrdinalIgnoreCase))
                {                    
                    TypewriterText($"What would you like your plain frequency to be [{mapOptions.PlainFrequency}=DEFAULT]? > ", newlines: 0, autoPauseAtEnd: 0);
                    line = Console.ReadLine();

                    if (!string.IsNullOrEmpty(line))
                        if (float.TryParse(line, out ftemp))
                            mapOptions.PlainFrequency = ftemp;

                    // Trees (actors)
                    TypewriterText($"Would you like trees in your map [{mapOptions.Actors}=DEFAULT]? > ", newlines: 0, autoPauseAtEnd: 0);
                    line = Console.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        if (line.Contains("1") || line.Contains("t", StringComparison.OrdinalIgnoreCase) || line.Contains("y", StringComparison.OrdinalIgnoreCase))
                            mapOptions.Actors = true;
                        else mapOptions.Actors = false;
                    }

                    // Lakes
                    TypewriterText($"Would you like lakes [{mapOptions.Lakes}=DEFAULT]? > ", newlines: 0, autoPauseAtEnd: 0);
                    line = Console.ReadLine();

                    if (!string.IsNullOrEmpty(line))
                    {
                        if (line.Contains("1") || line.Contains("t", StringComparison.OrdinalIgnoreCase) || line.Contains("y", StringComparison.OrdinalIgnoreCase))
                            mapOptions.Lakes = true;
                        else mapOptions.Lakes = false;
                    }

                    if (mapOptions.Lakes)
                    {
                        TypewriterText($"What would you like your lake frequency to be [{mapOptions.LakeFrequency}=DEFAULT]? > ", newlines: 0, autoPauseAtEnd: 0);
                        line = Console.ReadLine();

                        if (!string.IsNullOrEmpty(line))
                            if (float.TryParse(line, out ftemp))
                                mapOptions.LakeFrequency = ftemp;

                        TypewriterText($"What would you like your lake size to be [{mapOptions.LakeSize}=DEFAULT]? > ", newlines: 0, autoPauseAtEnd: 0);
                        line = Console.ReadLine();

                        if (!string.IsNullOrEmpty(line))
                            if (int.TryParse(line, out itemp))
                                mapOptions.LakeSize = itemp;
                    }

                    // Hills
                    TypewriterText($"Would you like hills [{mapOptions.Hills}=DEFAULT]? > ", newlines: 0, autoPauseAtEnd: 0);
                    line = Console.ReadLine();

                    if (!string.IsNullOrEmpty(line))
                    {
                        if (line.Contains("1") || line.Contains("t", StringComparison.OrdinalIgnoreCase) || line.Contains("y", StringComparison.OrdinalIgnoreCase))
                            mapOptions.Hills = true;
                        else mapOptions.Hills = false;
                    }

                    if (mapOptions.Hills)
                    {
                        TypewriterText($"What would you like your hill frequency to be [{mapOptions.HillFrequency}=DEFAULT]? > ", newlines: 0, autoPauseAtEnd: 0);
                        line = Console.ReadLine();

                        if (!string.IsNullOrEmpty(line))
                            if (float.TryParse(line, out ftemp))
                                mapOptions.HillFrequency = ftemp;

                        TypewriterText($"What would you like your hill clamp to be [{mapOptions.HillClamp}=DEFAULT]? > ", newlines: 0, autoPauseAtEnd: 0);
                        line = Console.ReadLine();

                        if (!string.IsNullOrEmpty(line))
                            if (int.TryParse(line, out itemp))
                                mapOptions.HillClamp = itemp;
                    }

                    // Mountains
                    TypewriterText($"Would you like mountains [{mapOptions.Mountains}=DEFAULT]? > ", newlines: 0, autoPauseAtEnd: 0);
                    line = Console.ReadLine();

                    if (!string.IsNullOrEmpty(line))
                    {
                        if (line.Contains("1") || line.Contains("t", StringComparison.OrdinalIgnoreCase) || line.Contains("y", StringComparison.OrdinalIgnoreCase))
                            mapOptions.Mountains = true;
                        else mapOptions.Mountains = false;
                    }

                    if (mapOptions.Mountains)
                    {
                        TypewriterText($"What would you like your mountain frequency to be [{mapOptions.MountainFrequency}=DEFAULT]? > ", newlines: 0, autoPauseAtEnd: 0);
                        line = Console.ReadLine();

                        if (!string.IsNullOrEmpty(line))
                            if (float.TryParse(line, out ftemp))
                                mapOptions.MountainFrequency = ftemp;

                        TypewriterText($"What would you like your additional mountain size to be [{mapOptions.AdditionalMountainSize}=DEFAULT]? > ", newlines: 0, autoPauseAtEnd: 0);
                        line = Console.ReadLine();

                        if (!string.IsNullOrEmpty(line))
                            if (double.TryParse(line, out dtemp))
                                mapOptions.AdditionalMountainSize = dtemp;
                    }

                    // Caves
                    TypewriterText($"Would you like caves [{mapOptions.Caves}=DEFAULT]? > ", newlines: 0, autoPauseAtEnd: 0);
                    line = Console.ReadLine();

                    if (!string.IsNullOrEmpty(line))
                    {
                        if (line.Contains("1") || line.Contains("t", StringComparison.OrdinalIgnoreCase) || line.Contains("y", StringComparison.OrdinalIgnoreCase))
                            mapOptions.Caves = true;
                        else mapOptions.Caves = false;
                    }

                    if (mapOptions.Caves)
                    {
                        TypewriterText($"What would you like your additional cave height to be [{mapOptions.AdditionalCaveHeight}=DEFAULT]? > ", newlines: 0, autoPauseAtEnd: 0);
                        line = Console.ReadLine();

                        if (!string.IsNullOrEmpty(line))
                            if (int.TryParse(line, out itemp))
                                mapOptions.AdditionalCaveHeight = itemp;
                    }

                    // Tunnels
                    TypewriterText($"Would you like tunnels [{mapOptions.Tunnels}=DEFAULT]? > ", newlines: 0, autoPauseAtEnd: 0);
                    line = Console.ReadLine();

                    if (!string.IsNullOrEmpty(line))
                    {
                        if (line.Contains("1") || line.Contains("t", StringComparison.OrdinalIgnoreCase) || line.Contains("y", StringComparison.OrdinalIgnoreCase))
                            mapOptions.Tunnels = true;
                        else mapOptions.Tunnels = false;
                    }

                    if (mapOptions.Tunnels)
                    {
                        TypewriterText($"What is the maximum number of tunnels that you'd like [{mapOptions.TunnelWormsMax}=DEFAULT]? > ", newlines: 0, autoPauseAtEnd: 0);
                        line = Console.ReadLine();

                        if (!string.IsNullOrEmpty(line))
                            if (int.TryParse(line, out itemp))
                                mapOptions.TunnelWormsMax = itemp;

                        TypewriterText($"What radius would you like your tunnels [{mapOptions.TunnelRadius}=DEFAULT]? > ", newlines: 0, autoPauseAtEnd: 0);
                        line = Console.ReadLine();

                        if (!string.IsNullOrEmpty(line))
                            if (int.TryParse(line, out itemp))
                                mapOptions.TunnelRadius = itemp;

                        TypewriterText($"How long would you like your tunnels [{mapOptions.TunnelLength}=DEFAULT]? > ", newlines: 0, autoPauseAtEnd: 0);
                        line = Console.ReadLine();

                        if (!string.IsNullOrEmpty(line))
                            if (int.TryParse(line, out itemp))
                                mapOptions.TunnelLength = itemp;
                    }
                }

                TypewriterText("", 2);

                if (string.IsNullOrEmpty(mapDesc)) mapDesc = $"Map specs => ({width}x{length}) {mapOptions.ToString()} -Built with love by Romans 8:28.";

                // CREATE MAP
                // convert back to proper unit
                width = (int)Math.Floor(width / 2.5d);
                length = (int)Math.Floor(length / 2.5d);
                BaseGenerator<short> generator = new MapGenerator((short)width, (short)length, 1, mapOptions);
                Map map = (Map)generator.GenerateMap();
                VoosGenerator voosGenerator = new VoosGenerator();
                voosGenerator.Generate(map, outputDirectory, mapName, mapDesc);

                TypewriterText("");
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
            TypewriterText("");
            TypewriterText("Press any key to exit...");
            Console.ReadKey();
            Environment.Exit(0);
        }

        private static void TypewriterText(string text, int? newlines = 1, int? autoPauseAtEnd = 1200)
        {
            if (!Fast)
                for (int i = 0; i < text.Length; i++)
                {
                    Console.Write(text[i]);
                    Thread.Sleep(10);
                }
            else
                Console.Write(text);

            if (newlines.HasValue)
                for (var i = 0; i < newlines; i++)
                    Console.Write(Environment.NewLine);

            if (!Fast && autoPauseAtEnd.HasValue)
                Thread.Sleep(autoPauseAtEnd.Value);
        }
    }
}