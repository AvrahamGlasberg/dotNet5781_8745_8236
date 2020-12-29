using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLAPI;
using DLAPI;
namespace BL
{
    class BLImp : IBL
    {
        IDL dl = DLFactory.GetDL();
    }
}
