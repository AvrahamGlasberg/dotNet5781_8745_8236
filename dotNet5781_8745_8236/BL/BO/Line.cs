using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Class to represent basic line
    /// </summary>
    public class Line
    {
        /// <summary>
        /// The line's ID
        /// </summary>
        public int DOLineId { get; set; }
        /// <summary>
        /// The line number (code)
        /// </summary>
        public int LineNumber { get; set; }
        /// <summary>
        /// The line's end station (destination)
        /// </summary>
        public Station EndStation { get; set; }
    }
}
