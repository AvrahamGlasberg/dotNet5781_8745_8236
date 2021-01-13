using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Bus
    {
        public int LicenseNum { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime LastTreatmentDate { get; set; }
        public double TotalTrip { get; set; }
        public double TripSinceTreatment { get; set; }
        public double FuelRemain { get; set; }
        public Status BusStatus { get; set; }
        public override string ToString()
        {
            string strLic = LicenseNum.ToString();
            if (strLic.Length == 7)
            {
                strLic = strLic.Insert(2, "-");
                strLic = strLic.Insert(6, "-");
            }
            else
            {
                strLic = strLic.Insert(3, "-");
                strLic = strLic.Insert(6, "-");
            }
            return strLic;
        }
    }
}
