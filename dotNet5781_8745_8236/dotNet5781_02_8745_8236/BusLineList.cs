﻿using System;
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
        public void addLine(int busNum, int area)
        {
            if (busNum < 0 || area < 0 || area > 4)
                throw new ArgumentException("Illegal Input!");
            busLst.Add(new BusLine(busNum, area));
        }
        //deletefisrt
        public void delLine(int busNum)
        {
            int ind = busLst.FindIndex(line => line.BusNum == busNum);
            if (ind == -1)
                throw new ArgumentException("Line is not exists!");
            busLst.RemoveAt(ind);
        }
        public BusLineList findBuses(int station)
        {
            BusLineList retLst = new BusLineList();
            retLst.busLst = busLst.FindAll(bus => bus.stasionExist(station));
            if(retLst.busLst.Count == 0)
                throw new ArgumentException("No buses drive in that station!");
            return retLst;
        }

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

        public BusLineList sortedList()
        {
            BusLineList retLst = new BusLineList();
            retLst.busLst = new List<BusLine>(busLst);
            retLst.busLst.Sort();
            return retLst;
        }
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
            set
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