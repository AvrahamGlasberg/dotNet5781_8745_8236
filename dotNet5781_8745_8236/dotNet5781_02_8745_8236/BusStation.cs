using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_8745_8236
{
    class BusStation
    {
        private int busStationKey;
        public int BusStationKey { get { return busStationKey; } }
        private double latitude;
        public double Latitude { get { return latitude; } }
        private double longitude;
        public double Longitude { get { return longitude; } }
        private string stationName;
        public string StationName { get { return stationName; } }
        public BusStation(int _busStationKey, double _latitude, double _longitude, string _stationName = "")
        {
            busStationKey = _busStationKey;
            latitude = _latitude;
            longitude = _longitude;

        }
        public override string ToString()
        {
            return string.Format("Bus Station Code: {0}, {1}°N {2}°E", busStationKey, latitude, longitude);
        }
    }
}
