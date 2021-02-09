using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Class to represent a bus 
    /// </summary>
    public class Bus
    {
        /// <summary>
        /// The bus's license number
        /// </summary>
        public int LicenseNum { get; set; }
        /// <summary>
        /// The bus's starting to act date
        /// </summary>
        public DateTime FromDate { get; set; }
        /// <summary>
        /// Date of the bus's last treatment
        /// </summary>
        public DateTime LastTreatmentDate { get; set; }
        /// <summary>
        /// Total km bus drove
        /// </summary>
        public double TotalTrip { get; set; }
        /// <summary>
        /// Total Km bus drove since last treatment
        /// </summary>
        public double TripSinceTreatment { get; set; }
        /// <summary>
        /// Fuel the bus have remaining
        /// </summary>
        public double FuelRemain { get; set; }
        /// <summary>
        /// The bus's status
        /// </summary>
        public Status BusStatus { get; set; }
        /// <summary>
        /// Overriding ToString
        /// </summary>
        /// <returns>The license in correct format</returns>
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
