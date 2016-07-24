using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace ZOmicronInfo
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string InternalName { get; set; }
        public PokeType Type1 { get; set; }
        public PokeType Type2 { get; set; }
        public string TypeString => Type1.ToString() + (Type2.ToString() == "" ? "" : "/" + Type2.ToString());
        public int[] BaseStats { get; set; }
        public String GenderRate { get; set; }
        public String GrowthRate { get; set; }
        public int BaseExp { get; set; }
        public int[] EffortPoints { get; set; }
        public int Rareness { get; set; }
        public int Happiness { get; set; }
        public string[] Abilities { get; set; }
        public string HiddenAbility { get; set; }
        public PokeMove[] Moves { get; set; }
        public EggMove[] EggMoves { get; set; }
        public int[] Compatability { get; set; }
        public int StepsToHatch { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public string Colour { get; set; }
        public string Habitat { get; set; }
        public int[] RegionalNumbers { get; set; }
        public string Kind { get; set; }
        public string Pokedex { get; set; }
        public PokeEvo[] Evolutions { get; set; }

        public static Pokemon FromRawIndex(int index, bool writeNoPoke = false)
        {
            if (Program.RawPokeData == null)
            {
                Log.AsyncLine("Raw Pokemon Data not set.");
                return null;
            }

            Pokemon ret = new Pokemon();

            int st = Array.IndexOf(Program.RawPokeData, $"[{index}]");

            if (st == -1)
            {
                if (writeNoPoke)
                    Log.AsyncLine("No Pokemon at index: " + index);
                return null;
            }

            int en = Array.IndexOf(Program.RawPokeData, $"[{index + 1}]", st + 1);
            if (en == -1)
                en = Program.RawPokeData.Length;

            //Log.AsyncLine((st + " - " + en);

            var t = Program.RawPokeData.SubArray(st, en - st);

            ret.Id = Convert.ToInt32(t[0].Replace("[", "").Replace("]", ""));
            ret.Name = t.PullKeyValue<string>("Name");
            ret.InternalName = t.PullKeyValue<string>("InternalName");
            ret.Type1 = t.PullKeyValue<PokeType>("Type1");
            ret.Type2 = t.PullKeyValue<PokeType>("Type2");
            ret.BaseStats = t.PullKeyValue<string>("BaseStats").Split(',').CastArrayInt();
            ret.GenderRate = t.PullKeyValue<string>("GenderRate");
            ret.GrowthRate = t.PullKeyValue<string>("GrowthRate");
            ret.BaseExp = t.PullKeyValue<string>("BaseEXP").AsInt();
            ret.EffortPoints = t.PullKeyValue<string>("EffortPoints").Split(',').CastArrayInt();
            ret.Rareness = t.PullKeyValue<string>("Rareness").AsInt();
            ret.Happiness = t.PullKeyValue<string>("Happiness").AsInt();
            ret.Abilities = t.PullKeyValue<string>("Abilities").Split(',');
            ret.HiddenAbility = t.PullKeyValue<string>("HiddenAbility");
            ret.Moves = t.PullKeyValue<PokeMove[]>("Moves");
            ret.EggMoves = t.PullKeyValue<EggMove[]>("EggMoves");
            ret.Compatability = t.PullKeyValue<string>("Compatibility").Split(',').CastArrayInt();
            ret.StepsToHatch = t.PullKeyValue<string>("StepsToHatch").AsInt();
            ret.Height = t.PullKeyValue<string>("Height").AsFloat();
            ret.Weight = t.PullKeyValue<string>("Weight").AsFloat();
            ret.Colour = t.PullKeyValue<string>("Color");
            ret.Habitat = t.PullKeyValue<string>("Habitat");
            ret.RegionalNumbers = t.PullKeyValue<string>("RegionalNumbers").Split(',').CastArrayInt();
            ret.Kind = t.PullKeyValue<string>("Kind");
            ret.Pokedex = t.PullKeyValue<string>("Pokedex");
            ret.Evolutions = t.PullKeyValue<PokeEvo[]>("Evolutions");

            foreach (PokeEvo pe in ret.Evolutions)
            {
                pe.CalcNextEvo();
            }

            //Log.AsyncLine((JsonConvert.SerializeObject(ret, Formatting.None) + "\n");

            return ret;
        }

        public static Pokemon FromInternalName(string intName)
        {
            int ni = Array.IndexOf(Program.RawPokeData, $"InternalName={intName}");

            if (ni >= 2)
            {
                int ind = Program.RawPokeData[ni - 2].Replace("[", "").Replace("]", "").AsInt();
                return FromRawIndex(ind);
            }

            Log.AsyncLine("No Pokemon with InternalName of: " + intName);
            return null;
        }

        public static Pokemon FromContainedIndex(int index)
        {
            if (Program.JsonPokeData == null)
            {
                Log.AsyncLine("JSON Pokemon Data not set.");
                return null;
            }

            if (index <= 0 || index > Program.ContainedPokeData.AllPokemon.Count)
            {
                Log.AsyncLine("Index out of bounds. Enter a number between 1 and " + Program.ContainedPokeData.AllPokemon.Count + ".)");
                return null;
            }

            return Program.ContainedPokeData.AllPokemon[index - 1];
        }

        public static Pokemon FromContainedInternalName(string intName)
        {
            if (Program.JsonPokeData == null)
            {
                Log.AsyncLine("JSON Pokemon Data not set.");
                return null;
            }

            if (Program.ContainedPokeData.AllPokemon.Any(x => x.InternalName == intName.ToUpperInvariant()))
            {
                return Program.ContainedPokeData.AllPokemon.FirstOrDefault(x => x.InternalName == intName.ToUpperInvariant());
            }

            Log.AsyncLine("No Pokemon with InternalName of: " + intName);
            return null;
        }

        public static string SerializeRaw()
        {
            PokemonContainer pokes = new PokemonContainer();
            Pokemon cur = null;

            for (int i = 0; i < Int32.MaxValue; i++)
            {
                cur = Pokemon.FromRawIndex(i + 1);
                if (cur == null)
                    break;
                pokes.AllPokemon.Add(cur);
            }

            return JsonConvert.SerializeObject(pokes, Formatting.Indented);
        }
    }

    public class PokemonContainer
    {
        public List<Pokemon> AllPokemon { get; set; }

        public PokemonContainer()
        {
            AllPokemon = new List<Pokemon>();
        }
    }
}
