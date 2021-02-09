using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// Class to represent two adjacent stations
    /// </summary>
    public class AdjacentStation
    {
        /// <summary>
        /// First station's code
        /// </summary>
        public int Station1 { get; set; }
        /// <summary>
        /// Second station's code
        /// </summary>
        public int Station2 { get; set; } 
        /// <summary>
        /// Distance between the stations in Km
        /// </summary>
        public double Distance { get; set; }
        /// <summary>
        /// Time takes to travel between the stations
        /// </summary>
        public TimeSpan Time { get; set; }
        /// <summary>
        /// If the object deleted or not
        /// </summary>
        public bool Deleted { get; set; }
    }
}
