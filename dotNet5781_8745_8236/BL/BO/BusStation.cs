using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BusStation : Station
    {
        public IEnumerable<Line> LinesInstation{ get; set; }
        public GeoCoordinate Location { get; set; }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
