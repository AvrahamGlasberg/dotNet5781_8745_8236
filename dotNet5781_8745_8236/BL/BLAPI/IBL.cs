using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLAPI
{
    public interface IBL
    {
        IEnumerable<BO.BusLine> GetAllBusLines();
        void DeleteBusLine(BO.BusLine line);
        void DeleteLineStation(BO.LineStation lineStation);
        IEnumerable<BO.BusStation> GetAllBusStations();
        BO.BusStation GetBusStation(int code);
        void DeleteBusStation(BO.BusStation busStation);
        bool IsTwoStationsInLine(int DOLineId);
        BO.User GetUser(string userName);
        void AddUser(BO.User user);
    }
}

