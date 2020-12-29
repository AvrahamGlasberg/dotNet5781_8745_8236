using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class LineStation
    {
        public int LineId { get; set; } // line id 
        public int Station { get; set; } // station code 
        public int LineStationIndex { get; set; }
        public int PrevStation { get; set; } // station code
        public int NextStation { get; set; }
        public bool Deleted { get; set; }
    }
}
