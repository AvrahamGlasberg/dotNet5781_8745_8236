using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DS
{
    public static class DataSource
    {
        public static List<AdjacentStation> AdjacentStations;
        public static List<Bus> Buses;
        public static List<BusOnTrip> BusesOnTrip;
        public static List<Line> Lines;
        public static List<LineStation> LineStations;
        public static List<LineTrip> LinesTrip;
        public static List<Station> Stations;
        public static List<Trip> Trips;
        public static List<User> Users;

        static DataSource()
        {
            AdjacentStations = new List<AdjacentStation>();
            Buses = new List<Bus>();
            BusesOnTrip = new List<BusOnTrip>();
            Lines = new List<Line>();
            LineStations = new List<LineStation>();
            LinesTrip = new List<LineTrip>();
            Stations = new List<Station>();
            Trips = new List<Trip>();
            Users = new List<User>();
        }
    }
    
}
