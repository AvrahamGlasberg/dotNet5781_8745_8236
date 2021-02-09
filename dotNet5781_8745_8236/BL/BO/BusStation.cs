using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Class to represent bus station.
    /// Inherit from "Station" class
    /// </summary>
    public class BusStation : Station
    {
        /// <summary>
        /// Collection of all lines going in that station
        /// </summary>
        public IEnumerable<Line> LinesInstation{ get; set; }
        /// <summary>
        /// Location of the station
        /// </summary>
        public GeoCoordinate Position { get; set; }
        /// <summary>
        /// Override the ToString
        /// </summary>
        /// <returns>The Staion basic information</returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
