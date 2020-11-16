using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Itamar Cohen 318558236 & Avraham Glasberg 206218745
namespace dotNet5781_02_8745_8236
{
    /// <summary>
    /// a collection of bus lines.
    /// </summary>
    class BusLineList : IEnumerable
    {
        /// <summary>
        /// list of the bus lines
        /// </summary>
        List<BusLine> busLst;
        /// <summary>
        /// property of buslst
        /// </summary>
        public BusLineList() { busLst = new List<BusLine>(); }
        /// <summary>
        /// implimenting IEnumerable interface
        /// </summary>
        /// <returns>list enumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return busLst.GetEnumerator();
        }
        //not checking "haloch-chazor" according to dan z.
        /// <summary>
        /// this function add line to the list
        /// </summary>
        /// <param name="busNum">the number of the bus</param>
        /// <param name="area">the area of the bus</param>
        public void addLine(int busNum, int area)
        {
            if (busNum < 0 || area < 0 || area > 4)
                throw new ArgumentException("Illegal Input!");
            busLst.Add(new BusLine(busNum, area));
        }
        /// <summary>
        /// this function delete the fisrt apperance of the line
        /// </summary>
        /// <param name="busNum">the number of the bus</param>
        public void delLine(int busNum)
        {
            int ind = busLst.FindIndex(line => line.BusNum == busNum);
            if (ind == -1)
                throw new ArgumentException("Line is not exists!");
            busLst.RemoveAt(ind);
        }
        /// <summary>
        /// this function find the buses that drive in the station
        /// </summary>
        /// <param name="station">the station that checked</param>
        /// <returns>list of buses that drive in the station</returns>
        public BusLineList findBuses(int station)
        {
            BusLineList retLst = new BusLineList();
            retLst.busLst = busLst.FindAll(bus => bus.stasionExist(station));
            if(retLst.busLst.Count == 0)
                throw new ArgumentException("No buses drive in that station!");
            return retLst;
        }
        /// <summary>
        /// this function find the buses that drive in two stations
        /// </summary>
        /// <param name="first">the src station</param>
        /// <param name="second">the dst station</param>
        /// <returns>list of the buses that drive in two stations</returns>
        public BusLineList findBusesInTwoStations(int first, int second)
        {
            BusLineList retLst = new BusLineList();
            foreach(BusLine cur in busLst)
            {
                if (cur.subRoute(first, second) != null)
                    retLst.busLst.Add(cur.subRoute(first, second));
            }
            if (retLst.busLst.Count == 0)
                throw new ArgumentException("No buses drive in that stations!");
            return retLst;
        }
        /// <summary>
        /// this func sorting list of BusLineList according to total driving times
        /// </summary>
        /// <returns>the sorted list</returns>
        public BusLineList sortedList()
        {
            BusLineList retLst = new BusLineList();
            retLst.busLst = new List<BusLine>(busLst);
            retLst.busLst.Sort();
            return retLst;
        }
        /// <summary>
        /// this function implement the indexer of the class
        /// </summary>
        /// <param name="busNum">the number of the bus</param>
        /// <param name="index">the number of apperance(times) of this bus number in the list</param>
        /// <returns>the bus line in the require index</returns>
        public BusLine this[int busNum, int index = 0]
        {
            get
            {
                if(index < 0)
                    throw new ArgumentException("Illegal input!");
                int ind = -1;
                do
                {
                    ind = busLst.FindIndex(ind + 1, bus => bus.BusNum == busNum);
                    if (ind == -1)
                        throw new ArgumentException("Bus does not exists in that index!");
                    index--;
                } while (index >= 0);
                return busLst[ind];
            }
            set // set to the require index
            {
                if (index < 0)
                    throw new ArgumentException("Illegal input!");
                int ind = -1;
                do
                {
                    ind = busLst.FindIndex(ind + 1, bus => bus.BusNum == busNum);
                    if (ind == -1)
                        throw new ArgumentException("Bus does not exists in that index!");
                    index--;
                } while (index >= 0);
                busLst[ind] = value;
            }
        }
    }
}
