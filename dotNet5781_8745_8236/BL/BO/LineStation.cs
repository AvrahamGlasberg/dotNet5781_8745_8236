using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class LineStation : Station
    {
        public int DOLineId { get; set; } 
        public double? DistanceFromPrev { get; set; }
        public TimeSpan? TimeFromPrev { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
