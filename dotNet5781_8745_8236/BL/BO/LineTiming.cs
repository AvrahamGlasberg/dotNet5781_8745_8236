using System;

namespace BO
{
    /// <summary>
    /// Class for line timing, represent time until the line arrives.
    /// </summary>
    public class LineTiming
    {
        /// <summary>
        /// Line timing's ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The line's number (code)
        /// </summary>
        public int LineNumber { get; set; }
        /// <summary>
        /// The line's destination
        /// </summary>
        public string Destination { get; set; }
        /// <summary>
        /// The time until the line arrive
        /// </summary>
        public TimeSpan Time { get; set; }
    }
}
