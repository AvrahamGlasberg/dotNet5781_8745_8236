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
        IEnumerable<BO.BusStation> GetAllBusStations();
        BO.BusStation GetBusStation(int code);
        void DeleteBusStation(BO.BusStation busStation);
    }
}

