using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// Class to represent line
    /// </summary>
    public class Line
    {
        /// <summary>
        /// The Line's unique ID
        /// </summary>
        public int Id { get; set; } 
        /// <summary>
        /// The Line's code (number)
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// The line's area
        /// </summary>
        public Areas Area { get; set; }
        /// <summary>
        /// First station's code
        /// </summary>
        public int FirstStation { get; set; } 
        /// <summary>
        /// Last station's code
        /// </summary>
        public int LastStation { get; set; }
        /// <summary>
        /// If the object deleted or not
        /// </summary>
        public bool Deleted { get; set; }
    }
}
