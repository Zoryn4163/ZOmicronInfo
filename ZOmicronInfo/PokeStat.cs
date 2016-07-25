using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace ZOmicronInfo
{
    public class PokeStat
    {
        public int HP { get; set; }
        public int  A { get; set; }
        public int  D { get; set; }
        public int SA { get; set; }
        public int SD { get; set; }
        public int SP { get; set; }

        [JsonIgnore]
        public int Total => HP + A + D + SA + SD + SP;

        [JsonIgnore]
        public string NiceString => $"{HP} HP, {A} A, {D} D, {SA} SA, {SD} SD, {SP} SP".ToLower();

        [JsonIgnore]
        public int[] RawArray => new int[] {HP, A, D, SA, SD, SP};

        public PokeStat()
        {
            
        }

        public PokeStat(int[] stats)
        {
            try
            {
                HP = stats[0];
                A = stats[1];
                D = stats[2];
                SA = stats[3];
                SD = stats[4];
                SP = stats[5];
            }
            catch
            {
                //one or more missing, Int can't be null so they will show 0
                MessageBox.Show("ERR");
            }
        }
    }
}
