using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLAPI
{
    public interface IBL
    {
        #region BO.BusLine
        void AddBusLine(BO.BusLine busLine);
        BO.BusLine GetUpdatedBOBusLine(int dolineId);
        void UpdateBusLineArea(BO.BusLine busLine);
        IEnumerable<BO.BusLine> GetAllBusLines();
        void DeleteBusLine(BO.BusLine line);
        void DeleteLineStation(BO.LineStation lineStation);
        bool IsTwoStationsInLine(int DOLineId);
        void AddLineStationToBusLine(BO.BusLine busLine, BO.Station station, int index);
        #endregion

        #region BO.BusStation
        IEnumerable<BO.BusStation> GetAllBusStations();
        BO.BusStation GetBusStation(int code);
        void AddBusStation(BO.BusStation busStation);
        void DeleteBusStation(BO.BusStation busStation);
        void UpdateBusStation(BO.BusStation busStation);
        BO.LineStation StationToLineStation(BO.Station st);
        IEnumerable<BO.Station> GetAllStationsNotInLine(int DOLineId);
        #endregion

        #region BO.User
        BO.User GetUser(string userName);
        void AddUser(BO.User user);
        #endregion

    }
}

