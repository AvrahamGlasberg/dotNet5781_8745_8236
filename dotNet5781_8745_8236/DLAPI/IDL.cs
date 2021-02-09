using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
namespace DLAPI
{
    public interface IDL
    {

        #region AdjacentStation
        /// <summary>
        /// Adding new AdjacentStation
        /// </summary>
        /// <param name="adjacentStation">New AdjacentStation to add</param>
        void AddAdjacentStation(AdjacentStation adjacentStation);
        /// <summary>
        /// Get AdjacentStation by two stations's codes
        /// </summary>
        /// <param name="station1">First station's code</param>
        /// <param name="station2">Second station's code</param>
        /// <returns></returns>
        AdjacentStation GetAdjacentStation(int station1, int station2);
        /// <summary>
        /// Get all AdjacentStation that meets predicate condition
        /// </summary>
        /// <param name="predicate">The condition</param>
        /// <returns>Ienumerable of all AdjacentStation that meets the condition</returns>
        IEnumerable<DO.AdjacentStation> GetAdjacentStationsBy(Predicate<DO.AdjacentStation> predicate);
        /// <summary>
        /// Updates AdjacentStation
        /// </summary>
        /// <param name="newAdjacentStation">AdjacentStation to update</param>
        void UpdateAdjacentStation(AdjacentStation newAdjacentStation);
        /// <summary>
        /// Deleting AdjacentStation
        /// </summary>
        /// <param name="station1">First station's code</param>
        /// <param name="station2">Second station's code</param>
        void DeleteAdjacentStation(int station1, int station2);
        #endregion

        #region Bus
        /// <summary>
        /// Adding new bus
        /// </summary>
        /// <param name="bus">New bus to add</param>
        void AddBus(Bus bus);
        /// <summary>
        /// Get bus
        /// </summary>
        /// <param name="license">License of bus to get</param>
        /// <returns>The bus with that license</returns>
        Bus GetBus(int license);
        /// <summary>
        /// Get all buses
        /// </summary>
        /// <returns>Ienumerable of all buses</returns>
        IEnumerable<Bus> GettAllBuses();
        /// <summary>
        /// Updates a bus
        /// </summary>
        /// <param name="newBus">Bus to update</param>
        void UpdateBus(Bus newBus);
        /// <summary>
        /// Deleting a bus
        /// </summary>
        /// <param name="license">License of bus to delete</param>
        void DeleteBus(int license);
        #endregion

        //obselete
        #region BusOnTrip
        void AddBusOnTrip(BusOnTrip busOnTrip);
        BusOnTrip GetBusOnTrip(int license, int lineID, TimeSpan takeOff);
        void UpdateBusOnTrip(BusOnTrip busOnTrip);
        void DeleteBusOnTrip(int license, int lineID, TimeSpan takeOff);
        #endregion 

        #region Line
        /// <summary>
        /// Adding new Line
        /// </summary>
        /// <param name="line">New line to add</param>
        void AddLine(Line line);
        /// <summary>
        /// Get line
        /// </summary>
        /// <param name="id">ID of line to get</param>
        /// <returns>The Line with that ID</returns>
        Line GetLine(int id);
        /// <summary>
        /// Get all the lines
        /// </summary>
        /// <returns>Ienumerable of all lines</returns>
        IEnumerable<Line> GetAllLines();
        /// <summary>
        /// Updates a line
        /// </summary>
        /// <param name="newLine">Line to update</param>
        void UpdateLine(Line newLine);
        /// <summary>
        /// Deleting a line
        /// </summary>
        /// <param name="id">ID of line to delete</param>
        void DeleteLine(int id);
        #endregion

