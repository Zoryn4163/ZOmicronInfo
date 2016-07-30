using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ZOmicronInfo
{
    class Program
    {
        private const UInt32 StdOutputHandle = 0xFFFFFFF5;
        [DllImport("kernel32")]
        public static extern bool AllocConsole();
        [DllImport("kernel32")]
        public static extern bool FreeConsole();
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(UInt32 nStdHandle);
        [DllImport("kernel32.dll")]
        private static extern void SetStdHandle(UInt32 nStdHandle, IntPtr handle);

        public static string ExePath = Assembly.GetExecutingAssembly().Location;
        public static string ExeDir = Path.GetDirectoryName(ExePath);
        public static string RawPokePath = Path.Combine(ExeDir, "pokemon.raw");
        public static string JsonPokePath = Path.Combine(ExeDir, "pokemon.json");
        public static string GraphicsPath = Path.Combine(ExeDir, "Graphics", "Battlers");
        public static string[] GraphicsExtensions = "png|jpg|gif".Split('|');
        public static int LastId = 0;

        public static string[] RawPokeData { get; private set; }
        public static JObject JsonPokeData { get; private set; }
        public static PokemonContainer ContainedPokeData { get; private set; }

        public static string req { get; private set; }
        public static Pokemon reqp { get; private set; }

        public static bool Piping { get; private set; }

        public static bool DisableColour { get; private set; }

        public static Thread ConsoleThread { get; internal set; }
        public static WindowMain MainForm { get; private set; }
        public static bool UsingGui { get; internal set; }

        public static string[] Args { get; private set; }

        public const string Version = "1.4";

        [STAThread]
        internal static void Main(string[] args)
        {
            Args = args;
            
            if (args.Length > 0)
            {
                UsingGui = false;
                UseConsole();
                return;
            }

            UsingGui = true;
            LoadPokeData();
            ConsoleThread = new Thread(UseConsole);

            Application.EnableVisualStyles();
            MainForm = new WindowMain();
            MainForm.Title = "ZOmicron Info by Zoryn4163 - Version " + Version;
            MainForm.ShowDialog();
        }

        public static void LoadPokeData()
        {
            if (File.Exists(RawPokePath))
            {
                RawPokeData = File.ReadAllLines(RawPokePath);

                Log.AsyncLine("Raw Pokemon File Found\nConverting All Raw Data To Json", ConsoleColor.Yellow);
                JsonPokeData = JObject.Parse(Pokemon.SerializeRaw());
                File.WriteAllText(JsonPokePath, JsonPokeData.ToString());
                Log.AsyncLine("Conversion Complete", ConsoleColor.Green);
                Log.AsyncLine("NOTICE: In the user environment, it is recommended to delete 'pokemon.raw' for improved performance.", ConsoleColor.Red);
            }
            else if (File.Exists(JsonPokePath))
            {
                Log.AsyncLine("Json Pokemon File Found", ConsoleColor.Yellow);
                JsonPokeData = JObject.Parse(File.ReadAllText(JsonPokePath));
                Log.AsyncLine("Loading Complete", ConsoleColor.Green);
            }
            else
            {
                string err = "ERROR: Raw/Json data file not found.\nEnsure the file 'pokemon.raw' or 'pokemon.json' exists next to EXE.";

                if (UsingGui)
                {
                    MessageBox.Show(err, "ERROR");
                    Environment.Exit(-1);
                }

                Log.AsyncLine(err, ConsoleColor.Red);

                if (!Piping)
                    Console.ReadKey();
                Environment.Exit(-1);
            }

            ContainedPokeData = JsonPokeData.ToObject<PokemonContainer>();

            Log.AsyncLine($"{ContainedPokeData.AllPokemon.Count} Pokemon Loaded", ConsoleColor.Green);
            
        }

        public static void UseConsole()
        {
            Log.StartWriting();

            if (Args.ElementAtOrDefault(0) != null)
            {
                req = Args[0].Trim();
            }

            if (Args.Contains("-p"))
            {
                Piping = true;
            }

            DisableColour = Args.Contains("-nc");

            if (!Piping)
            {
                //AllocConsole();

                //Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) {AutoFlush = true});
                //Console.SetIn(new StreamReader(Console.OpenStandardInput()));

                Console.Title = "ZOmicron Info by Zoryn4163 V" + Version;
            }

            LoadPokeData();

            Log.AsyncLine("Append any query with '-a' to get ALL information on the pokemon.");
            Log.AsyncLine("Append any query with '-j' to get a JSON response on the pokemon.");
            Log.AsyncLine("No arguments will return common data on the pokemon.");

            while (true)
            {
                if (!Piping)
                {
                    Log.Async("\nEnter ID or InternalName of pokemon: ", ConsoleColor.Cyan);

                    while (Log.Messages.Any())
                    {
                        Thread.Sleep(100);
                    }

                    req = Console.ReadLine()?.Trim() ?? string.Empty;
                }

                bool all = false, json = false;

                if (req.Contains("-a"))
                {
                    all = true;
                    req = req.Replace("-a", "");
                }

                if (req.Contains("-j"))
                {
                    json = true;
                    req = req.Replace("-j", "");
                }

                if (req == "q" || req == "x" || req == "quit" || req == "exit")
                    Environment.Exit(0);

                if (req == "cls")
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, 0);
                    continue;
                }

                req = req.Trim();

                /*if (req == "gui")
                {
                    MainForm.EnableFromConsole();
                    while (UsingGui)
                    {
                    }
                    continue;
                }*/

                if (req.AsInt() > 0)
                {
                    reqp = Pokemon.FromContainedIndex(req.AsInt());
                }
                else
                {
                    reqp = Pokemon.FromContainedInternalName(req.ToUpperInvariant());
                }

                if (reqp != null)
                {
                    if (Piping)
                    {
                        Console.WriteLine(reqp.Serialize());

                        Environment.Exit(0);
                    }

                    if (json)
                    {
                        Log.AsyncLine(reqp.Serialize());
                        continue;
                    }

                    Log.AsyncLine($"Display Name: {reqp.Name} - Internal Name: {reqp.InternalName} - ID: {reqp.Id}", ConsoleColor.Red);
                    Log.AsyncLine($"Type: {reqp.TypeString} - Base Stats: {reqp.BaseStats.NiceString} - Total: {reqp.BaseStats.Total}", ConsoleColor.DarkYellow);
                    Log.AsyncLine($"Evolutions: {reqp.Evolutions.ToSingular()}", ConsoleColor.Yellow);
                    Log.AsyncLine($"Abilities: {reqp.Abilities.ToSingular()} - Hidden: {reqp.HiddenAbility}", ConsoleColor.Green);
                    Log.AsyncLine($"Moves: {reqp.Moves.ToSingular()}", ConsoleColor.Cyan);
                    Log.AsyncLine($"Egg Moves: {reqp.EggMoves.ToSingular()}", ConsoleColor.DarkCyan);

                    if (all)
                    {
                        Log.AsyncLine($"Base EXP: {reqp.BaseExp} - EV Gain: {reqp.EffortPoints.NiceString} - EXP Rate: {reqp.GrowthRate}", ConsoleColor.Magenta);
                        Log.AsyncLine($"Gender Rate: {reqp.GenderRate} - Rareness: {reqp.Rareness} - Happiness: {reqp.Happiness}", ConsoleColor.Red);
                        Log.AsyncLine($"Egg Steps: {reqp.StepsToHatch} - Egg Groups: {reqp.Compatability.ToSingular()} - Regional IDs: {reqp.RegionalNumbers.ToSingular()}", ConsoleColor.Yellow);
                        Log.AsyncLine($"Weight: {reqp.Weight} - Height: {reqp.Height} - Habitat: {reqp.Habitat} - Kind: {reqp.Kind}", ConsoleColor.Green);
                        Log.AsyncLine($"Pokedex Entry: {reqp.Pokedex}", ConsoleColor.Cyan);
                    }
                }

                if (Piping)
                {
                    //failsafe
                    break;
                }
            }
        }
    }
}
