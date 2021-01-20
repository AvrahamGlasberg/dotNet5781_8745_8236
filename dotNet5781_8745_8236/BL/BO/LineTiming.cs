using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class LineTiming
    {
        public int Id { get; set; }
        public int LineNumber { get; set; }
        public string Destination { get; set; }
        public TimeSpan Time { get; set; }
    }
}
