using System.Collections.Generic;
using System.Linq;

namespace BO
{
    /// <summary>
    /// Class to represent bus line.
    /// Inherit from "Line" class
    /// </summary>
    public class BusLine : Line
    {
        /// <summary>
        /// Collection of all the bus line stations
        /// </summary>
        public IEnumerable<LineStation> LineStations { get; set; }
        /// <summary>
        /// The line's area
        /// </summary>
        public Areas Area { get; set; }
        /// <summary>
        /// Override the ToString.
        /// </summary>
        /// <returns>Information of the line's code and first and last station</returns>
        public override string ToString()
        {
            string str = "Line " + LineNumber.ToString() + ": ";
            str += "\nFirst station: " + LineStations.First<LineStation>().ToString();
            str += "\nLast station: " + LineStations.Last<LineStation>().ToString();
            return str;
        }
    }
}
