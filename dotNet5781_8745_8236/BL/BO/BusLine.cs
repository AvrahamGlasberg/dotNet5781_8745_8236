using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BusLine : Line
    {
        public IEnumerable<LineStation> LineStations { get; set; }
        public Areas Area { get; set; }
        public override string ToString()
        {
            string str = "Line " + LineNumber.ToString() + ": ";
            str += "\nFirst station: " + LineStations.First<LineStation>().ToString();
            str += "\nLast station: " + LineStations.Last<LineStation>().ToString();
            return str;
        }
    }
}
