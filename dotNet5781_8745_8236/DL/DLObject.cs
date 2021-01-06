﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLAPI;
using DO;
using DS;
namespace DL
{
    sealed class DLObject : IDL
    {
        #region singelton 
        DLObject() { }
        static readonly DLObject instance = new DLObject();
        public static DLObject Instance { get { return instance; } }
        #endregion

        //CRUD
        #region AdjacentStation
        public void AddAdjacentStation(AdjacentStation adjacentStation)
        {
            if (DataSource.AdjacentStations.FirstOrDefault(stations => stations.Station1 == adjacentStation.Station1 && stations.Station2 == adjacentStation.Station2 && !stations.Deleted) == null)
                DataSource.AdjacentStations.Add(adjacentStation.Clone());

        }

        public AdjacentStation GetAdjacentStation(int station1, int station2)
        {
            AdjacentStation retValue = DataSource.AdjacentStations.FirstOrDefault(stations => stations.Station1 == station1 && stations.Station2 == station2 && !stations.Deleted);
            if (retValue != null)
                return retValue.Clone();
            else throw new AdjacentStationExceptions(station1, station2, false);
        }

        public void UpdateAdjacentStation(AdjacentStation NewAdjacentStation)
        {
            AdjacentStation cur = DataSource.AdjacentStations.FirstOrDefault(stations => stations.Station1 == NewAdjacentStation.Station1 && stations.Station2 == NewAdjacentStation.Station2 && !stations.Deleted);
            if (cur == null)
                throw new AdjacentStationExceptions(NewAdjacentStation.Station1, NewAdjacentStation.Station2, false);
            DataSource.AdjacentStations.Remove(cur);
            DataSource.AdjacentStations.Add(NewAdjacentStation.Clone());
        }

        public void DeleteAdjacentStation(int station1, int station2)
        {
            AdjacentStation cur = DataSource.AdjacentStations.FirstOrDefault(stations => stations.Station1 == station1 && stations.Station2 == station2 && !stations.Deleted);
            if (cur == null)
                throw new AdjacentStationExceptions(station1, station2, false);
            cur.Deleted = true;
        }
        #endregion

        #region Bus
        public void AddBus(Bus bus)
        {
            if (DataSource.Buses.FirstOrDefault(curBus => curBus.LicenseNum == bus.LicenseNum && !curBus.Deleted) != null)
                throw new BusExceptions(bus.LicenseNum, true);
            else
                DataSource.Buses.Add(bus.Clone());
        }

        public Bus GetBus(int license)
        {
            Bus retValue = DataSource.Buses.FirstOrDefault(curBus => curBus.LicenseNum == license && !curBus.Deleted);
            if (retValue != null)
                return retValue.Clone();
            else throw new BusExceptions(license, false);
        }

        public void UpdateBus(Bus NewBus)
        {
            Bus cur = DataSource.Buses.FirstOrDefault(curBus => curBus.LicenseNum == NewBus.LicenseNum && !curBus.Deleted);
            if (cur == null)
                throw new BusExceptions(NewBus.LicenseNum, false);
            DataSource.Buses.Remove(cur);
            DataSource.Buses.Add(NewBus.Clone());
        }
        public void DeleteBus(int license)
        {
            Bus cur = DataSource.Buses.FirstOrDefault(curBus => curBus.LicenseNum == license && !curBus.Deleted);
            if (cur == null)
                throw new BusExceptions(license, false);
            cur.Deleted = true;
        }
        #endregion

        #region BusOnTrip
        public void AddBusOnTrip(BusOnTrip busOnTrip)
        {
            if (DataSource.BusesOnTrip.FirstOrDefault(curBus => curBus.LicenseNum == busOnTrip.LicenseNum
            && curBus.LineId == busOnTrip.LineId && curBus.PlannedTakeOff == busOnTrip.PlannedTakeOff && !curBus.Deleted) != null)
                throw new BusOnTripExceptions(busOnTrip.LicenseNum, busOnTrip.LineId, busOnTrip.PlannedTakeOff, true);
            else
                DataSource.BusesOnTrip.Add(busOnTrip.Clone());
        }

