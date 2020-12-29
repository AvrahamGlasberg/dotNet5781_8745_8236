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
        BusOnTrip GetBusOnTrip(int license);
        void UpdateBusOnTrip(BusOnTrip NewBusOnTrip);
        void DeleteBusOnTrip(int license);
        #endregion

        #region Line
        void AddLine(Line line);
        Line GetLine(int id);
        void UpdateLine(Line NewLine);
        void DeleteLine(int id);
        #endregion

        #region LineStation
        void AddLineStation(LineStation lineStation);
        LineStation GetLineStation(int station);
        void UpdateLineStation(LineStation NewLineStation);
        void DeleteLineStation(int station);
        #endregion

        #region LineTrip
        void AddLineTrip(LineTrip lineTrip);
        LineTrip GetLineTrip(int lineId);
        void UpdateLineTrip(LineTrip NewLineTrip);
        void DeleteLineTrip(int lineId);
        #endregion

        #region Station
        void AddStation(Station station);
        Station GetStation(int code);
        void UpdateStation(Station NewStation);
        void DeleteStation(int code);
        #endregion

        #region Trip
        void AddTrip(Trip trip);
        Trip GetTrip(int lineId);
        void UpdateTrip(Trip NewTrip);
        void DeleteTrip(int lineId);
        #endregion

        #region User
        void AddUser(User user);
        User GetUser(string name);
        void UpdateUser(User NewUser);
        void DeleteUser(string name);
        #endregion
    }
}
