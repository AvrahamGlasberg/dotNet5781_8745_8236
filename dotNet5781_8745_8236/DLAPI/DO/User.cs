namespace DO
{
    /// <summary>
    /// Class to represent user
    /// </summary>
    public class User
    {
        /// <summary>
        /// User's name
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// User's password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// If the user is administer or not
        /// </summary>
        public bool Admin { get; set; }
        /// <summary>
        /// If the object deleted or not
        /// </summary>
        public bool Deleted { get; set; }
        /// <summary>
        /// User's cash
        /// </summary>
        public double Cash { get; set; }
    }
}
