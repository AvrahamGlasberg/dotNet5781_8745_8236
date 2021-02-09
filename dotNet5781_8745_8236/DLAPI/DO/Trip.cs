using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    [Obsolete]
    public class Trip
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; } 
        /// <summary>
        /// User's name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Line's id
        /// </summary>
        public int LineId { get; set; }
        /// <summary>
        /// Cur station's code
        /// </summary>
        public int InStation { get; set; }
        /// <summary>
        ///Time at
        /// </summary>
        public TimeSpan InAt { get; set; }
        /// <summary>
        /// Destination
        /// </summary>
        public int OutStation { get; set; }
        /// <summary>
        /// Out time
        /// </summary>
        public TimeSpan OutAt { get; set; }
        /// <summary>
        /// If the object deleted or not
        /// </summary>
        public bool Deleted { get; set; }
    }
}