        public BusOnTrip GetBusOnTrip(int License, int LineID, TimeSpan TakeOff)
        {
            BusOnTrip retValue = DataSource.BusesOnTrip.FirstOrDefault(curBus => curBus.LicenseNum == License 
            && curBus.LineId == LineID && curBus.PlannedTakeOff == TakeOff && !curBus.Deleted);
            if (retValue != null)
                return retValue.Clone();
            else throw new BusOnTripExceptions(License, LineID, TakeOff, false);
        }

        public void UpdateBusOnTrip(BusOnTrip NewBusOnTrip)
        {
            BusOnTrip cur = DataSource.BusesOnTrip.FirstOrDefault(curBus => curBus.LicenseNum == NewBusOnTrip.LicenseNum && curBus.LineId == NewBusOnTrip.LineId && curBus.PlannedTakeOff == NewBusOnTrip.PlannedTakeOff && !curBus.Deleted);
            if (cur == null)
                throw new BusOnTripExceptions(NewBusOnTrip.LicenseNum,NewBusOnTrip.LineId, NewBusOnTrip.PlannedTakeOff, false);
            DataSource.BusesOnTrip.Remove(cur);
            DataSource.BusesOnTrip.Add(NewBusOnTrip.Clone());
        }
        public void DeleteBusOnTrip(int License, int LineID, TimeSpan TakeOff)
        {
            BusOnTrip cur = DataSource.BusesOnTrip.FirstOrDefault(curBus => curBus.LicenseNum == License
            && curBus.LineId == LineID && curBus.PlannedTakeOff == TakeOff && !curBus.Deleted);
            if (cur == null)
                throw new BusOnTripExceptions(License, LineID, TakeOff, false);
            cur.Deleted = true;
        }
        #endregion

        #region Line
        public void AddLine(Line line)
        {
            if (DataSource.Lines.FirstOrDefault(curLine => curLine.Id == line.Id && !curLine.Deleted) != null)
                throw new LineExceptions(line.Id, true);
            else
                DataSource.Lines.Add(line.Clone());
        }

        public Line GetLine(int id)
        {
            Line retValue = DataSource.Lines.FirstOrDefault(curLine => curLine.Id == id && !curLine.Deleted);
            if (retValue != null)
                return retValue.Clone();
            else throw new LineExceptions(id, false);
        }

        public IEnumerable<Line> GetAllLines()
        {
            if (DataSource.Lines.Count == 0)
                throw new LineExceptions(0, false);
            return from CurLine in DataSource.Lines
                   where !CurLine.Deleted
                   orderby CurLine.Code
                   select CurLine;
        }
        public void UpdateLine(Line NewLine)
        {
            Line cur = DataSource.Lines.FirstOrDefault(curLine => curLine.Id == NewLine.Id && !curLine.Deleted);
            if (cur == null)
                throw new LineExceptions(NewLine.Id, false);
            DataSource.Lines.Remove(cur);
            DataSource.Lines.Add(NewLine.Clone());
        }
        public void DeleteLine(int id)
        {
            Line cur = DataSource.Lines.FirstOrDefault(curLine => curLine.Id == id && !curLine.Deleted);
            if (cur == null)
                throw new LineExceptions(id, false);
            cur.Deleted = true;
        }
        #endregion

        #region LineStation
        public void AddLineStation(LineStation lineStation)
        {
            if (DataSource.LineStations.FirstOrDefault(curLineStation => curLineStation.LineId == lineStation.LineId && curLineStation.Station == lineStation.Station && !curLineStation.Deleted) != null)
                throw new LineStationExceptions(lineStation.LineId, lineStation.Station, true);
            else
                DataSource.LineStations.Add(lineStation.Clone());
        }

