using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// Class that represents a station
    /// </summary>
    public class Station
    {
        /// <summary>
        /// Station's code
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// Station's code
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Station's Longitude
        /// </summary>
        public double Longitude { get; set; }
        /// <summary>
        /// Station's Latitude
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// If the object deleted or not
        /// </summary>
        public bool Deleted { get; set; }
    }
}
