using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public static class Config
    {
        private static int busOnTripId = 0;
        public static int BusOnTripId { get { return ++busOnTripId; } }

        private static int lineId = 0;
        public static int LineId { get { return ++lineId; } }

        private static int tripId = 0;
        public static int TripId { get { return ++tripId; } }
    }
}
