using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public static class Config
    {
        private static int lineStationId = 0;
        public static int LineStationId { get { return ++lineStationId; } }

        private static int lineId = 0;
        public static int LineId { get { return ++lineId; } }
    }
}
