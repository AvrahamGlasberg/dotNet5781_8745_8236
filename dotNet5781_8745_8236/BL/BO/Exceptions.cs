using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    #region BO.BusLine
    /// <summary>
    /// Bus line not found exceptions
    /// </summary>
    [Serializable]
    public class BusLineNotFound : Exception
    {
        /// <summary>
        /// Bus line's line number
        /// </summary>
        public int LineNumber;
        /// <summary>
        /// New bus line not found exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="lineNumber">Bus line's line number</param>
        public BusLineNotFound(string message, int lineNumber) : base(message) => LineNumber = lineNumber;
        /// <summary>
        /// New bus line not found exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="lineNumber">Bus line's line number</param>
        /// <param name="innerEX">Inner exception</param>
        public BusLineNotFound(string message, int lineNumber, Exception innerEX) : base(message, innerEX) => LineNumber = lineNumber;
    }
    /// <summary>
    /// Bus line not found exceptions
    /// </summary>
    [Serializable]
    public class BusLineExists : Exception
    {
        /// <summary>
        /// Bus line's line number
        /// </summary>
        public int LineNumber;
        /// <summary>
        /// New bus line found exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="lineNumber">Bus line's line number</param>
        public BusLineExists(string message, int lineNumber) : base(message) => LineNumber = lineNumber;
        /// <summary>
        /// New bus line found exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="lineNumber">Bus line's line number</param>
        /// <param name="innerEX">Inner exception</param>
        public BusLineExists(string message, int lineNumber, Exception innerEX) : base(message, innerEX) => LineNumber = lineNumber;
    }
    #endregion

    #region BO.LineTrip
    /// <summary>
    /// Line trip not found exceptions
    /// </summary>
    [Serializable]
    public class LineTripNotFound : Exception
    {
        /// <summary>
        /// The line number
        /// </summary>
        public int LineNumber;
        /// <summary>
        /// The trip starting time
        /// </summary>
        public TimeSpan Start;
        /// <summary>
        /// New Line trip not found exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="lineNumber">The line trip's line number</param>
        /// <param name="start">The line trip's start time</param>
        public LineTripNotFound(string message, int lineNumber, TimeSpan start) : base(message)
        {
            LineNumber = lineNumber;
            Start = start;
        }
        /// <summary>
        /// New Line trip not found exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="lineNumber">The line trip's line number</param>
        /// <param name="start">The line trip's start time</param>
        /// <param name="innerEX">Inner exception</param>
        public LineTripNotFound(string message, int lineNumber, TimeSpan start, Exception innerEX) : base(message, innerEX)
        {
            LineNumber = lineNumber;
            Start = start;
        }
    }
    /// <summary>
    /// Line trip already exists exceptions
    /// </summary>
    [Serializable]
    public class LineTripExists : Exception
    {
        /// <summary>
        /// The line number
        /// </summary>
        public int LineNumber;
        /// <summary>
        /// The trip starting time
        /// </summary>
        public TimeSpan Start;
        /// <summary>
        /// New Line trip already exists exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="lineNumber">The line trip's line number</param>
        /// <param name="start">The line trip's start time</param>
        public LineTripExists(string message, int lineNumber, TimeSpan start) : base(message)
        {
            LineNumber = lineNumber;
            Start = start;
        }
        /// <summary>
        /// New Line trip already exists exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="lineNumber">The line trip's line number</param>
        /// <param name="start">The line trip's start time</param>
        /// <param name="innerEX">Inner exception</param>
        public LineTripExists(string message, int lineNumber, TimeSpan start, Exception innerEX) : base(message, innerEX)
        {
            LineNumber = lineNumber;
            Start = start;
        }
    }
    #endregion

    #region BO.Station
    /// <summary>
    /// Station not found exceptions
    /// </summary>
    [Serializable]
    public class StationNotFound : Exception
    {
        /// <summary>
        /// Station's code
        /// </summary>
        public int Code;
        /// <summary>
        /// New station not found exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="code">The station's code</param>
        public StationNotFound(string message, int code) : base(message) => Code = code;
        /// <summary>
        /// New station not found exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="code">The station's code</param>
        /// <param name="innerEX">Inner Exception</param>
        public StationNotFound(string message, int code, Exception innerEX) : base(message, innerEX) => Code = code;
    }
    /// <summary>
    /// Station already exists exceptions
    /// </summary>
    [Serializable]
    public class StationExists : Exception
    {
        /// <summary>
        /// Station's code
        /// </summary>
        public int Code;
        /// <summary>
        /// New station already exists exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="code">The station's code</param>
        public StationExists(string message, int code) : base(message) => Code = code;
        /// <summary>
        /// New station already exists exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="code">The station's code</param>
        /// <param name="innerEX">Inner exception</param>
        public StationExists(string message, int code, Exception innerEX) : base(message, innerEX) => Code = code;
    }
    #endregion

    #region BO.Bus
    /// <summary>
    /// Bus not found exceptions
    /// </summary>
    [Serializable]
    public class BusNotFound : Exception
    {
        /// <summary>
        /// The bus's license
        /// </summary>
        public int License;
        /// <summary>
        /// New bus not found exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="license">The bus's license</param>
        public BusNotFound(string message, int license) : base(message) => License = license;
        /// <summary>
        /// New bus not found exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="license">The bus's license</param>
        /// <param name="innerEx">Inner exception</param>
        public BusNotFound(string message, int license, Exception innerEx) : base(message, innerEx) => License = license;
    }
    /// <summary>
    /// Bus already exists exceptions
    /// </summary>
    public class BusExists : Exception
    {
        /// <summary>
        /// The bus's license
        /// </summary>
        public int License;
        /// <summary>
        /// New bus already exists exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="license">The bus's license</param>
        public BusExists(string message, int license) : base(message) => License = license;
        /// <summary>
        /// New bus already exists exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="license">The bus's license</param>
        /// <param name="innerEx">Inner exception</param>
        public BusExists(string message, int license, Exception innerEx) : base(message, innerEx) => License = license;
    }
    #endregion

    #region BO.User
    /// <summary>
    /// User not found exceptions
    /// </summary>
    [Serializable]
    public class UserNotFound : Exception
    {
        /// <summary>
        /// The user's name
        /// </summary>
        public string Name;
        /// <summary>
        /// New user not found exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="name">The user's name</param>
        public UserNotFound(string message, string name) : base(message) => Name = name;
        /// <summary>
        /// New user not found exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="name">The user's name</param>
        /// <param name="innerEX">Inner exception</param>
        public UserNotFound(string message, string name, Exception innerEX) : base(message, innerEX) => Name = name;
    }
    /// <summary>
    /// User already exists exceptions
    /// </summary>
    [Serializable]
    public class UserExists : Exception
    {
        /// <summary>
        /// The user's name
        /// </summary>
        public string Name;
        /// <summary>
        /// New user already exists exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="name">The user's name</param>
        public UserExists(string message, string name) : base(message) => Name = name;
        /// <summary>
        /// New user already exists exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="name">The user's name</param>
        /// <param name="innerEX">Inner exception</param>
        public UserExists(string message, string name, Exception innerEX) : base(message, innerEX) => Name = name;
    }
    /// <summary>
    /// Not enough money for the user exceptions
    /// </summary>
    [Serializable]
    public class NotEnoughMoney : Exception
    {
        /// <summary>
        /// Amount of cash missing
        /// </summary>
        public double shortOf;
        /// <summary>
        /// New not enough money for the user exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="shortof">Amount of cash missing</param>
        public NotEnoughMoney(string message, double shortof) : base(message) => shortOf = shortof;
    }
    #endregion

    #region MissingData

    /// <summary>
    /// Exceptions for missing data
    /// </summary>
    [Serializable]
    public class MissingData : Exception
    {
        /// <summary>
        /// Missing data exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        public MissingData (string message): base(message) { }
        /// <summary>
        /// Missing data exception
        /// </summary>
        /// <param name="message">Message of the exception</param>
        /// <param name="ex">Inner exception</param>
        public MissingData (string message, Exception ex):base(message, ex) { }
    }
    #endregion
}
