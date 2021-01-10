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
        public double? DistanceToNext { get; set; }
        public TimeSpan? TimeToNext { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
