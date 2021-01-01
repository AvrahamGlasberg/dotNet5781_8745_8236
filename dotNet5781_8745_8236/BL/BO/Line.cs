using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Line
    {
        public int DOLineId { get; set; }
        public int LineNumber { get; set; }
        public Station EndStation { get; set; }
    }
}
