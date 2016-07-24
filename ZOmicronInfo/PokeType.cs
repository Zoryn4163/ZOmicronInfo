using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZOmicronInfo
{
    public class PokeType
    {
        public string Type { get; set; }

        public PokeType(string typeString)
        {
            Type = typeString;
        }

        public override string ToString()
        {
            if (Type == "NIL")
                return string.Empty;
            return Type;
        }
    }
}
