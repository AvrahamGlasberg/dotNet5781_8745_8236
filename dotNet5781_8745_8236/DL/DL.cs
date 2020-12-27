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
    sealed class DL : IDL
    {
        #region singelton 
        DL() { }
        static readonly DL instance = new DL();
        public static DL Instance { get { return instance; } }
        #endregion

        #region AdjacentStation
        public void AddAdjacentStation(AdjacentStation adjacentStation)
        {
            if (DataSource.AdjacentStations.FirstOrDefault(stations => stations.Station1 == adjacentStation.Station1 && stations.Station2 == adjacentStation.Station2) != null)
                throw new AdjacentStationExceptions(adjacentStation.Station1, adjacentStation.Station2);
            else DataSource.AdjacentStations.Add(adjacentStation);
        }

        public AdjacentStation GetAdjacentStation(int station1, int station2)
        {
            AdjacentStation retValue = DataSource.AdjacentStations.FirstOrDefault(stations => stations.Station1 == station1 && stations.Station2 == station2);
            if (retValue != null)
                return retValue.Clone();
            //else throw new ...
            else return null;
        }
        #endregion

        #region Bus
        public void AddBus(Bus bus)
        {
            if (DataSource.Buses.FirstOrDefault(curBus => curBus.LicenseNum == bus.LicenseNum) != null)
                throw new BusExceptions(bus.LicenseNum);
            else
                DataSource.Buses.Add(bus);
        }

        public Bus GetBus(int license)
        {
            Bus retValue = DataSource.Buses.FirstOrDefault(curBus => curBus.LicenseNum == license);
            if (retValue != null)
                return retValue.Clone();
            //else throw new ...
            else return null;
        }
        #endregion

        #region BusOnTrip
        public void AddBusOnTrip(BusOnTrip busOnTrip)
        {
            if (DataSource.Buses.FirstOrDefault(curBus => curBus.LicenseNum == busOnTrip.LicenseNum) != null)
                throw new BusOnTripExceptions(busOnTrip.LicenseNum);
            else
                DataSource.BusesOnTrip.Add(busOnTrip);
        }

        public BusOnTrip GetBusOnTrip(int license)
        {
            BusOnTrip retValue = DataSource.BusesOnTrip.FirstOrDefault(curBus => curBus.LicenseNum == license);
            if (retValue != null)
                return retValue.Clone();
            //else throw new ...
            else return null;
        }
        #endregion

        #region Line
        public void AddLine(Line line)
        {
            if (DataSource.Lines.FirstOrDefault(curLine => curLine.Id == line.Id) != null)
                throw new LineExceptions(line.Id);
            else
                DataSource.Lines.Add(line);
        }

        public Line GetLine(int id)
        {
            Line retValue = DataSource.Lines.FirstOrDefault(curLine => curLine.Id == id);
            if (retValue != null)
                return retValue.Clone();
            //else throw new ...
            else return null;
        }
        #endregion

        #region LineStation
        public void AddLineStation(LineStation lineStation)
        {
            if (DataSource.LineStations.FirstOrDefault(curLineStation => curLineStation.Station == lineStation.Station) != null)
                throw new LineStationExceptions(lineStation.Station);
            else
                DataSource.LineStations.Add(lineStation);
        }

        public LineStation GetLineStation(int station)
        {
            LineStation retValue = DataSource.LineStations.FirstOrDefault(curLineStation => curLineStation.Station == station);
            if (retValue != null)
                return retValue.Clone();
            //else throw new ...
            else return null;
        }
        #endregion

        #region LineTrip
        public void AddLineTrip(LineTrip lineTrip)
        {
            if ((DataSource.LinesTrip.FirstOrDefault(curLine => curLine.LineId == lineTrip.LineId) != null))
                throw new LineTripExceptions(lineTrip.LineId);
            else
                DataSource.LinesTrip.Add(lineTrip);
        }

        public LineTrip GetLineTrip(int lineId)
        {
            LineTrip retValue = DataSource.LinesTrip.FirstOrDefault(curLine => curLine.LineId == lineId);
            if (retValue != null)
                return retValue.Clone();
            //else throw new ...
            else return null;
        }
        #endregion

        #region Station
        public void AddStation(Station station)
        {
            if (DataSource.Stations.FirstOrDefault(curStation => curStation.Code == station.Code) != null)
                throw new StationExceptions(station.Code);
            else
                DataSource.Stations.Add(station);
        }

        public Station GetStation(int code)
        {
            Station retValue = DataSource.Stations.FirstOrDefault(curStation => curStation.Code == code);
            if (retValue != null)
                return retValue.Clone();
            //else throw new ...
            else return null;
        }
        #endregion

        #region Trip
        public void AddTrip(Trip trip)
        {
            if (DataSource.Trips.FirstOrDefault(curTrip => curTrip.LineId == trip.LineId) != null)
                throw new TripExceptions(trip.LineId);
            else
                DataSource.Trips.Add(trip);
        }

        public Trip GetTrip(int lineId)
        {
            Trip retValue = DataSource.Trips.FirstOrDefault(curTrip => curTrip.LineId == lineId);
            if (retValue != null)
                return retValue.Clone();
            //else throw new ...
            else return null;
        }
        #endregion

        #region User
        public void AddUser(User user)
        {
            if (DataSource.Users.FirstOrDefault(curUser => curUser.UserName == user.UserName) != null)
                throw new UserExceptions(user.UserName);
            else
                DataSource.Users.Add(user);
        }

        public User GetUser(string name)
        {
            User retValue = DataSource.Users.FirstOrDefault(curUser => curUser.UserName == name);
            if (retValue != null)
                return retValue.Clone();
            //else throw new ...
            else return null;
        }
        #endregion
    }
}
