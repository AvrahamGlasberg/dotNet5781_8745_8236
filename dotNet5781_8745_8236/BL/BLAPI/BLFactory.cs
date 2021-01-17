using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL;

namespace BLAPI
{
    public static class BLFactory
    {
        public static IBL GetBL()
        {
            try
            {
                return BLImp.Instance;
            }
            catch//creating dal/ds failed
            {
                throw new BO.MissingData("DL could not open!");
            }
        }
    }
}
