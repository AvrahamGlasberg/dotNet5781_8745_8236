using System;

namespace BO
{
    /// <summary>
    /// Class to represent bus line station.
    /// Inherit from "Station" class
    /// </summary>
    public class LineStation : Station
    {
        /// <summary>
        /// The line's ID
        /// </summary>
        public int DOLineId { get; set; } 
        /// <summary>
        /// Distance in Km to the next station
        /// </summary>
        public double? DistanceToNext { get; set; }
        /// <summary>
        /// Time in travel to the next station
        /// </summary>
        public TimeSpan? TimeToNext { get; set; }
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
