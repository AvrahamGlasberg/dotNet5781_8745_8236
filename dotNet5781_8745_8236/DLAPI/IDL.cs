using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
namespace DLAPI
{
    public interface IDL
    {

        #region AdjacentStation
        void AddAdjacentStation(AdjacentStation adjacentStation);
        AdjacentStation GetAdjacentStation(int station1, int station2);
        IEnumerable<DO.AdjacentStation> GetAdjacentStationsBy(Predicate<DO.AdjacentStation> predicate);
        void UpdateAdjacentStation(AdjacentStation NewAdjacentStation);
        void DeleteAdjacentStation(int station1, int station2);
        #endregion

        #region Bus
        void AddBus(Bus bus);
        Bus GetBus(int license);
        void UpdateBus(Bus newBus);
        void DeleteBus(int license);
        #endregion

        #region BusOnTrip
        void AddBusOnTrip(BusOnTrip busOnTrip);
        BusOnTrip GetBusOnTrip(int License, int LineID, TimeSpan TakeOff);
        void UpdateBusOnTrip(BusOnTrip NewBusOnTrip);
        void DeleteBusOnTrip(int License, int LineID, TimeSpan TakeOff);
        #endregion

        #region Line
        void AddLine(Line line);
        Line GetLine(int id);
        IEnumerable<Line> GetAllLines();
        void UpdateLine(Line NewLine);
        void DeleteLine(int id);
        #endregion

        #region LineStation
        void AddLineStation(LineStation lineStation);
        LineStation GetLineStation(int lineId, int station);
        IEnumerable<LineStation> GetAllLineStations(int lineId);
        IEnumerable<LineStation> GetAllLineStationsBy(Predicate<DO.LineStation> predicate);
        void UpdateLineStation(LineStation NewLineStation);
        void DeleteLineStation(int lineId, int station);
        void DeleteAlLineStationslBy(Predicate<DO.LineStation> predicate);
        #endregion

        #region LineTrip
        void AddLineTrip(LineTrip lineTrip);
        LineTrip GetLineTrip(int lineId, TimeSpan start);
        void UpdateLineTrip(LineTrip NewLineTrip);
        void DeleteLineTrip(int lineId, TimeSpan start);
        #endregion

        #region Station
        void AddStation(Station station);
        Station GetStation(int code);
        IEnumerable<DO.Station> GetAllStations();
        void UpdateStation(Station NewStation);
        void DeleteStation(int code);
        #endregion

        #region Trip
        void AddTrip(Trip trip);
        Trip GetTrip(int id);
        void UpdateTrip(Trip NewTrip);
        void DeleteTrip(int id);
        #endregion

        #region User
        void AddUser(User user);
        User GetUser(string name);
        void UpdateUser(User NewUser);
        void DeleteUser(string name);
        #endregion
    }
}
