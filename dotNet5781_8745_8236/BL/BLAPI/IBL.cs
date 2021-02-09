using System;
using System.Collections.Generic;
using System.Device.Location;

namespace BLAPI
{
    public interface IBL
    {
        #region BO.BusLine
        /// <summary>
        /// Adding new busline.
        /// </summary>
        /// <param name="busLine">New bus line to add</param>
        void AddBusLine(BO.BusLine busLine);
        /// <summary>
        /// Get updated BO bus line from DO lineid.
        /// </summary>
        /// <param name="dolineId">ID of the bus to get</param>
        /// <returns>Updated BO.Busline with this id</returns>
        BO.BusLine GetUpdatedBOBusLine(int dolineId);
        /// <summary>
        /// Update the bus line
        /// </summary>
        /// <param name="busLine">Bus line with new area to update</param>
        void UpdateBusLineArea(BO.BusLine busLine);
        /// <summary>
        /// Get all the bus line in the system
        /// </summary>
        /// <returns>Ienumerable of all the bus lines.</returns>
        IEnumerable<BO.BusLine> GetAllBusLines();
        /// <summary>
        /// Deleting the bus line
        /// </summary>
        /// <param name="line">Bus line to delete</param>
        void DeleteBusLine(BO.BusLine line);
        /// <summary>
        /// Deleting line station
        /// </summary>
        /// <param name="lineStation">line station to delete</param>
        void DeleteLineStation(BO.LineStation lineStation);
        /// <summary>
        /// Checks if the line only have 2 stations
        /// </summary>
        /// <param name="DOLineId">ID of the line</param>
        /// <returns>True if there is only 2 stations in that line, false otherwise.</returns>
        bool IsTwoStationsInLine(int DOLineId);
        /// <summary>
        /// Adding new bus line station to existing line.
        /// </summary>
        /// <param name="busLine">The existing bus line</param>
        /// <param name="station">The new station</param>
        /// <param name="index">Index of the new station in the line's route.</param>
        void AddLineStationToBusLine(BO.BusLine busLine, BO.Station station, int index);
        /// <summary>
        /// Updateing time and distance between two stations
        /// </summary>
        /// <param name="first">The first station</param>
        /// <param name="second">The second station</param>
        void UpdateTimeAndDis(BO.LineStation first, BO.LineStation second);
        #endregion

        #region BO.LineTrip
        /// <summary>
        /// Adding new line trip
        /// </summary>
        /// <param name="lineTrip">The new line trip to add</param>
        void AddLineTrip(BO.LineTrip lineTrip);
        /// <summary>
        /// Deleting line trip
        /// </summary>
        /// <param name="lineTrip">line trip to delete</param>
        void DeleteLineTrip(BO.LineTrip lineTrip);
        /// <summary>
        /// Get all line trips in one bus line
        /// </summary>
        /// <param name="busLine">Bus line with the line trips</param>
        /// <returns></returns>
        IEnumerable<BO.LineTrip> GetAllLineTripsInLine(BO.BusLine busLine);
        #endregion

        #region BO.BusStation
        /// <summary>
        /// Get all bus stations exists
        /// </summary>
        /// <returns>Ienumerable of all bus stations exists</returns>
        IEnumerable<BO.BusStation> GetAllBusStations();
        /// <summary>
        /// Get bus station
        /// </summary>
        /// <param name="code">Code of bus station to get</param>
        /// <returns>The bus station with that code</returns>
        BO.BusStation GetBusStation(int code);
        /// <summary>
        /// Adding new bus station
        /// </summary>
        /// <param name="busStation">New bus station to add</param>
        void AddBusStation(BO.BusStation busStation);
        /// <summary>
        /// Deleting bus station
        /// </summary>
        /// <param name="busStation">Bus station to delete</param>
        void DeleteBusStation(BO.BusStation busStation);
        /// <summary>
        /// Updating bus station
        /// </summary>
        /// <param name="busStation">Bus station to update</param>
        void UpdateBusStation(BO.BusStation busStation);
        /// <summary>
        /// Convert station to line station (with only possible fields)
        /// </summary>
        /// <param name="st">Station to convert</param>
        /// <returns>The new line station</returns>
        BO.LineStation StationToLineStation(BO.Station st);
        /// <summary>
        /// Get all the line stations except those in one line 
        /// </summary>
        /// <param name="DOLineId">ID of the line</param>
        /// <returns>All the stations not in the line</returns>
        IEnumerable<BO.Station> GetAllStationsNotInLine(int DOLineId);
        #endregion

        #region BO.Bus
        /// <summary>
        /// Get all the buses exists
        /// </summary>
        /// <returns>All the buses</returns>
        IEnumerable<BO.Bus> GetAllBuses();
        /// <summary>
        /// Refuling the bus
        /// </summary>
        /// <param name="bus">Bus to refuel</param>
        void Refuel(BO.Bus bus);
        /// <summary>
        /// Treatment the bus
        /// </summary>
        /// <param name="bus">Bus to get treatment</param>
        void Treatment(BO.Bus bus);
        /// <summary>
        /// Adding new bus
        /// </summary>
        /// <param name="bus">New bus to add</param>
        void AddBus(BO.Bus bus);
        /// <summary>
        /// Deleting bus
        /// </summary>
        /// <param name="bus">Bus to delete</param>
        void DeleteBus(BO.Bus bus);
        #endregion

        #region BO.User
        /// <summary>
        /// Get user
        /// </summary>
        /// <param name="userName">Name of the user to get</param>
        /// <returns>The user with that name</returns>
        BO.User GetUser(string userName);
        /// <summary>
        /// Adding new user
        /// </summary>
        /// <param name="user">New user to add</param>
        void AddUser(BO.User user);
        /// <summary>
        /// Calculates the index of the station in list of stations that closest (geographically) to location.
        /// </summary>
        /// <param name="stations">The list of the stations</param>
        /// <param name="coordinate">The location</param>
        /// <returns>The index of the closest station</returns>
        int ClosestStationIndex(List<BO.BusStation> stations, GeoCoordinate coordinate);
        /// <summary>
        /// Get all possible trips in lines between two stations for the user
        /// </summary>
        /// <param name="firstStation">The first station</param>
        /// <param name="lastStation">The last station</param>
        /// <returns>Ienumerable of all possible user line trips</returns>
        IEnumerable<BO.UserLineTrip> GetUserLineTrips(BO.BusStation firstStation, BO.BusStation lastStation);
        /// <summary>
        /// User's new travel, 
        /// decreasing the user's money.
        /// </summary>
        /// <param name="user">User to travel</param>
        /// <param name="price">Cost of the travel</param>
        void UserTravel(BO.User user, double price);
        /// <summary>
        /// Adding cash to the user
        /// </summary>
        /// <param name="user">User to add cash to</param>
        /// <param name="cash">amount of cash to add</param>
        void AddCash(BO.User user, double cash);
        #endregion

        #region Simulator
        /// <summary>
        /// Starts the simulator.
        /// </summary>
        /// <param name="startTime">The starting time of the simulator</param>
        /// <param name="rate">Rate of the simulator (relative to real time)</param>
        /// <param name="func">Action for updates</param>
        void StartSimulator(TimeSpan startTime, int rate, Action<TimeSpan> func);
        /// <summary>
        /// Setting the station in watch
        /// </summary>
        /// <param name="station">The station in watch's code</param>
        /// <param name="updateBus">Action for updates</param>
        void SetStationPanel(int station, Action<BO.LineTiming> updateBus);
        /// <summary>
        /// Stops the simulator
        /// </summary>
        void StopSimulator();
        #endregion
    }
}

