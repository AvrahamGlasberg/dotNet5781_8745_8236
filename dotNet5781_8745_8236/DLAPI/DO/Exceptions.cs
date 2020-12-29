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
        public bool isExists;
        public AdjacentStationExceptions(int stat1, int stat2, bool Exists) : base()
        {
            station1 = stat1;
            station2 = stat2;
            isExists = Exists;
        }
    }
    public class BusExceptions : Exception
    {
        public int lic;
        public bool isExists;
        public BusExceptions(int license, bool Exists) : base()
        {
            lic = license;
            isExists = Exists;
        }
    }
    public class BusOnTripExceptions : Exception
    {
        public int lic;
        public bool isExists;
        public BusOnTripExceptions(int license, bool Exists) : base()
        {
            lic = license;
            isExists = Exists;
        }
    }
    public class LineExceptions : Exception
    {
        int id;
        public bool isExists;
        public LineExceptions(int ID, bool Exists) : base()
        {
            id = ID;
            isExists = Exists;
        }
    }
    public class LineStationExceptions : Exception
    {
        public int station;
        public bool isExists;
        public LineStationExceptions(int station, bool Exists) : base()
        {
            this.station = station;
            isExists = Exists;
        }
    }
    public class LineTripExceptions : Exception
    {
        public int lineId;
        public bool isExists;
        public LineTripExceptions(int lineid, bool Exists) : base()
        {
            lineId = lineid;
            isExists = Exists;
        }
    }
    public class StationExceptions : Exception
    {
        public int code;
        public bool isExists;
        public StationExceptions(int code, bool Exists) : base()
        {
            this.code = code;
            isExists = Exists;
        }
    }
                                
    public class TripExceptions : Exception
    {
        public int lineId;
        public bool isExists;
        public TripExceptions(int lineid, bool Exists) : base()
        {
            lineId = lineid;
            isExists = Exists;
        }

    }
    public class UserExceptions : Exception
    {
        public string name;
        public bool isExists;
        public UserExceptions(string username, bool Exists) : base()
        {
            name = username;
            isExists = Exists;
        }
    }
    
}
