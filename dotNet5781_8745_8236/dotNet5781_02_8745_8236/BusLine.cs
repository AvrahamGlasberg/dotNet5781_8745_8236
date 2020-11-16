using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Itamar Cohen 318558236 & Avraham Glasberg 206218745
namespace dotNet5781_02_8745_8236
{
    public enum Area
    {
        General, North, South, Center, Jerusalem
    }
    class BusLine : IComparable
    {
        private int busNum;
        public int BusNum { get { return busNum; } }
        public BusLineStations FirstStation { get { if (stations.Count != 0) return stations[0]; else throw new ArgumentException("There are no stations"); } }
        public BusLineStations LastStation { get { if (stations.Count != 0) return stations[stations.Count - 1]; else throw new ArgumentException("There are no stations"); } }
        private Area area;
        private List<BusLineStations> stations;
        public List<BusLineStations> Stations { get { return stations; } }
        public BusLine(int _busNum, int _area) // ctor
        {
            busNum = _busNum;
            area = (Area)_area;
            stations = new List<BusLineStations>();
        }
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
        ///  -?- what to do in the next station feilds
        /// </summary>
        /// <param name="prevBusStation"></param>
        /// <param name="_busStationKey"></param>
        /// <param name="_latitude"></param>
        /// <param name="_longitude"></param>
        /// <param name="_distance"></param>
        /// <param name="_drivingTime"></param>
        /// <param name="_stationName"></param>
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

            
            stations.Insert(ind + 1, new BusLineStations(_busStationKey, _latitude, _longitude, _distance, _drivingTime, _stationName)); // insert at the right place of the list
            stations[ind + 2].Distance = distanceToNext;
            stations[ind + 2].DrivingTime = timeToNext;
        }
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
        public bool stasionExist(int _busStationKey)
        {
            return stations.Exists(station => station.BusStationKey == _busStationKey);
        }
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

        public int CompareTo(object obj)
        {
            return this.totalTime(this.FirstStation.BusStationKey, this.LastStation.BusStationKey).CompareTo(((BusLine)obj).totalTime(((BusLine)obj).FirstStation.BusStationKey, ((BusLine)obj).LastStation.BusStationKey));
        }
    }

}


