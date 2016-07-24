using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZOmicronInfo
{
    public class PokeMove
    {
        public virtual int LevelLearned { get; set; }
        public virtual string Move { get; set; }

        public override string ToString()
        {
            return $"L{LevelLearned}: {Move}";
        }
    }

    public class EggMove : PokeMove
    {
        public override int LevelLearned { get; set; }

        public EggMove()
        {
            LevelLearned = 0;
        }

        public override string ToString()
        {
            return Move;
        }
    }
}
