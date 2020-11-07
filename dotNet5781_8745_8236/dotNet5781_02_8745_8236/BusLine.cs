using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_02_8745_8236
{
    public enum Area
    {
        General, North, South, Center, Jerusalem
    }
    class BusLine
    {
        private int busLine;
        private BusLineStations firstStation;
        private BusLineStations lastStation;
        private Area area;
        private List<BusLineStations> stations;
        public BusLine(int _busLine, int _area) // ctor
        {
            busLine = _busLine;
            area = (Area)_area;
            stations = new List<BusLineStations>();

        }
        public override string ToString()
        {
            string retStr = String.Format("Bus line: {0}, Area: {1} \n", busLine, area);
            foreach(BusLineStations curr in stations)
            {
                retStr += curr.ToString();
                retStr += "\n";
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
        public void addStation(int prevBusStation, int _busStationKey, double _latitude, double _longitude, int _distance, int _drivingTime, string _stationName = "")
        {
            if (stasionExist(_busStationKey))
                throw new ArgumentException("Station already exist.");
            if (prevBusStation == 0) 
            {
                stations.Insert(0, new BusLineStations(_busStationKey, _latitude, _longitude, _distance, _drivingTime, _stationName)); // insert at the front of the list
            }
            else
            {
                if (!stasionExist(prevBusStation))
                    throw new ArgumentException("Previos station does not exist!");
                int ind = stations.FindIndex(station => station.BusStationKey == prevBusStation);
                if(ind == -1)
                    throw new ArgumentException("Previos station does not exist!");
                stations.Insert(ind + 1, new BusLineStations(_busStationKey, _latitude, _longitude, _distance, _drivingTime, _stationName)); // insert at the right place of the list

            }

        }
        public void delStation(int _busStationKey)
        { 
            int ind = stations.FindIndex(station => station.BusStationKey == _busStationKey);
            if(ind == -1)
                throw new ArgumentException("Station already exist.");
            stations.RemoveAt(ind); // insert at the right place of the list
        }
        public bool stasionExist(int _busStationKey)
        {
            return stations.Exists(station => station.BusStationKey == _busStationKey);
        }
        public int distance(int station1Key, int station2Key)
        {
            int dis = 0;
            int ind1 = stations.FindIndex(station =>station.BusStationKey == station1Key);
            int ind2 = stations.FindIndex(station => station.BusStationKey == station2Key);

            if (ind1 == -1 ||ind2 == -1)
                throw new ArgumentException("at least one of the stations isn't exist.");
            for (int i = Math.Min(ind1, ind2) + 1; i <= Math.Max(ind1, ind2); i++)
                dis += stations[i].Distance;
            return dis;
        }
        public int time(int station1Key, int station2Key)
        {
            int time = 0;
            int ind1 = stations.FindIndex(station => station.BusStationKey == station1Key);
            int ind2 = stations.FindIndex(station => station.BusStationKey == station2Key);

            if (ind1 == -1 || ind2 == -1)
                throw new ArgumentException("at least one of the stations isn't exist.");
            for (int i = Math.Min(ind1, ind2) + 1; i <= Math.Max(ind1, ind2); i++)
                time += stations[i].DrivingTime;
            return time;
        }
        public BusLine subRoute(int station1Key, int station2Key)
        {
            BusLine retBus = new BusLine(this.busLine, (int)this.area);
            int ind1 = stations.FindIndex(station => station.BusStationKey == station1Key);
            int ind2 = stations.FindIndex(station => station.BusStationKey == station2Key);

            if (ind1 == -1 || ind2 == -1)
                throw new ArgumentException("at least one of the stations isn't exist.");
            for (int i = Math.Min(ind1, ind2); i <= Math.Max(ind1, ind2); i++)                retBus.addLast(stations[i]);
            return retBus;
        }
        public void addLast(BusLineStations busLineStations)
        {
            stations.Add(busLineStations);
        }


    }

}


