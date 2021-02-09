using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLAPI;
namespace DO
{
    /// <summary>
    /// Config class for running numbers
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// DL implimentation object
        /// </summary>
        static private IDL dl;
        /// <summary>
        /// Static constructor
        /// </summary>
        static Config()
        {
            dl = DLFactory.GetDL();
            lineId = dl.GetAllLines().Count();
        }
        [Obsolete]
        private static int busOnTripId = 0;
        [Obsolete]
        public static int BusOnTripId { get { return ++busOnTripId; } }
        /// <summary>
        /// The last Line's ID
        /// </summary>
        private static int lineId;
        /// <summary>
        /// Gets ID for the next line
        /// </summary>
        public static int LineId { get { return ++lineId; } }
        [Obsolete]
        private static int tripId = 0;
        [Obsolete]
        public static int TripId { get { return ++tripId; } }
    }
}
