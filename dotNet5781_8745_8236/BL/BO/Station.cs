namespace BO
{
    /// <summary>
    /// Class to represent basic station
    /// </summary>
    public class Station
    {
        /// <summary>
        /// The station's code
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// The station's name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Override the ToString
        /// </summary>
        /// <returns>The station's code and name</returns>
        public override string ToString()
        {
            return string.Format("Station Code: {0}, Name: {1}", Code, Name);
        }
    }
}
