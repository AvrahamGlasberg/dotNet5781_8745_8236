using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// Class that represents line's trip
    /// </summary>
    public class LineTrip
    {
        /// <summary>
        /// The line's ID
        /// </summary>
        public int LineId { get; set; } 
        /// <summary>
        /// Starting time
        /// </summary>
        public TimeSpan StartAt { get; set; }
        /// <summary>
        /// Frequency in minutes
        /// </summary>
        public TimeSpan Frequency { get; set; }
        /// <summary>
        /// Finishing time
        /// </summary>
        public TimeSpan FinishAt { get; set; }
        /// <summary>
        /// If the object deleted or not
        /// </summary>
        public bool Deleted { get; set; }
    }
}
