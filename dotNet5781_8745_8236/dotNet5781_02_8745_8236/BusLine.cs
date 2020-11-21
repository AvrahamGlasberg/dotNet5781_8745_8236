using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Itamar Cohen 318558236 & Avraham Glasberg 206218745
namespace dotNet5781_02_8745_8236
{
    /// <summary>
    /// enum type for bus line's area options
    /// </summary>
    public enum Area
    {
        General, North, South, Center, Jerusalem
    }
    /// <summary>
    /// bus line class. implimenting IComparable interface
    /// </summary>
    public class BusLine : IComparable
    {
        /// <summary>
        /// bus line key number
        /// </summary>
        private int busNum;
        /// <summary>
        /// property for busNum
        /// </summary>
        public int BusNum { get { return busNum; } }
        /// <summary>
        /// first station in the bus line. does not save value - there is no need for real varieble
        /// </summary>
        public BusLineStations FirstStation { get { if (stations.Count != 0) return stations[0]; else throw new ArgumentException("There are no stations"); } }
        /// <summary>
        /// last station in the bus line. does not save value - there is no need for real varieble
        /// </summary>
        public BusLineStations LastStation { get { if (stations.Count != 0) return stations[stations.Count - 1]; else throw new ArgumentException("There are no stations"); } }
        /// <summary>
        /// the area of the bus. enum type.
        /// </summary>
        private Area area;
        /// <summary>
        /// property for area
        /// </summary>
        public Area Area { get { return area; } }
        /// <summary>
        /// a list of all the stations in that bus line
        /// </summary>
        private List<BusLineStations> stations;
        /// <summary>
        /// property for stations
        /// </summary>
        public List<BusLineStations> Stations { get { return stations; } }
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="_busNum">the bus line key number</param>
        /// <param name="_area">the bus line area</param>
        public BusLine(int _busNum, int _area) 
        {
            busNum = _busNum;
            area = (Area)_area;
            stations = new List<BusLineStations>();
        }
        /// <summary>
        /// ovverride to the function ToString
        /// </summary>
        /// <returns>the bus and all it's stations information as string</returns>
        public override string ToString()
        {
            int i = 1;
            string retStr = String.Format("Bus line number: {0}, Area: {1} \n", busNum, area);
            foreach (BusLineStations curr in stations)
            {
                retStr += i.ToString() + ": ";
                retStr += curr.ToString();
                retStr += "\n";
                i++;
            }
            return retStr;
        }
        /// <summary>
        /// adding a station in the middle of the route
        /// </summary>
        /// <param name="prevBusStation">previos bus station key number</param>
        /// <param name="_busStationKey">this new bus station key number</param>
        /// <param name="_latitude">this new bus station latitude</param>
        /// <param name="_longitude">this new bus station longitude</param>
        /// <param name="_distance">distance from previos station</param>
        /// <param name="_drivingTime">driving time from previos station</param>
        /// <param name="distanceToNext">distance to next station</param>
        /// <param name="timeToNext">driving time to previos station</param>
        /// <param name="_stationName">the new bus station name</param>
        public void addStation(int prevBusStationNumber, int _busStationKey, double _latitude, double _longitude, int _distance, int _drivingTime, int distanceToNext, int timeToNext,  string _stationName = "")
        {
            if (_busStationKey < 0 || _latitude < 0 || _longitude < 0 || _distance <= 0 || _drivingTime <= 0 || distanceToNext <= 0 || timeToNext <= 0)
                throw new ArgumentException("Illegal input!");
            if (stasionExist(_busStationKey))
                throw new ArgumentException("Station already exist.");
            if (stations.Count < 2)
                throw new ArgumentException("You have to input first/last station first.");
            int ind = stations.FindIndex(station => station.BusStationKey == prevBusStationNumber);
            if (ind == -1 || ind == stations.Count - 1)
                throw new ArgumentException("Illegal previos bus number. (for last station input last station first)");

            //updating next station.
            stations.Insert(ind + 1, new BusLineStations(_busStationKey, _latitude, _longitude, _distance, _drivingTime, _stationName)); // insert at the right place of the list
            stations[ind + 2].Distance = distanceToNext;
            stations[ind + 2].DrivingTime = timeToNext;
        }
        /// <summary>
        /// adding new first station
        /// </summary>
        /// <param name="_busStationKey">this new bus station key number</param>
        /// <param name="_latitude">this new bus station latitude</param>
        /// <param name="_longitude">this new bus station longitude</param>
        /// <param name="distanceToNext">distance to next station</param>
        /// <param name="timeToNext">driving time to previos station</param>
        /// <param name="_stationName">the new bus station name</param>
        public void addFirstStation(int _busStationKey, double _latitude, double _longitude, int distanceToNext = 0, int timeToNext = 0, string _stationName = "")
        {
            if (_busStationKey < 0 || _latitude < 0 ||_longitude < 0 || distanceToNext < 0 || timeToNext < 0)
                throw new ArgumentException("Illegal input!");
            if (stasionExist(_busStationKey))
                throw new ArgumentException("Station already exist.");
            if(stations.Count > 0 && (distanceToNext==0|| timeToNext == 0))
                throw new ArgumentException("You have to input info about the next station!");
            stations.Insert(0, new BusLineStations(_busStationKey, _latitude, _longitude,0, 0, _stationName));
            if(stations.Count > 1)
            {
                stations[1].Distance = distanceToNext;
                stations[1].DrivingTime = timeToNext;
            }
        }
        /// <summary>
        /// adding new last station
        /// </summary>
        /// <param name="_busStationKey">this new bus station key number</param>
        /// <param name="_latitude">this new bus station latitude</param>
        /// <param name="_longitude">this new bus station longitude</param>
        /// <param name="_distance">distance from previos station</param>
        /// <param name="_drivingTime">driving time from previos station</param>
        /// <param name="_stationName">the new bus station name</param>
        public void addLastStation(int _busStationKey, double _latitude, double _longitude, int _distance, int _drivingTime, string _stationName = "")
        {
            if (_busStationKey < 0 || _latitude < 0 || _longitude < 0 || _distance <= 0 || _drivingTime <= 0)
                throw new ArgumentException("Illegal input!");
            if (stasionExist(_busStationKey))
                throw new ArgumentException("Station already exist.");
            if (stations.Count == 0)
                throw new ArgumentException("You have to input first station first.");
            stations.Add(new BusLineStations(_busStationKey, _latitude, _longitude, _distance, _drivingTime, _stationName));
        }
        /// <summary>
        /// deleting the first or last station
        /// </summary>
        /// <param name="_busStationKey">the bus station to delete key number</param>
        public void deleteFirstOrLastStation(int _busStationKey)
        {
            if(stations.Count <= 3)
                throw new ArgumentException("Line must have at least 2 stations");
            int ind = stations.FindIndex(station => station.BusStationKey == _busStationKey);
            if (ind == -1 || (ind != 0 && ind != stations.Count - 1))
                throw new ArgumentException("Station does not exist in the beggining or in the end.");
            stations.RemoveAt(ind); // insert at the right place of the list
            if(ind == 0 && stations.Count > 0)
            {
                stations[0].DrivingTime = 0;
                stations[0].Distance = 0;
            }
        }
        /// <summary>
        /// delting a station from the middle of the route.
        /// </summary>
        /// <param name="_busStationKey">the bus station to delete key number</param>
        /// <param name="newDistance">the distance in the new route</param>
        /// <param name="newTime">the driving  time in the new route</param>
        public void deleteStation(int _busStationKey, int newDistance, int newTime)
        {
            if (stations.Count <= 3)
                throw new ArgumentException("Line must have at least 2 stations");
            int ind = stations.FindIndex(station => station.BusStationKey == _busStationKey);
            if (newDistance <= 0 || newTime <= 0)
                throw new ArgumentException("Illegal input!");
            if (ind == -1 || ind == 0 || ind == stations.Count - 1)
                throw new ArgumentException("Station does not exist in the middle of the list!");
            stations.RemoveAt(ind); // insert at the right place of the list
            stations[ind].Distance = newDistance;
            stations[ind].DrivingTime = newTime;
        }
        /// <summary>
        /// checking if the stations exists
        /// </summary>
        /// <param name="_busStationKey">the bus station in chech key nuber</param>
        /// <returns>true if the station exsits and false if does not.</returns>
        public bool stasionExist(int _busStationKey)
        {
            return stations.Exists(station => station.BusStationKey == _busStationKey);
        }
        /// <summary>
        /// calculating the total distance of the bus between 2 stations
        /// </summary>
        /// <param name="station1Key">first station key number</param>
        /// <param name="station2Key">second station key number</param>
        /// <returns>the total meters of the bus between 2 stations</returns>
        public int totalDistance(int station1Key, int station2Key)
        {
            int dis = 0;
            int ind1 = stations.FindIndex(station => station.BusStationKey == station1Key);
            int ind2 = stations.FindIndex(station => station.BusStationKey == station2Key);

            if (ind1 == -1 || ind2 == -1)
                throw new ArgumentException("at least one of the stations does not exist.");
            for (int i = Math.Min(ind1, ind2) + 1; i <= Math.Max(ind1, ind2); i++)
                dis += stations[i].Distance;
            return dis;
        }
        /// <summary>
        /// calculating the total driving time of the bus between 2 stations
        /// </summary>
        /// <param name="station1Key">first station key number</param>
        /// <param name="station2Key">second station key number</param>
        /// <returns>the total minutes of the bus's driving between 2 stations</returns>
        public int totalTime(int station1Key, int station2Key)
        {
            int time = 0;
            int ind1 = stations.FindIndex(station => station.BusStationKey == station1Key);
            int ind2 = stations.FindIndex(station => station.BusStationKey == station2Key);

            if (ind1 == -1 || ind2 == -1)
                throw new ArgumentException("at least one of the stations does not exist.");
            for (int i = Math.Min(ind1, ind2) + 1; i <= Math.Max(ind1, ind2); i++)
                time += stations[i].DrivingTime;
            return time;
        }
        /// <summary>
        /// claculate the route between 2 stations
        /// </summary>
        /// <param name="station1Key">first station key number</param>
        /// <param name="station2Key">second station key number</param>
        /// <returns>new line with the same number and area andd the sub route between the 2 stations, and null if there is none</returns>
        public BusLine subRoute(int station1Key, int station2Key)
        {
            BusLine retBus = new BusLine(this.busNum, (int)this.area);
            int ind1 = stations.FindIndex(station => station.BusStationKey == station1Key);
            int ind2 = stations.FindIndex(ind1 + 1, station => station.BusStationKey == station2Key);

            if (ind1 == -1 || ind2 == -1)
                return null;
            for (int i = ind1; i <= ind2; i++) 
                retBus.stations.Add(stations[i]);
            return retBus;
        }
        /// <summary>
        /// implimenting IComparable interface
        /// </summary>
        /// <param name="obj">bus line obkect</param>
        /// <returns>the comparTo between the bus lines driving time</returns>

        public int CompareTo(object obj)
        {
            return this.totalTime(this.FirstStation.BusStationKey, this.LastStation.BusStationKey).CompareTo(((BusLine)obj).totalTime(((BusLine)obj).FirstStation.BusStationKey, ((BusLine)obj).LastStation.BusStationKey));
        }
    }

}


