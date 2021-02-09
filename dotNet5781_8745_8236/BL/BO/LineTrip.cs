using System;

namespace BO
{
    /// <summary>
    /// Class to represent line trip
    /// </summary>
    public class LineTrip
    {
        /// <summary>
        /// The bus line
        /// </summary>
        public BO.BusLine LineInTrip { get; set; }
        /// <summary>
        /// The starting time
        /// </summary>
        public TimeSpan StartAt { get; set; }
        /// <summary>
        /// The frequency of the line trip
        /// </summary>
        public TimeSpan Frequency { get; set; }
        /// <summary>
        /// The finishing time
        /// </summary>
        public TimeSpan FinishAt { get; set; }
    }
}
