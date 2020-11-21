using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Itamar Cohen 318558236 & Avraham Glasberg 206218745
namespace dotNet5781_02_8745_8236
{
    /// <summary>
    /// bus line station. inheritamce from busStation.
    /// </summary>
    public class BusLineStations : BusStation
    {
        /// <summary>
        /// the distance from last station in meters
        /// </summary>
        private int distance;
        /// <summary>
        /// property for distance
        /// </summary>
        public int Distance { get { return distance; } set { distance = value; } }
        /// <summary>
        /// the driving timr from last station in minutes
        /// </summary>
        private int drivingTime;
        /// <summary>
        /// property for drivingTime
        /// </summary>
        public int DrivingTime { get { return drivingTime; } set { drivingTime = value; } }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="_busStationKey">bus station key number</param>
        /// <param name="_latitude">bus station latitude</param>
        /// <param name="_longitude">bus station longitude</param>
        /// <param name="_distance">the station's distance from last station</param>
        /// <param name="_drivingTime">the station's driving time from last station</param>
        /// <param name="_stationName">bus station Name</param>
        public BusLineStations(int _busStationKey, double _latitude, double _longitude, int _distance, int _drivingTime, string _stationName) : base(_busStationKey, _latitude, _longitude, _stationName) 
        {
            distance = _distance;
            drivingTime = _drivingTime;
        }

    }
}