        public LineStation GetLineStation(int lineId, int station)
        {
            LineStation retValue = DataSource.LineStations.FirstOrDefault(curLineStation => curLineStation.LineId == lineId && curLineStation.Station == station && !curLineStation.Deleted);
            if (retValue != null)
                return retValue.Clone();
            else throw new LineStationExceptions(lineId, station, false);
        }
        
        public IEnumerable<LineStation> GetAllLineStations(int lineId)
        {
            if (DataSource.LineStations.Count == 0)
                throw new LineStationExceptions(0, 0, false);
            return from item in DataSource.LineStations
                   where item.LineId == lineId && !item.Deleted
                   orderby item.LineStationIndex
                   select item.Clone();
        }

        public void UpdateLineStation(LineStation NewLineStation)
        {
            LineStation cur = DataSource.LineStations.FirstOrDefault(curLineStation => curLineStation.LineId == NewLineStation.LineId && curLineStation.Station == NewLineStation.Station && !curLineStation.Deleted);
            if(cur == null)
                throw new LineStationExceptions(NewLineStation.LineId, NewLineStation.Station, false);
            DataSource.LineStations.Remove(cur);
            DataSource.LineStations.Add(NewLineStation);
        }

        public void DeleteLineStation(int lineId, int station)
        {
            LineStation cur = DataSource.LineStations.FirstOrDefault(curLineStation => curLineStation.LineId == lineId && curLineStation.Station == station && !curLineStation.Deleted);
            if (cur == null)
                throw new LineStationExceptions(lineId, station, false);
            cur.Deleted = true;
        }

        public void DeleteAlLineStationslBy(Predicate<DO.LineStation> predicate)
        {
            foreach (var station in DataSource.LineStations)
                if (predicate(station))
                    station.Deleted = true;
        }

        public IEnumerable<LineStation> GetAllLineStationsBy(Predicate<DO.LineStation> predicate)
        {
            return from lineStation in DataSource.LineStations
                   where predicate(lineStation) && !lineStation.Deleted
                   select lineStation.Clone();
        }
        #endregion

        #region LineTrip
        public void AddLineTrip(LineTrip lineTrip)
        {
            if ((DataSource.LinesTrip.FirstOrDefault(curLine => curLine.LineId == lineTrip.LineId && curLine.StartAt == lineTrip.StartAt && !curLine.Deleted) != null))
                throw new LineTripExceptions(lineTrip.LineId, lineTrip.StartAt, true);
            else
                DataSource.LinesTrip.Add(lineTrip.Clone());
        }

        public LineTrip GetLineTrip(int lineId, TimeSpan start)
        {
            LineTrip retValue = DataSource.LinesTrip.FirstOrDefault(curLine => curLine.LineId == lineId && curLine.StartAt == start && !curLine.Deleted);
            if (retValue != null)
                return retValue.Clone();
            else throw new LineTripExceptions(lineId, start, false);
        }

        public void UpdateLineTrip(LineTrip NewLineTrip)
        {
            LineTrip cur = DataSource.LinesTrip.FirstOrDefault(curLineTrip => curLineTrip.LineId == NewLineTrip.LineId && curLineTrip.StartAt == NewLineTrip.StartAt && !curLineTrip.Deleted);
            if (cur == null)
                throw new LineTripExceptions(NewLineTrip.LineId, NewLineTrip.StartAt, false);
            DataSource.LinesTrip.Remove(cur);
            DataSource.LinesTrip.Add(NewLineTrip);
        }

        public void DeleteLineTrip(int lineId, TimeSpan start)
        {
            LineTrip cur = DataSource.LinesTrip.FirstOrDefault(curLineTrip => curLineTrip.LineId == lineId && curLineTrip.StartAt == start && !curLineTrip.Deleted);
            if (cur == null)
                throw new LineTripExceptions(lineId, start, false);
            cur.Deleted = true;
        }
        #endregion

