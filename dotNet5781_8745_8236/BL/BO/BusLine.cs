using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BusLine
    {
        public int Id { get; set; } // run
        public int LineNumber { get; set; }
        public IEnumerable<LineStation> LineStations { get; set; }
    }
}