        #region LineStation
        /// <summary>
        /// Adding new line stations
        /// </summary>
        /// <param name="lineStation">New line station to add</param>
        void AddLineStation(LineStation lineStation);
        /// <summary>
        /// Get line station
        /// </summary>
        /// <param name="lineId">Line's ID</param>
        /// <param name="station">Station's code</param>
        /// <returns></returns>
        LineStation GetLineStation(int lineId, int station);
        /// <summary>
        /// Get all line stations in one line
        /// </summary>
        /// <param name="lineId">The line's ID</param>
        /// <returns>Ienumerable of all line stations in that line</returns>
        IEnumerable<LineStation> GetAllLineStations(int lineId);
        /// <summary>
        /// Get all line station that meets predicate's condition
        /// </summary>
        /// <param name="predicate">The condition</param>
        /// <returns>IEnumerable of line stations that meets the condition</returns>
        IEnumerable<LineStation> GetAllLineStationsBy(Predicate<DO.LineStation> predicate);
        /// <summary>
        /// Updates line station
        /// </summary>
        /// <param name="newLineStation">Line station to update</param>
        void UpdateLineStation(LineStation newLineStation);
        /// <summary>
        /// Deleting line stations
        /// </summary>
        /// <param name="lineId">Line's ID of line station to delete</param>
        /// <param name="station">Station's code of line station to delete</param>
        void DeleteLineStation(int lineId, int station);
        /// <summary>
        /// Delete all line station who meets predicate's condition
        /// </summary>
        /// <param name="predicate">The condition</param>
        void DeleteAlLineStationslBy(Predicate<DO.LineStation> predicate);
        #endregion

        #region LineTrip
        /// <summary>
        /// Adding new line trip
        /// </summary>
        /// <param name="lineTrip">New line trip to add</param>
        void AddLineTrip(LineTrip lineTrip);
        /// <summary>
        /// Get line trip
        /// </summary>
        /// <param name="lineId">Line trip's line's ID</param>
        /// <param name="start">Starting time</param>
        /// <returns>The line trip</returns>
        LineTrip GetLineTrip(int lineId, TimeSpan start);
        /// <summary>
        /// Get all line trip that meets predicate's condition
        /// </summary>
        /// <param name="predicate">The condition</param>
        /// <returns>IEnumerable of all line trips that meets the condition</returns>
        IEnumerable<DO.LineTrip> GetAllLineTripsBy(Predicate<DO.LineTrip> predicate);
        /// <summary>
        /// Updates line trip
        /// </summary>
        /// <param name="newLineTrip">Line trip to update</param>
        void UpdateLineTrip(LineTrip newLineTrip);
        /// <summary>
        /// Deleting line trip
        /// </summary>
        /// <param name="lineId">Line trip's Line's ID to delete</param>
        /// <param name="start">Line trip's to delete starting time</param>
        void DeleteLineTrip(int lineId, TimeSpan start);
        #endregion

        #region Station
        /// <summary>
        /// Adding new station
        /// </summary>
        /// <param name="station">New Station to add</param>
        void AddStation(Station station);
        /// <summary>
        /// Get station
        /// </summary>
        /// <param name="code">Station's code</param>
        /// <returns>The station with that code</returns>
        Station GetStation(int code);
        /// <summary>
        /// Get all stations
        /// </summary>
        /// <returns>IEnumerable of all stations</returns>
        IEnumerable<DO.Station> GetAllStations();
        /// <summary>
        /// Updates a station
        /// </summary>
        /// <param name="newStation">Station to update</param>
        void UpdateStation(Station newStation);
        /// <summary>
        /// Deleting a station
        /// </summary>
        /// <param name="code">Station's to delete code</param>
        void DeleteStation(int code);
        #endregion

        //obselete
        #region Trip
        //void AddTrip(Trip trip);
        //Trip GetTrip(int id);
        //void UpdateTrip(Trip newTrip);
        //void DeleteTrip(int id);
        #endregion

        #region User
        /// <summary>
        /// Adding user
        /// </summary>
        /// <param name="user">New user to add</param>
        void AddUser(User user);
        /// <summary>
        /// Get user
        /// </summary>
        /// <param name="name">The user's name</param>
        /// <returns>User with that name</returns>
        User GetUser(string name);
        /// <summary>
        /// Updates a user
        /// </summary>
        /// <param name="newUser">User to update</param>
        void UpdateUser(User newUser);
        /// <summary>
        /// Deleting a user
        /// </summary>
        /// <param name="name">User's to delete name</param>
        void DeleteUser(string name);
        #endregion
    }
}
