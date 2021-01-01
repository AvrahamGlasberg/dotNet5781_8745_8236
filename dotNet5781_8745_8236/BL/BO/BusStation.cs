using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BusStation : Station
    {
        public IEnumerable<Line> LinesInstation;
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
