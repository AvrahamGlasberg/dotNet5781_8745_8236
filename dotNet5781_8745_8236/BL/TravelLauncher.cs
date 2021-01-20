using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        internal int stationInWatch = -1;
        private event Action<BO.LineTiming> stationObserver;
        internal event Action<BO.LineTiming> StationObserver { add { stationObserver = value; } remove { stationObserver -= value; } }
        internal void StartLaunch()
        {
            TimeSpan curTime = Clock.Instance.Time;
            List<BO.LineTrip> allTravels = new List<BO.LineTrip>();
            foreach(var trip in BLImp.Instance.GetAllLineTrips())
                for (TimeSpan t = trip.StartAt; t <= trip.FinishAt && trip.Frequency != TimeSpan.Zero; t += trip.Frequency)
                    allTravels.Add(new BO.LineTrip() { StartAt = t, LineInTrip = trip.LineInTrip});
            allTravels = allTravels.OrderBy(trip => trip.StartAt).ToList();

            TimeSpan time = allTravels[0].StartAt - curTime;
            int timeToSleep = 0;
            if (time < TimeSpan.Zero)
                time += new TimeSpan(24, 0, 0);
            timeToSleep = time.Hours * 360 * 1000;
            timeToSleep += time.Minutes * 60 * 1000;
            timeToSleep += time.Seconds * 1000;
            timeToSleep /= Clock.Instance.Rate;
            System.Threading.Thread.Sleep(timeToSleep);
            for (int i = 0; !BLImp.Instance.stopSim; i = (i + 1) % allTravels.Count)
            {
                //xml problems
                allTravels[i].LineInTrip.LineStations = (IEnumerable<BO.LineStation>)(allTravels[i].LineInTrip.LineStations.ToList());
                Thread trip = new Thread(()=>Trip(allTravels[i].LineInTrip, BO.Config.LineOnTripId));
                trip.Start();

                int nextInd = (i + 1) % allTravels.Count;
                time = allTravels[nextInd].StartAt - curTime;
                timeToSleep = 0;
                if (time < TimeSpan.Zero)
                    time += new TimeSpan(24, 0, 0);
                timeToSleep = time.Hours * 360 * 1000;
                timeToSleep += time.Minutes * 60 * 1000;
                timeToSleep += time.Seconds * 1000;
                timeToSleep /= Clock.Instance.Rate;
                System.Threading.Thread.Sleep(timeToSleep);
            }
        }
        internal void UpdateStation(BO.LineTiming lineTiming)
        {
            stationObserver.Invoke(lineTiming);
        }
        private void Trip(BO.BusLine busLine, int id)
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            BO.LineStation curStation = busLine.LineStations.First<BO.LineStation>();
            List<BO.LineStation> allStations = busLine.LineStations.ToList();
            for (int j = 0; j < allStations.Count && !BLImp.Instance.stopSim;j++)
            {
                var station = allStations[j];
                TimeSpan time = TimeSpan.Zero;
                for (int i = allStations.FindIndex(st=> st.Code == station.Code); i < allStations.Count; i++)
                {
                    if(allStations[i].Code == stationInWatch)
                    {
                        UpdateStation(new BO.LineTiming() {Id = id,
                            LineNumber = busLine.LineNumber,
                            Destination =  busLine.EndStation.Name, 
                            Time = time});
                    }
                    if(allStations[i].Code != busLine.EndStation.Code)
                         time += (TimeSpan)allStations[i].TimeToNext;
                }
                if (station.Code != busLine.EndStation.Code)
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
    }
}
