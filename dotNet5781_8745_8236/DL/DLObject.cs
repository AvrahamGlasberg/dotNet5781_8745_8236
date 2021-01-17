using System;
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
            AdjacentStation retValue = DataSource.AdjacentStations.FirstOrDefault(stations => (stations.Station1 == station1 && stations.Station2 == station2) || (stations.Station1 == station2 && stations.Station2 == station1) && !stations.Deleted);
            if (retValue != null)
                return retValue.Clone();
            else throw new AdjacentStationExceptions(station1, station2, false);
        }

        public IEnumerable<DO.AdjacentStation> GetAdjacentStationsBy(Predicate<DO.AdjacentStation> predicate)
        {
            return from stations in DataSource.AdjacentStations
                   where predicate(stations) && !stations.Deleted
                   select stations.Clone();
        }
        public void UpdateAdjacentStation(AdjacentStation newAdjacentStation)
        {
            AdjacentStation cur = DataSource.AdjacentStations.FirstOrDefault(stations => (stations.Station1 == newAdjacentStation.Station1 && stations.Station2 == newAdjacentStation.Station2) || (stations.Station1 == newAdjacentStation.Station2 && stations.Station2 == newAdjacentStation.Station1) && !stations.Deleted);
            if (cur == null)
                throw new AdjacentStationExceptions(newAdjacentStation.Station1, newAdjacentStation.Station2, false);
            DataSource.AdjacentStations.Remove(cur);
            DataSource.AdjacentStations.Add(newAdjacentStation.Clone());
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

        public IEnumerable<Bus> GettAllBuses()
        {
            return from bus in DataSource.Buses
                   where !bus.Deleted
                   orderby bus.LicenseNum
                   select bus.Clone();
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

        public BusOnTrip GetBusOnTrip(int license, int lineID, TimeSpan takeOff)
        {
            BusOnTrip retValue = DataSource.BusesOnTrip.FirstOrDefault(curBus => curBus.LicenseNum == license
            && curBus.LineId == lineID && curBus.PlannedTakeOff == takeOff && !curBus.Deleted);
            if (retValue != null)
                return retValue.Clone();
            else throw new BusOnTripExceptions(license, lineID, takeOff, false);
        }

        public void UpdateBusOnTrip(BusOnTrip busOnTrip)
        {
            BusOnTrip cur = DataSource.BusesOnTrip.FirstOrDefault(curBus => curBus.LicenseNum == busOnTrip.LicenseNum && curBus.LineId == busOnTrip.LineId && curBus.PlannedTakeOff == busOnTrip.PlannedTakeOff && !curBus.Deleted);
            if (cur == null)
                throw new BusOnTripExceptions(busOnTrip.LicenseNum, busOnTrip.LineId, busOnTrip.PlannedTakeOff, false);
            DataSource.BusesOnTrip.Remove(cur);
            DataSource.BusesOnTrip.Add(busOnTrip.Clone());
        }
        public void DeleteBusOnTrip(int license, int lineID, TimeSpan takeOff)
        {
            BusOnTrip cur = DataSource.BusesOnTrip.FirstOrDefault(curBus => curBus.LicenseNum == license
            && curBus.LineId == lineID && curBus.PlannedTakeOff == takeOff && !curBus.Deleted);
            if (cur == null)
                throw new BusOnTripExceptions(license, lineID, takeOff, false);
            cur.Deleted = true;
        }
        #endregion

        #region Line
        public void AddLine(Line line)
        {
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
            return from CurLine in DataSource.Lines
                   where !CurLine.Deleted
                   orderby CurLine.Code
                   select CurLine;
        }
        public void UpdateLine(Line newLine)
        {
            Line cur = DataSource.Lines.FirstOrDefault(curLine => curLine.Id == newLine.Id && !curLine.Deleted);
            if (cur == null)
                throw new LineExceptions(newLine.Id, false);
            DataSource.Lines.Remove(cur);
            DataSource.Lines.Add(newLine.Clone());
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
            return from item in DataSource.LineStations
                   where item.LineId == lineId && !item.Deleted
                   orderby item.LineStationIndex
                   select item.Clone();
        }

        public void UpdateLineStation(LineStation newLineStation)
        {
            LineStation cur = DataSource.LineStations.FirstOrDefault(curLineStation => curLineStation.LineId == newLineStation.LineId && curLineStation.Station == newLineStation.Station && !curLineStation.Deleted);
            if(cur == null)
                throw new LineStationExceptions(newLineStation.LineId, newLineStation.Station, false);
            DataSource.LineStations.Remove(cur);
            DataSource.LineStations.Add(newLineStation);
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

        public IEnumerable<DO.LineTrip> GetAllLineTripsBy(Predicate<DO.LineTrip> predicate)
        {
            return from lTrip in DataSource.LinesTrip
                   where predicate(lTrip) && !lTrip.Deleted
                   orderby lTrip.StartAt
                   select lTrip;
        }
        public void UpdateLineTrip(LineTrip newLineTrip)
        {
            LineTrip cur = DataSource.LinesTrip.FirstOrDefault(curLineTrip => curLineTrip.LineId == newLineTrip.LineId && curLineTrip.StartAt == newLineTrip.StartAt && !curLineTrip.Deleted);
            if (cur == null)
                throw new LineTripExceptions(newLineTrip.LineId, newLineTrip.StartAt, false);
            DataSource.LinesTrip.Remove(cur);
            DataSource.LinesTrip.Add(newLineTrip);
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

        public void UpdateStation(Station newStation)
        {
            Station cur = DataSource.Stations.FirstOrDefault(curStation => curStation.Code == newStation.Code && !curStation.Deleted);
            if (cur == null)
                throw new StationExceptions(newStation.Code, false);
            DataSource.Stations.Remove(cur);
            DataSource.Stations.Add(newStation);
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

        public void UpdateTrip(Trip newTrip)
        {
            Trip cur = DataSource.Trips.FirstOrDefault(curTrip => curTrip.Id == newTrip.Id && !curTrip.Deleted);
            if (cur == null)
                throw new TripExceptions(newTrip.Id, false);
            DataSource.Trips.Remove(cur);
            DataSource.Trips.Add(newTrip);
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

        public void UpdateUser(User newUser)
        {
            User cur = DataSource.Users.FirstOrDefault(curUser => curUser.UserName == newUser.UserName && !curUser.Deleted);
            if (cur == null)
                throw new UserExceptions(newUser.UserName, false);
            DataSource.Users.Remove(cur);
            DataSource.Users.Add(newUser);
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
