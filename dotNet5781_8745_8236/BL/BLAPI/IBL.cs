using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLAPI
{
    public interface IBL
    {
        string Check();
        IEnumerable<BO.BusLine> GetAllBusLines();
        BO.BusStation GetBusStation(int code);
    }
}

