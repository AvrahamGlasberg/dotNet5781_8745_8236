using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Class to represent a user
    /// </summary>
    public class User
    {
        /// <summary>
        /// The user's name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// The user's password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// If the user is admin or not
        /// </summary>
        public bool Admin { get; set; }
        /// <summary>
        /// The user's cash
        /// </summary>
        public double Cash { get; set; }
    }
}
