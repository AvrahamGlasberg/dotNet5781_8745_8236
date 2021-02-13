using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BL
{
    class TravelLauncher
    {
        #region singelton
        static readonly TravelLauncher instance = new TravelLauncher();
        static TravelLauncher() { }// static ctor to ensure instance init is done just before first usage
        TravelLauncher() { } // default => private
        public static TravelLauncher Instance { get => instance; }// The public Instance property to use
        #endregion

        /// <summary>
        /// The station in watch's code
        /// </summary>
        internal int stationInWatch = -1;
        /// <summary>
        /// Random varieble
        /// </summary>
        Random rand = new Random(DateTime.Now.Millisecond);
        /// <summary>
        /// Observer's action, private
        /// </summary>
        private event Action<BO.LineTiming> stationObserver;
        /// <summary>
        /// Observer's action
        /// </summary>
        internal event Action<BO.LineTiming> StationObserver { add { stationObserver = value; } remove { stationObserver -= value; } }

        /// <summary>
        /// The function launch all line trips as long the simulator is running. 
        /// When the line trips ends the function 'sleeps' until the next day and starts over
        /// </summary>
        internal void StartLaunch()
        {
            List<Thread> linesThreads = new List<Thread>();//list of all threads - for interrupts

            try
            {
                //current time
                TimeSpan curTime = Clock.Instance.Time;

                //list of all travels
                List<BO.LineTrip> allTravels = new List<BO.LineTrip>();
                foreach (var trip in BLImp.Instance.GetAllLineTrips())
                    for (TimeSpan t = trip.StartAt; t <= trip.FinishAt && trip.Frequency != TimeSpan.Zero; t += trip.Frequency)
                        allTravels.Add(new BO.LineTrip() { StartAt = t, LineInTrip = trip.LineInTrip });
                for (int i = 0; i < allTravels.Count; i++)
                    if (allTravels[i].StartAt < curTime)
                        allTravels[i].StartAt += new TimeSpan(24, 0, 0);
                allTravels = allTravels.OrderBy(trip => trip.StartAt).ToList();

                if (allTravels.Count > 0)//if there is travels
                {
                    TimeSpan time = allTravels[0].StartAt - curTime;
                    int timeToSleep = 0;
                    //if first travel is tommorow
                    if (time < TimeSpan.Zero)
                        time += new TimeSpan(24, 0, 0);

                    //sleeping time
                    timeToSleep = time.Hours * 360 * 1000;
                    timeToSleep += time.Minutes * 60 * 1000;
                    timeToSleep += time.Seconds * 1000;
                    timeToSleep /= Clock.Instance.Rate;
                    System.Threading.Thread.Sleep(timeToSleep);
                    for (int i = 0; !BLImp.Instance.stopSim; i = (i + 1) % allTravels.Count)
                    {
                        curTime = Clock.Instance.Time;//current time

                        //The trip's function arguments
                        var allLineStations = allTravels[i].LineInTrip.LineStations.ToList();
                        var number = allTravels[i].LineInTrip.LineNumber;
                        var id = BO.Config.LineOnTripId;
                        
                        //new trip
                        Thread trip = new Thread(() => Trip(number, allLineStations, id));
                        linesThreads.Add(trip);
                        trip.Start();

                        //sleeps until next trip
                        int nextInd = (i + 1) % allTravels.Count;
                        time = allTravels[nextInd].StartAt - curTime;
                        timeToSleep = 0;

                        if (nextInd == 0)
                            time += new TimeSpan(24, 0, 0);
                        else if (time < TimeSpan.Zero)//means that time of computer runtime cause to miss a line launch 
                            time = TimeSpan.Zero;
                        //
                        timeToSleep = time.Hours * 360 * 1000;
                        timeToSleep += time.Minutes * 60 * 1000;
                        timeToSleep += time.Seconds * 1000;
                        timeToSleep /= Clock.Instance.Rate;
                        System.Threading.Thread.Sleep(timeToSleep);
                    }
                }

            }
            catch (ThreadInterruptedException)//the thread is interrupted - simulator is finished
            {
                //interrupts all still alive trips's threads
                foreach (var l in linesThreads)
                    if (l.IsAlive)
                        l.Interrupt();
            }
        }
        /// <summary>
        /// The function calls the observer's function
        /// </summary>
        /// <param name="lineTiming">The line timing to send to the observer</param>
        internal void UpdateStation(BO.LineTiming lineTiming)
        {
            stationObserver.Invoke(lineTiming);
        }
        /// <summary>
        /// Trip of line. the function sleeps between stations as 'driving' and updates the observer in every station
        /// </summary>
        /// <param name="lineNumber">The line number</param>
        /// <param name="allStations">List of all stations</param>
        /// <param name="id">Trip's ID</param>
        private void Trip(int lineNumber, List<BO.LineStation> allStations, int id)
        {
            try
            {
                BO.LineStation curStation = allStations.First<BO.LineStation>();
                //travel in all stations
                for (int j = 0; j < allStations.Count && !BLImp.Instance.stopSim; j++)
                {
                    var station = allStations[j];
                    TimeSpan time = TimeSpan.Zero;
                    for (int i = allStations.FindIndex(st => st.Code == station.Code); i < allStations.Count; i++)
                    {
                        if (allStations[i].Code == stationInWatch)
                        {
                            //update the observer
                            UpdateStation(new BO.LineTiming()
                            {
                                Id = id,
                                LineNumber = lineNumber,
                                Destination = allStations.Last().Name,
                                Time = time
                            });
                        }
                        if (allStations[i].Code != allStations.Last().Code)
                            time += (TimeSpan)allStations[i].TimeToNext;
                    }
                    //sleeps until next station
                    if (station.Code != allStations.Last().Code)
                    {
                        int timeToSleep = ((TimeSpan)station.TimeToNext).Hours * 360 * 1000;
                        timeToSleep += ((TimeSpan)station.TimeToNext).Minutes * 60 * 1000;
                        timeToSleep += ((TimeSpan)station.TimeToNext).Seconds * 1000;
                        timeToSleep /= Clock.Instance.Rate;
                        timeToSleep *= (int)((double)rand.Next(90, 200) / 100);//real time slowing down or speeding up, 90%-200%.
                        System.Threading.Thread.Sleep(timeToSleep);
                    }
                }
            }
            catch(ThreadInterruptedException)//thread is interrupted
            {
                //stops the thread - no need to do something.
            }
        }
    }
}
