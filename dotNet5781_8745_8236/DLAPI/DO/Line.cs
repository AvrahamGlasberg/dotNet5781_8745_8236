using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class Line
    {
        public int Id { get; set; } // run
        public int Code { get; set; }
        public Areas Area { get; set; }
        public int FirstStation { get; set; } // station code
        public int LastStation { get; set; }
        public bool Deleted { get; set; }
    }
}
