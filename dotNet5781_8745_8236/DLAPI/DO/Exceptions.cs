using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class AdjacentStationExceptions : Exception
    {
        public int Station1, Station2;
        public bool IsExists;
        public AdjacentStationExceptions(int stat1, int stat2, bool exists) : base()
        {
            Station1 = stat1;
            Station2 = stat2;
            IsExists = exists;
        }
    }
    public class BusExceptions : Exception
    {
        public int License;
        public bool IsExists;
        public BusExceptions(int license, bool exists) : base()
        {
            License = license;
            IsExists = exists;
        }
    }
    public class BusOnTripExceptions : Exception
    {
        int LineID;
        TimeSpan TakeOff;
        public int License;
        public bool IsExists;
        public BusOnTripExceptions(int license, int lineId, TimeSpan time, bool exists) : base()
        {
            this.License = license;
            LineID = lineId;
            TakeOff = time;
            IsExists = exists;
        }
    }
    public class LineExceptions : Exception
    {
        int Id;
        public bool IsExists;
        public LineExceptions(int id, bool exists) : base()
        {
            Id = id;
            IsExists = exists;
        }
    }
    public class LineStationExceptions : Exception
    {
        int LineId;
        public int Station;
        public bool IsExists;
        public LineStationExceptions(int lineId, int station, bool exists) : base()
        {
            this.LineId = lineId;
            this.Station = station;
            IsExists = exists;
        }
    }
    public class LineTripExceptions : Exception
    {
        public int LineId;
        TimeSpan StartTime;
        public bool IsExists;
        public LineTripExceptions(int lineid, TimeSpan start, bool exists) : base()
        {
            LineId = lineid;
            StartTime = start;
            IsExists = exists;
        }
    }
    public class StationExceptions : Exception
    {
        public int Code;
        public bool IsExists;
        public StationExceptions(int code, bool exists) : base()
        {
            this.Code = code;
            IsExists = exists;
        }
    }
                                
    public class TripExceptions : Exception
    {
        public int Id;
        public bool IsExists;
        public TripExceptions(int id, bool exists) : base()
        {
            Id = id;
            IsExists = exists;
        }

    }
    public class UserExceptions : Exception
    {
        public string Name;
        public bool IsExists;
        public UserExceptions(string username, bool exists) : base()
        {
            Name = username;
            IsExists = exists;
        }
    }
    
}
