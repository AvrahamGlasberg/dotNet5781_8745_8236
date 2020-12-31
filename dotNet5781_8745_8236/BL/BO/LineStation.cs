using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class LineStation : Station
    {
        public int Id { get; set; } 
        public double? DistanceFromPrev { get; set; }
        public TimeSpan? TimeFromPrev { get; set; }

        public override string ToString()
        {
            return base.ToString();
            //string str = ""; 
            //if (DistanceFromPrev != null && TimeFromPrev != null)
            //    str += string.Format("-> Distance: {0}, Time: {1} -> ", DistanceFromPrev, TimeFromPrev);
            //str += base.ToString();
            //return str + '\n';
        }
    }
}
