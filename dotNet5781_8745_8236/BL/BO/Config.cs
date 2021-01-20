using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public static class Config
    {
        private static int lineOnTripId = 0;
        public static int LineOnTripId { get { return ++lineOnTripId; } }
    }
}
