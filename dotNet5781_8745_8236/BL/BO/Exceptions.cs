﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    #region BO.BusLine
    [Serializable]
    public class BusLineNotFound : Exception
    {
        public int LineNumber;
        public BusLineNotFound(string message, int lineNumber) : base(message) => LineNumber = lineNumber;
        public BusLineNotFound(string message, int lineNumber, Exception innerEX) : base(message, innerEX) => LineNumber = lineNumber;
    }
    [Serializable]
    public class BusLineExists : Exception
    {
        public int LineNumber;
        public BusLineExists(string message, int lineNumber) : base(message) => LineNumber = lineNumber;
        public BusLineExists(string message, int lineNumber, Exception innerEX) : base(message, innerEX) => LineNumber = lineNumber;
    }
    #endregion

    #region BO.Station
    [Serializable]
    public class StationNotFound : Exception
    {
        public int Code;
        public StationNotFound(string message, int code) : base(message) => Code = code;
        public StationNotFound(string message, int code, Exception innerEX) : base(message, innerEX) => Code = code;
    }
    [Serializable]
    public class StationExists : Exception
    {
        public int Code;
        public StationExists(string message, int code) : base(message) => Code = code;
        public StationExists(string message, int code, Exception innerEX) : base(message, innerEX) => Code = code;
    }
    #endregion

    #region BO.User
    [Serializable]
    public class UserNotFound : Exception
    {
        public string Name;
        public UserNotFound(string message, string name) : base(message) => Name = name;
        public UserNotFound(string message, string name, Exception innerEX) : base(message, innerEX) => Name = name;
    }
    [Serializable]
    public class UserExists : Exception
    {
        public string Name;
        public UserExists(string message, string name) : base(message) => Name = name;
        public UserExists(string message, string name, Exception innerEX) : base(message, innerEX) => Name = name;
    }
    #endregion

    [Serializable]
    public class MissingData : Exception
    {
        public MissingData (string message): base(message) { }
        public MissingData (string message, Exception ex):base(message, ex) { }
    }
}