        #region Station
        public void AddStation(Station station)
        {
            if (DataSource.Stations.FirstOrDefault(curStation => curStation.Code == station.Code && 
            !curStation.Deleted) != null)
                throw new StationExceptions(station.Code, true);
            else
                DataSource.Stations.Add(station.Clone());
        }

        public Station GetStation(int code)
        {
            Station retValue = DataSource.Stations.FirstOrDefault(curStation => curStation.Code == code && 
            !curStation.Deleted);
            if (retValue != null)
                return retValue.Clone();
            else throw new StationExceptions(code, false);
        }
        public IEnumerable<DO.Station> GetAllStations()
        {
            return from station in DataSource.Stations
                   where !station.Deleted
                   select station;
        }

        public void UpdateStation(Station NewStation)
        {
            Station cur = DataSource.Stations.FirstOrDefault(curStation => curStation.Code == NewStation.Code && !curStation.Deleted);
            if (cur == null)
                throw new StationExceptions(NewStation.Code, false);
            DataSource.Stations.Remove(cur);
            DataSource.Stations.Add(NewStation);
        }

        public void DeleteStation(int code)
        {
            Station cur = DataSource.Stations.FirstOrDefault(curStation => curStation.Code == code && !curStation.Deleted);
            if (cur == null)
                throw new StationExceptions(code, false);
            cur.Deleted = true;
        }
        #endregion

        #region Trip
        public void AddTrip(Trip trip)
        {
            if (DataSource.Trips.FirstOrDefault(curTrip => curTrip.Id == trip.Id && !curTrip.Deleted) != null)
                throw new TripExceptions(trip.Id, true);
            else
                DataSource.Trips.Add(trip.Clone());
        }

        public Trip GetTrip(int id)
        {
            Trip retValue = DataSource.Trips.FirstOrDefault(curTrip => curTrip.Id == id && !curTrip.Deleted);
            if (retValue != null)
                return retValue.Clone();
            else throw new TripExceptions(id, false);
        }

        public void UpdateTrip(Trip NewTrip)
        {
            Trip cur = DataSource.Trips.FirstOrDefault(curTrip => curTrip.Id == NewTrip.Id && !curTrip.Deleted);
            if (cur == null)
                throw new TripExceptions(NewTrip.Id, false);
            DataSource.Trips.Remove(cur);
            DataSource.Trips.Add(NewTrip);
        }

        public void DeleteTrip(int id)
        {
            Trip cur = DataSource.Trips.FirstOrDefault(curTrip => curTrip.Id == id && !curTrip.Deleted);
            if (cur == null)
                throw new TripExceptions(id, false);
            cur.Deleted = true;
        }
        #endregion

        #region User
        public void AddUser(User user)
        {
            if (DataSource.Users.FirstOrDefault(curUser => curUser.UserName == user.UserName && !curUser.Deleted) != null)
                throw new UserExceptions(user.UserName, true);
            else
                DataSource.Users.Add(user.Clone());
        }

        public User GetUser(string name)
        {
            User retValue = DataSource.Users.FirstOrDefault(curUser => curUser.UserName == name && !curUser.Deleted);
            if (retValue != null)
                return retValue.Clone();
            else throw new UserExceptions(name, false);
        }

        public void UpdateUser(User NewUser)
        {
            User cur = DataSource.Users.FirstOrDefault(curUser => curUser.UserName == NewUser.UserName && !curUser.Deleted);
            if (cur == null)
                throw new UserExceptions(NewUser.UserName, false);
            DataSource.Users.Remove(cur);
            DataSource.Users.Add(NewUser);
        }

        public void DeleteUser(string userName)
        {
            User cur = DataSource.Users.FirstOrDefault(curUser => curUser.UserName == userName && !curUser.Deleted);
            if (cur == null)
                throw new UserExceptions(userName, false);
            cur.Deleted = true;
        }
        #endregion

    }
}
