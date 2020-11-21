using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
//Itamar Cohen 318558236 & Avraham Glasberg 206218745
namespace dotNet5781_02_8745_8236
{
    /// <summary>
    /// basic bus station class
    /// </summary>
    public class BusStation
    {
        /// <summary>
        /// the bus station number key 
        /// </summary>
        private int busStationKey;
        /// <summary>
        /// property for busStationKey
        /// </summary>
        public int BusStationKey { get { return busStationKey; } }
        /// <summary>
        /// the latitude of the station.
        /// </summary>
        private double latitude;
        /// <summary>
        /// property for latitude.
        /// </summary>
        public double Latitude { get { return latitude; } }
        /// <summary>
        /// the longitude of the station.
        /// </summary>
        private double longitude;
        /// <summary>
        /// property for longitude.
        /// </summary>
        public double Longitude { get { return longitude; } }
        /// <summary>
        /// the station's name
        /// </summary>
        private string stationName;
        /// <summary>
        /// property for stationName.
        /// </summary>
        public string StationName { get { return stationName; } }
        /// <summary>
        /// constructor 
        /// </summary>
        /// <param name="_busStationKey">bus station key number</param>
        /// <param name="_latitude">bus station latitude</param>
        /// <param name="_longitude">bus station longitude</param>
        /// <param name="_stationName">bus station Name</param>
        public BusStation(int _busStationKey, double _latitude, double _longitude, string _stationName)
        {
            busStationKey = _busStationKey;
            latitude = _latitude;
            longitude = _longitude;
            stationName = _stationName;

        }
        /// <summary>
        /// ovverride to the function ToString
        /// </summary>
        /// <returns>the bus station number and location as string</returns>
        public override string ToString()
        {
            return string.Format("Bus Station Code: {0}, {1}°N {2}°E", busStationKey, latitude, longitude);
        }
    }
}
