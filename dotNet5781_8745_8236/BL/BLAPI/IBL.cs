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
        void UpdateTimeAndDis(BO.LineStation first, BO.LineStation second);
        #endregion

        #region BO.LineTrip
        void AddLineTrip(BO.LineTrip lineTrip);
        void DeleteLineTrip(BO.LineTrip lineTrip);
        IEnumerable<BO.LineTrip> GetAllLineTripsInLine(BO.BusLine busLine);
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

        #region BO.Bus
        IEnumerable<BO.Bus> GetAllBuses();
        void Refuel(BO.Bus bus);
        void Treatment(BO.Bus bus);
        void AddBus(BO.Bus bus);
        void DeleteBus(BO.Bus bus);
        #endregion

        #region BO.User
        BO.User GetUser(string userName);
        void AddUser(BO.User user);
        #endregion

        void StartSimulator(TimeSpan startTime, int rate, Action<TimeSpan> func);
        void SetStationPanel(int station, Action<BO.LineTiming> updateBus);
        void StopSimulator();
    }
}

