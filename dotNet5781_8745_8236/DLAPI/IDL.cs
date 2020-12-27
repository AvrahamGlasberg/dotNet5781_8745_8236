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
        #endregion

        #region Bus
        void AddBus(Bus bus);
        Bus GetBus(int license);
        #endregion

        #region BusOnTrip
        void AddBusOnTrip(BusOnTrip busOnTrip);
        BusOnTrip GetBusOnTrip(int license);
        #endregion

        #region Line
        void AddLine(Line line);
        Line GetLine(int id);
        #endregion

        #region LineStation
        void AddLineStation(LineStation lineStation);
        LineStation GetLineStation(int station);
        #endregion

        #region LineTrip
        void AddLineTrip(LineTrip lineTrip);
        LineTrip GetLineTrip(int lineId);
        #endregion

        #region Station
        void AddStation(Station station);
        Station GetStation(int code);
        #endregion

        #region Trip
        void AddTrip(Trip trip);
        Trip GetTrip(int lineId);
        #endregion

        #region User
        void AddUser(User user);
        User GetUser(string name);
        #endregion
    }
}
