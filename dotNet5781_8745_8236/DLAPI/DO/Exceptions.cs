using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    /// <summary>
    /// Adjacent Station Exceptions
    /// </summary>
    [Serializable]
    public class AdjacentStationExceptions : Exception
    {
        /// <summary>
        /// The two station's codes
        /// </summary>
        public int Station1, Station2;
        /// <summary>
        /// Type of exception - whether the object exists or not
        /// </summary>
        public bool IsExists;
        /// <summary>
        /// New Adjacent Station Exception
        /// </summary>
        /// <param name="stat1">First station's code</param>
        /// <param name="stat2">Second station's code</param>
        /// <param name="exists">If the object exists</param>
        public AdjacentStationExceptions(int stat1, int stat2, bool exists) : base()
        {
            Station1 = stat1;
            Station2 = stat2;
            IsExists = exists;
        }
    }

    /// <summary>
    /// Bus Exceptions
    /// </summary>
    [Serializable]
    public class BusExceptions : Exception
    {
        /// <summary>
        /// Bus's License
        /// </summary>
        public int License;
        /// <summary>
        /// Type of exception - whether the object exists or not
        /// </summary>
        public bool IsExists;
        /// <summary>
        /// New Bus exception
        /// </summary>
        /// <param name="license">The Bus's License</param>
        /// <param name="exists">If the object exists</param>
        public BusExceptions(int license, bool exists) : base()
        {
            License = license;
            IsExists = exists;
        }
    }

    /// <summary>
    /// Bus On Trip Exceptions
    /// </summary>
    [Serializable]
    public class BusOnTripExceptions : Exception
    {
        /// <summary>
        /// Line's ID
        /// </summary>
        int LineID;
        /// <summary>
        /// Starts time
        /// </summary>
        TimeSpan TakeOff;
        /// <summary>
        /// License
        /// </summary>
        public int License;
        /// <summary>
        /// Type of exception - whether the object exists or not
        /// </summary>
        public bool IsExists;
        /// <summary>
        /// New bus on trip exception
        /// </summary>
        /// <param name="license">Bus's license</param>
        /// <param name="lineId">Line's ID</param>
        /// <param name="time">Starting time</param>
        /// <param name="exists">If the object exists</param>
        public BusOnTripExceptions(int license, int lineId, TimeSpan time, bool exists) : base()
        {
            this.License = license;
            LineID = lineId;
            TakeOff = time;
            IsExists = exists;
        }
    }

    /// <summary>
    /// Line Exceptions
    /// </summary>
    [Serializable]
    public class LineExceptions : Exception
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id;
        /// <summary>
        /// Type of exception - whether the object exists or not
        /// </summary>
        public bool IsExists;
        /// <summary>
        /// New Line Exception
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="exists">If the object exists</param>
        public LineExceptions(int id, bool exists) : base()
        {
            Id = id;
            IsExists = exists;
        }
    }

    /// <summary>
    /// Line Station Exceptions
    /// </summary>
    [Serializable]
    public class LineStationExceptions : Exception
    {
        /// <summary>
        /// Line's ID
        /// </summary>
        int LineId;
        /// <summary>
        /// Station's code
        /// </summary>
        public int Station;
        /// <summary>
        /// Type of exception - whether the object exists or not
        /// </summary>
        public bool IsExists;
        /// <summary>
        /// New Line Station Exception
        /// </summary>
        /// <param name="lineId">Line's ID</param>
        /// <param name="station">Station's code</param>
        /// <param name="exists">If the object exists</param>
        public LineStationExceptions(int lineId, int station, bool exists) : base()
        {
            this.LineId = lineId;
            this.Station = station;
            IsExists = exists;
        }
    }

    /// <summary>
    /// Line Trip Exceptions
    /// </summary>
    [Serializable]
    public class LineTripExceptions : Exception
    {
        /// <summary>
        /// Line's number
        /// </summary>
        public int LineNumber;
        /// <summary>
        /// Start time
        /// </summary>
        public TimeSpan StartTime;
        /// <summary>
        /// Type of exception - whether the object exists or not
        /// </summary>
        public bool IsExists;
        /// <summary>
        /// New Line Trip Exception
        /// </summary>
        /// <param name="lineNumber">Line's number</param>
        /// <param name="start">Start time</param>
        /// <param name="exists">If the object exists</param>
        public LineTripExceptions(int lineNumber, TimeSpan start, bool exists) : base()
        {
            LineNumber = lineNumber;
            StartTime = start;
            IsExists = exists;
        }
    }

    /// <summary>
    /// Station Exceptions
    /// </summary>
    [Serializable]
    public class StationExceptions : Exception
    {
        /// <summary>
        /// Station's code
        /// </summary>
        public int Code;
        /// <summary>
        /// Type of exception - whether the object exists or not
        /// </summary>
        public bool IsExists;
        /// <summary>
        /// New Station Exception
        /// </summary>
        /// <param name="code">Station's code</param>
        /// <param name="exists">If the object exists</param>
        public StationExceptions(int code, bool exists) : base()
        {
            this.Code = code;
            IsExists = exists;
        }
    }

    /// <summary>
    /// Trip Exceptions
    /// </summary>
    [Serializable]
    public class TripExceptions : Exception
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id;
        /// <summary>
        /// Type of exception - whether the object exists or not
        /// </summary>
        public bool IsExists;
        /// <summary>
        /// New Trip Exception
        /// </summary>
        /// <param name="id">ID<param>
        /// <param name="exists">If the object exists</param>
        public TripExceptions(int id, bool exists) : base()
        {
            Id = id;
            IsExists = exists;
        }

    }

    /// <summary>
    /// User Exceptions
    /// </summary>
    [Serializable]
    public class UserExceptions : Exception
    {
        /// <summary>
        /// User's name
        /// </summary>
        public string Name;
        /// <summary>
        /// Type of exception - whether the object exists or not
        /// </summary>
        public bool IsExists;
        /// <summary>
        /// New User Exception
        /// </summary>
        /// <param name="username">User's name</param>
        /// <param name="exists">If the object exists</param>
        public UserExceptions(string username, bool exists) : base()
        {
            Name = username;
            IsExists = exists;
        }
    }

    /// <summary>
    /// XML exceptions
    /// </summary>
    [Serializable]
    public class XMLFileLoadCreateException : Exception
    {
        /// <summary>
        /// File's path
        /// </summary>
        public string xmlFilePath;
        /// <summary>
        /// New XML File Load Create Exception
        /// </summary>
        /// <param name="xmlPath">File's path</param>
        public XMLFileLoadCreateException(string xmlPath) : base() { xmlFilePath = xmlPath; }
        /// <summary>
        /// New XML File Load Create Exception
        /// </summary>
        /// <param name="xmlPath">File's path</param>
        /// <param name="message">The exception's message</param>
        public XMLFileLoadCreateException(string xmlPath, string message) :
            base(message)
        { xmlFilePath = xmlPath; }
        /// <summary>
        /// New XML File Load Create Exception
        /// </summary>
        /// <param name="xmlPath">File's path</param>
        /// <param name="message">The exception's message</param>
        /// <param name="innerException">Inner exception</param>
        public XMLFileLoadCreateException(string xmlPath, string message, Exception innerException) :
            base(message, innerException)
        { xmlFilePath = xmlPath; }
        /// <summary>
        /// Override the ToString
        /// </summary>
        /// <returns>The file's path</returns>
        public override string ToString() => base.ToString() + $", fail to load or create xml file: {xmlFilePath}";
    }

}
