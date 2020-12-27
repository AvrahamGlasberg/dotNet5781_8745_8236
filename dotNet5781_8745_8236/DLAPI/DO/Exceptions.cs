using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class AdjacentStationExceptions : Exception
    {
        public int station1, station2;
        public AdjacentStationExceptions(int stat1, int stat2) : base()
        {
            station1 = stat1;
            station2 = stat2;
        }
    }
    public class BusExceptions : Exception
    {
        public int lic;
        public BusExceptions(int license):base()
        {
            lic = license;
        }
    }
    public class BusOnTripExceptions : Exception
    {
        public int lic;
        public BusOnTripExceptions(int license) :base()
        {
            lic = license;
        }
    }
    public class LineExceptions : Exception
    {
        int id;
        public LineExceptions(int ID):base()
        {
            id = ID;
        }
    }
    public class LineStationExceptions : Exception
    {
        public int station;
        public LineStationExceptions(int station):base()
        {
            this.station = station;
        }
    }
    public class LineTripExceptions : Exception
    {
        public int lineId;
        public LineTripExceptions(int lineid):base()
        {
            lineId = lineid;
        }
    }
    public class StationExceptions : Exception
    {
        public int code;
        public StationExceptions(int code):base()
        {
            this.code = code; 
        }
    }
                                
    public class TripExceptions : Exception
    {
        public int lineId;
        public TripExceptions(int lineid):base()
        {
            lineId = lineid;
        }

    }
    public class UserExceptions : Exception
    {
        public string name;
        public UserExceptions(string username):base()
        {
            name = username;
        }
    }
    
}
