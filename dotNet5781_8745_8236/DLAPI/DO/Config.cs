using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLAPI;
namespace DO
{
    public static class Config
    {
        static private IDL dl;
        static Config()
        {
            dl = DLFactory.GetDL();
            lineId = dl.GetAllLines().Count();
        }
        [Obsolete]
        private static int busOnTripId = 0;
        [Obsolete]
        public static int BusOnTripId { get { return ++busOnTripId; } }

        private static int lineId;
        public static int LineId { get { return ++lineId; } }
        [Obsolete]
        private static int tripId = 0;
        [Obsolete]
        public static int TripId { get { return ++tripId; } }
    }
}
