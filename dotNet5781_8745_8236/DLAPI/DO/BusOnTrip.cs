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
        /// <summary>
        /// Bus on trip ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Bus's license
        /// </summary>
        public int LicenseNum { get; set; } 
        /// <summary>
        /// Line's ID
        /// </summary>
        public int LineId { get; set; }
        /// <summary>
        /// Planned start time
        /// </summary>
        public TimeSpan PlannedTakeOff { get; set; }
        /// <summary>
        /// Actual start time
        /// </summary>
        public TimeSpan ActualTakeOff { get; set; }
        /// <summary>
        /// Previous station's code
        /// </summary>
        public int PrevStation { get; set; }
        /// <summary>
        /// Time at the previous station
        /// </summary>
        public TimeSpan PrevStationAt { get; set; }
        /// <summary>
        /// Time until next station
        /// </summary>
        public TimeSpan NextStationAt { get; set; }
        /// <summary>
        /// If the object deleted or not
        /// </summary>
        public bool Deleted { get; set; }
    }
}
