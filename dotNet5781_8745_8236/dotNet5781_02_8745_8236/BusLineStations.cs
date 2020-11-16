using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Itamar Cohen 318558236 & Avraham Glasberg 206218745
namespace dotNet5781_02_8745_8236
{
    class BusLineStations : BusStation
    {
        private int distance;
        public int Distance { get { return distance; } set { distance = value; } }
        private int drivingTime;
        public int DrivingTime { get { return drivingTime; } set { drivingTime = value; } }
        public BusLineStations(int _busStationKey, double _latitude, double _longitude, int _distance, int _drivingTime, string _stationName) : base(_busStationKey, _latitude, _longitude, _stationName) 
        {
            distance = _distance;
            drivingTime = _drivingTime;
        }

    }
}
