using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class AdjacentStation
    {
        public int Station1 { get; set; } // station code
        public int Station2 { get; set; } // station code
        public double Distance { get; set; }
        public TimeSpan Time { get; set; }
        public bool Deleted { get; set; }
    }
}
