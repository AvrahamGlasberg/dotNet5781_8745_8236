using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// Class that represents line station
    /// </summary>
    public class LineStation
    {
        /// <summary>
        /// The line's ID
        /// </summary>
        public int LineId { get; set; }
        /// <summary>
        /// The station's code
        /// </summary>
        public int Station { get; set; } 
        /// <summary>
        /// The station's index in the line
        /// </summary>
        public int LineStationIndex { get; set; }
        /// <summary>
        /// Prev station's code
        /// </summary>
        public int? PrevStation { get; set; }
        /// <summary>
        /// Next station's code
        /// </summary>
        public int? NextStation { get; set; }
        /// <summary>
        /// If the object deleted or not
        /// </summary>
        public bool Deleted { get; set; }
    }
}
