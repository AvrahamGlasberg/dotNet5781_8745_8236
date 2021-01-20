using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    [Obsolete]
    public class BusOnTrip
    {
        public int Id { get; set; } // run
        public int LicenseNum { get; set; } // bus license
        public int LineId { get; set; } // line id
        public TimeSpan PlannedTakeOff { get; set; }
        public TimeSpan ActualTakeOff { get; set; }
        public int PrevStation { get; set; } // station code 
        public TimeSpan PrevStationAt { get; set; }
        public TimeSpan NextStationAt { get; set; }
        public bool Deleted { get; set; }
    }
}
