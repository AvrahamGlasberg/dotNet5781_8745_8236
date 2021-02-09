using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Config class for running numbers
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// The last line on trip id
        /// </summary>
        private static int lineOnTripId = 0;
        /// <summary>
        /// Gets running number for lines on trip
        /// </summary>
        public static int LineOnTripId { get { return ++lineOnTripId; } }
    }
}
