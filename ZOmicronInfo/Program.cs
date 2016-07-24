using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ZOmicronInfo
{
    class Program
    {
        public static string ExePath = Assembly.GetExecutingAssembly().Location;
        public static string ExeDir = Path.GetDirectoryName(ExePath);
        public static string RawPokePath = Path.Combine(ExeDir, "pokemon.raw");
        public static string JsonPokePath = Path.Combine(ExeDir, "pokemon.json");

        public static string[] RawPokeData { get; private set; }
        public static JObject JsonPokeData { get; private set; }
        public static PokemonContainer ContainedPokeData { get; private set; }

        public static string req { get; private set; }
        public static Pokemon reqp { get; private set; }

        public static bool Piping { get; private set; }

        public static bool DisableColour { get; private set; }

        static void Main(string[] args)
        {
            if (args.ElementAtOrDefault(0) != null)
            {
                req = args[0].Trim();
            }

            if (args.Contains("-p"))
            {
                Piping = true;
            }

            DisableColour = args.Contains("-nc");

            Console.Title = "ZOmicron Info by Zoryn4163";

            Log.StartWriting();

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
                Log.AsyncLine("ERROR: Raw/Json data file not found.\nEnsure the file 'pokemon.raw' or 'pokemon.json' exists next to EXE.", ConsoleColor.Red);

                if (!Piping)
                    Console.ReadKey();
                Environment.Exit(-1);
            }

            ContainedPokeData = JsonPokeData.ToObject<PokemonContainer>();

            Log.AsyncLine($"{ContainedPokeData.AllPokemon.Count} Pokemon Loaded", ConsoleColor.Green);
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
                    Log.AsyncLine($"Type: {reqp.TypeString} - Base Stats: {reqp.BaseStats.ToSingular()} - Total: {reqp.BaseStats.Sum()}", ConsoleColor.DarkYellow);
                    Log.AsyncLine($"Evolutions: {reqp.Evolutions.ToSingular()}", ConsoleColor.Yellow);
                    Log.AsyncLine($"Abilities: {reqp.Abilities.ToSingular()} - Hidden: {reqp.HiddenAbility}", ConsoleColor.Green);
                    Log.AsyncLine($"Moves: {reqp.Moves.ToSingular()}", ConsoleColor.Cyan);
                    Log.AsyncLine($"Egg Moves: {reqp.EggMoves.ToSingular()}", ConsoleColor.DarkCyan);

                    if (all)
                    {
                        Log.AsyncLine($"Base EXP: {reqp.BaseExp} - EV Gain: {reqp.EffortPoints.ToSingular()} - EXP Rate: {reqp.GrowthRate}", ConsoleColor.Magenta);
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
