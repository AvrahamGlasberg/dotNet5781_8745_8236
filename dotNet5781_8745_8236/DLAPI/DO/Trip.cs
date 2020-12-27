using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class Trip
    {
        public int Id { get; set; }
        public string UserName { get; set; }//user username
        public int LineId { get; set; }
        public int InStation { get; set; }//station code
        public TimeSpan InAt { get; set; }
        public int OutStation { get; set; }//station code
        public TimeSpan OutAt { get; set; }
    }
}
