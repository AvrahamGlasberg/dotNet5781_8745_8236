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
            if (DataSource.AdjacentStations.FirstOrDefault(stations => stations.Station1 == adjacentStation.Station1 && stations.Station2 == adjacentStation.Station2 && !stations.Deleted) != null)
                throw new AdjacentStationExceptions(adjacentStation.Station1, adjacentStation.Station2, true);
            else DataSource.AdjacentStations.Add(adjacentStation.Clone());
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
            if (DataSource.BusesOnTrip.FirstOrDefault(curBus => curBus.LicenseNum == busOnTrip.LicenseNum && 
            !curBus.Deleted) != null)
                throw new BusOnTripExceptions(busOnTrip.LicenseNum, true);
            else
                DataSource.BusesOnTrip.Add(busOnTrip.Clone());
        }

        public BusOnTrip GetBusOnTrip(int license)
        {
            BusOnTrip retValue = DataSource.BusesOnTrip.FirstOrDefault(curBus => curBus.LicenseNum == license && 
            !curBus.Deleted);
            if (retValue != null)
                return retValue.Clone();
            else throw new BusOnTripExceptions(license, false);
        }

        public void UpdateBusOnTrip(BusOnTrip NewBusOnTrip)
        {
            BusOnTrip cur = DataSource.BusesOnTrip.FirstOrDefault(curBus => curBus.LicenseNum == NewBusOnTrip.LicenseNum &&
            !curBus.Deleted);
            if (cur == null)
                throw new BusOnTripExceptions(NewBusOnTrip.LicenseNum, false);
            DataSource.BusesOnTrip.Remove(cur);
            DataSource.BusesOnTrip.Add(NewBusOnTrip.Clone());
        }
        public void DeleteBusOnTrip(int license)
        {
            BusOnTrip cur = DataSource.BusesOnTrip.FirstOrDefault(curBus => curBus.LicenseNum == license &&
            !curBus.Deleted);
            if (cur == null)
                throw new BusOnTripExceptions(license, false);
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
            if (DataSource.LineStations.FirstOrDefault(curLineStation => curLineStation.Station == lineStation.Station && !curLineStation.Deleted) != null)
                throw new LineStationExceptions(lineStation.Station, true);
            else
                DataSource.LineStations.Add(lineStation.Clone());
        }

        public LineStation GetLineStation(int station)
        {
            LineStation retValue = DataSource.LineStations.FirstOrDefault(curLineStation => curLineStation.Station == station && !curLineStation.Deleted);
            if (retValue != null)
                return retValue.Clone();
            else throw new LineStationExceptions(station, false);
        }

        public void UpdateLineStation(LineStation NewLineStation)
        {
            LineStation cur = DataSource.LineStations.FirstOrDefault(curLineStation => curLineStation.Station == NewLineStation.Station && !curLineStation.Deleted);
            if(cur == null)
                throw new LineStationExceptions(NewLineStation.Station, false);
            DataSource.LineStations.Remove(cur);
            DataSource.LineStations.Add(NewLineStation);
        }

        public void DeleteLineStation(int station)
        {
            LineStation cur = DataSource.LineStations.FirstOrDefault(curLineStation => curLineStation.Station == station && !curLineStation.Deleted);
            if (cur == null)
                throw new LineStationExceptions(station, false);
            cur.Deleted = true;
        }
        #endregion

        #region LineTrip
        public void AddLineTrip(LineTrip lineTrip)
        {
            if ((DataSource.LinesTrip.FirstOrDefault(curLine => curLine.Id == lineTrip.Id && !curLine.Deleted) != null))
                throw new LineTripExceptions(lineTrip.LineId, true);
            else
                DataSource.LinesTrip.Add(lineTrip.Clone());
        }

        public LineTrip GetLineTrip(int id)
        {
            LineTrip retValue = DataSource.LinesTrip.FirstOrDefault(curLine => curLine.Id == id && !curLine.Deleted);
            if (retValue != null)
                return retValue.Clone();
            else throw new LineTripExceptions(id, false);
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
        #endregion

        #region Trip
        public void AddTrip(Trip trip)
        {
            if (DataSource.Trips.FirstOrDefault(curTrip => curTrip.LineId == trip.LineId && !curTrip.Deleted) != null)
                throw new TripExceptions(trip.LineId, true);
            else
                DataSource.Trips.Add(trip.Clone());
        }

        public Trip GetTrip(int lineId)
        {
            Trip retValue = DataSource.Trips.FirstOrDefault(curTrip => curTrip.LineId == lineId && !curTrip.Deleted);
            if (retValue != null)
                return retValue.Clone();
            else throw new TripExceptions(lineId, false);
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
        #endregion
    }
}
