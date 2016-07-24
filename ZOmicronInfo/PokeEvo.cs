using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZOmicronInfo
{
    public class PokeEvo
    {
        public string EvoPoke { get; set; }
        public string EvoMethod { get; set; }
        public string EvoData { get; set; }
        public PokeEvo NextEvo { get; set; }

        public void CalcNextEvo()
        {
            Pokemon nxt = null;
            if (Program.RawPokeData != null)
                nxt = Pokemon.FromInternalName(EvoPoke);
            else if (Program.JsonPokeData != null)
                nxt = Pokemon.FromContainedInternalName(EvoPoke);

            if (nxt.Evolutions.ElementAtOrDefault(0) != null)
            {
                NextEvo = new PokeEvo();
                NextEvo.EvoPoke = nxt.Evolutions[0].EvoPoke;
                NextEvo.EvoMethod = nxt.Evolutions[0].EvoMethod;
                NextEvo.EvoData = nxt.Evolutions[0].EvoData;
            }
        }

        public override string ToString()
        {
            return $"{EvoPoke} {EvoMethod} {EvoData}" + (NextEvo == null ? "" : $" - {NextEvo}");
        }
    }
}
