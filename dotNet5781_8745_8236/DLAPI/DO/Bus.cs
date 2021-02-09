using System;

namespace DO
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
        /// If the object deleted or not
        /// </summary>
        public bool Deleted { get; set; }
    }
}
