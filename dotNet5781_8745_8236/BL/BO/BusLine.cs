using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BusLine
    {
        public int LineNumber { get; set; }
        public IEnumerable<LineStation> LineStations { get; set; }
        public override string ToString()
        {
            string str = "Line " + LineNumber.ToString() + ": ";
            str += "\nFirst station: " + LineStations.First<LineStation>().ToString();
            str += "\nLast station: " + LineStations.Last<LineStation>().ToString();
            return str;
        }
    }
}
