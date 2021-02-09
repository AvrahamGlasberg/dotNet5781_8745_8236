using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using BLAPI;
using DLAPI;
using System.Threading;
using System.ComponentModel;
using System.Diagnostics;

namespace BL
{
    /// <summary>
    /// BL implementation
    /// </summary>
    class BLImp : IBL
    {
        #region singelton
        static readonly BLImp instance = new BLImp();
        static BLImp() { }// static ctor to ensure instance init is done just before first usage
        BLImp() { } // default => private
        public static BLImp Instance { get => instance; }// The public Instance property to use
        #endregion

        /// <summary>
        /// DL implication object
        /// </summary>
        IDL dl = DLFactory.GetDL();

        #region Simulator
        /// <summary>
        /// Simulator state, true means the simulator is running
        /// </summary>
        internal volatile bool stopSim = false;
        public void StartSimulator(TimeSpan startTime, int rate, Action<TimeSpan> func)//IBL function
        {
            //starts the clock
            Clock.Instance.Rate = rate;
            Clock.Instance.ClockObserver += func;
            stopSim = false;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Restart();

            //starts the trips launcher
            Thread tripsLauncher = new Thread(TravelLauncher.Instance.StartLaunch);
            tripsLauncher.Start();

            //updates the time (the clock)
            while (!stopSim)
            {
                TimeSpan t = startTime + new TimeSpan(rate * stopWatch.ElapsedTicks);
                Clock.Instance.Time = new TimeSpan(t.Hours, t.Minutes, t.Seconds);
                Thread.Sleep(100);
            }
            //simulation is over
            if (tripsLauncher.IsAlive)
                tripsLauncher.Interrupt();
        }
        /// <summary>
        /// Get all the line trips
        /// </summary>
        /// <returns>List of all the line trips</returns>
        internal List<BO.LineTrip> GetAllLineTrips()
        {
            return (from item in dl.GetAllLineTripsBy(s => true)
                   select DOLineTripToBOLineTrip(item)).ToList();
        }
        public void SetStationPanel(int station, Action<BO.LineTiming> updateBus)//IBL function
        {
            TravelLauncher.Instance.stationInWatch = station;
            TravelLauncher.Instance.StationObserver += updateBus;
        }
        public void StopSimulator()//IBL function
        {
            stopSim = true;
        }
        #endregion

        #region BO.BusLine
        public void UpdateTimeAndDis(BO.LineStation first, BO.LineStation second)//IBL function
        {
            try
            {
                dl.UpdateAdjacentStation(new DO.AdjacentStation()
                {
                    Station1 = first.Code, 
                    Station2 = second.Code, 
                    Distance = (Double)first.DistanceToNext,
                    Time = (TimeSpan)first.TimeToNext
                });
            }
            catch(DO.AdjacentStationExceptions ex)//exception from dl
            {
                throw new BO.MissingData(string.Format("Missing information about the stations {0} and {1} to update!", ex.Station1, ex.Station2), ex);
            }
        }
        public void AddLineStationToBusLine(BO.BusLine busLine, BO.Station station, int index)//IBL function
        {
            try
            {
                var stations = busLine.LineStations.ToList();
                var DOLine = dl.GetLine(busLine.DOLineId);
                DO.LineStation newLineStation = new DO.LineStation()
                {
                    LineId = busLine.DOLineId,
                    LineStationIndex = index,
                    Station = station.Code,
                    PrevStation = null,
                    NextStation = null
                };
                //update all the stataion's indexes
                for (int i = index; i < stations.Count; i++)
                {
                    DO.LineStation DOLineStation = dl.GetLineStation(busLine.DOLineId, stations[i].Code);
                    DOLineStation.LineStationIndex += 1;
                    dl.UpdateLineStation(DOLineStation);
                }
                //update previous station
                if (index != 0)
                {
                    newLineStation.PrevStation = stations[index - 1].Code;
                    DO.LineStation prev = dl.GetLineStation(busLine.DOLineId, stations[index - 1].Code);
                    prev.NextStation = station.Code;
                    dl.UpdateLineStation(prev);

                    double dis = CalcDis(station.Code, prev.Station);
                    TimeSpan time = CalcTime(dis);
                    dl.AddAdjacentStation(new DO.AdjacentStation()
                    {
                        Station1 = station.Code,
                        Station2 = prev.Station,
                        Distance = dis,
                        Time = time
                    });
                }
                //update next station
                if (index != stations.Count)
                {
                    newLineStation.NextStation = stations[index].Code;
                    DO.LineStation next = dl.GetLineStation(busLine.DOLineId, stations[index].Code);
                    next.PrevStation = station.Code;
                    dl.UpdateLineStation(next);

                    double dis = CalcDis(station.Code, next.Station);
                    TimeSpan time = CalcTime(dis);
                    dl.AddAdjacentStation(new DO.AdjacentStation()
                    {
                        Station1 = station.Code,
                        Station2 = next.Station,
                        Distance = dis,
                        Time = time
                    });
                }
                dl.AddLineStation(newLineStation);

                //adding station in DL.
                DOLine.FirstStation = index == 0 ? station.Code : DOLine.FirstStation;
                DOLine.LastStation = index == stations.Count ? station.Code : DOLine.LastStation;
                dl.UpdateLine(DOLine);
            }
            catch(DO.LineStationExceptions ex)//dl exception
            {
                throw new BO.MissingData(string.Format("Could not found {0} line station", ex.Station), ex);
            }
        }
        public BO.BusLine GetUpdatedBOBusLine(int dolineId)//IBL function
        {
            try
            {
                return LineDOToBusLineBO(dl.GetLine(dolineId));
            }
            catch(DO.LineExceptions ex)//DL exception
            {
                throw new BO.BusLineNotFound("Busline not found! sendind his ID", ex.Id, ex);
            }
        }
        public void UpdateBusLineArea(BO.BusLine busLine)//IBL function
        {
            try
            {
                DO.Line line = dl.GetLine(busLine.DOLineId);
                line.Area = (DO.Areas)(int)busLine.Area;
                dl.UpdateLine(line);
            }
            catch(DO.LineExceptions ex)//DL exception
            {
                throw new BO.BusLineNotFound("Could not update bus line ", ex.Id, ex);
            }
        }
        public void AddBusLine(BO.BusLine busLine)//IBL function
        {
            List<BO.LineStation> stations = busLine.LineStations.ToList();
            //checking if there is another bus line with that first&end station
            foreach (var line in dl.GetAllLines())
            {
                if (line.Code == busLine.LineNumber && line.FirstStation == stations.First<BO.LineStation>().Code && line.LastStation == stations.Last<BO.LineStation>().Code)
                    throw new BO.BusLineExists("Line with that last and start stations exists!", busLine.LineNumber);
            }

            //adding line to dl
            DO.Line newLine = new DO.Line()
            {
                Id = DO.Config.LineId,
                Code = busLine.LineNumber,
                Area = (DO.Areas)(int)busLine.Area,
                FirstStation = stations.First<BO.LineStation>().Code,
                LastStation = stations.Last<BO.LineStation>().Code
            };
            dl.AddLine(newLine);

            //adding all line stations
            for (int i = 0; i < stations.Count; i++)
            {
                dl.AddLineStation(new DO.LineStation()
                {
                    LineId = newLine.Id,
                    Station = stations[i].Code,
                    LineStationIndex = i,
                    PrevStation = i == 0 ? (int?)null : stations[i - 1].Code,
                    NextStation = i == stations.Count - 1 ? (int?)null : stations[i + 1].Code
                });
                //adding Adjacent Stations
                if (i != 0)
                {
                    double dis = CalcDis(stations[i].Code, stations[i - 1].Code);
                    TimeSpan time = CalcTime(dis);
                    dl.AddAdjacentStation(new DO.AdjacentStation()
                    {
                        Station1 = stations[i].Code,
                        Station2 = stations[i - 1].Code,
                        Distance = dis,
                        Time = time
                    });
                }
            }
        }
        public IEnumerable<BO.BusLine> GetAllBusLines()//IBL function
        {
            return from CurLine in dl.GetAllLines()
                   select LineDOToBusLineBO(CurLine);
        }
        /// <summary>
        /// Converts DO.Line into BO.BusLine
        /// </summary>
        /// <param name="line">DO.Line to convert</param>
        /// <returns>The converted BO.BusLine</returns>
        private BO.BusLine LineDOToBusLineBO(DO.Line line)
        {
            BO.BusLine newBusLine = new BO.BusLine() { DOLineId = line.Id, LineNumber = line.Code,
                Area = (BO.Areas)(int)line.Area};
            newBusLine.LineStations = GetAllBOLineStationsInDOLine(line.Id);
            newBusLine.EndStation = newBusLine.LineStations.Last<BO.LineStation>();
            return newBusLine;
        }
        /// <summary>
        /// Get all the bus line stations (of BO) in line.
        /// </summary>
        /// <param name="DOLineId">The line's ID</param>
        /// <returns>Collection of all the bus line stations as BO.LineStation</returns>
        private IEnumerable<BO.LineStation> GetAllBOLineStationsInDOLine(int DOLineId)
        {
            try
            {
                var DOline = dl.GetLine(DOLineId);//the line
                var Linestations = dl.GetAllLineStations(DOLineId);//the stations
                IEnumerable<BO.LineStation> stations = from item1 in Linestations
                                                       let ind1 = item1.LineStationIndex
                                                       from item2 in Linestations
                                                       let ind2 = item2.LineStationIndex
                                                       where ind2 == ind1 + 1//two following stations
                                                       orderby ind1
                                                       select DOLineStationsToBoLineStation(item1, item2);
                IEnumerable<BO.LineStation> lastStation = from DOstation in Linestations
                                                          where DOstation.Station == DOline.LastStation
                                                          select DOLineStationsToBoLastLineStation(DOstation);
                return stations.Concat(lastStation);//all stations + last station

            }
            catch(DO.LineExceptions ex)//dl exception
            {
                throw new BO.BusLineNotFound("Line could be found!", ex.Id, ex);
            }
        }
        /// <summary>
        /// Converts DO.LineStation into last BO.LineStaion.
        /// time & distance to the next station will be null.
        /// </summary>
        /// <param name="DOLineStation">The original DO.LineStation</param>
        /// <returns>The converted BO.LineStation</returns>
        private BO.LineStation DOLineStationsToBoLastLineStation(DO.LineStation DOLineStation)
        {
            try
            {
                DO.Station station = dl.GetStation(DOLineStation.Station);
                return new BO.LineStation()
                {
                    Code = station.Code,
                    Name = station.Name,
                    DOLineId = DOLineStation.LineId,
                    DistanceToNext = null,
                    TimeToNext = null
                };
            }
            catch(DO.StationExceptions ex)//dl exception
            {
                throw new BO.StationNotFound("Station not found!", ex.Code, ex);
            }
        }
        /// <summary>
        /// Converts two DO.LineStation into the first of them as BO.LineStation
        /// </summary>
        /// <param name="FirstStation">The first station</param>
        /// <param name="NextStation">The second station</param>
        /// <returns>The first station as BO.LineStation with time & distance to the second station</returns>
        private BO.LineStation DOLineStationsToBoLineStation(DO.LineStation FirstStation, DO.LineStation NextStation)
        {
            try
            {
                DO.Station baseStation = dl.GetStation(FirstStation.Station);
                DO.AdjacentStation nearStations = dl.GetAdjacentStation(FirstStation.Station, NextStation.Station);
                return new BO.LineStation()
                {
                    Code = baseStation.Code,
                    Name = baseStation.Name,
                    DOLineId = NextStation.LineId,
                    DistanceToNext = nearStations.Distance,
                    TimeToNext = nearStations.Time
                };
            }
            catch(DO.StationExceptions ex)//dl exception
            {
                throw new BO.StationNotFound("Station not found!", ex.Code, ex);
            }
            catch(DO.AdjacentStationExceptions ex)//dl exception
            {
                throw new BO.MissingData(string.Format("Time and distance between {0} and {1} stations could not be found!", ex.Station1, ex.Station2), ex);
            }
        }
        public void DeleteBusLine(BO.BusLine line)//IBL function
        {
            try
            {
                dl.DeleteLine(line.DOLineId);
                dl.DeleteAlLineStationslBy(lineStation => lineStation.LineId == line.DOLineId);
            }
            catch(DO.LineExceptions ex)//dl exception
            {
                throw new BO.BusLineNotFound("Line could not be found!", ex.Id, ex);
            }
        }
        public void DeleteLineStation(BO.LineStation lineStation)//IBL function
        {
            try
            {
                DO.Line line;
                DO.LineStation DOlineStation;

                line = dl.GetLine(lineStation.DOLineId);
                DOlineStation = dl.GetLineStation(lineStation.DOLineId, lineStation.Code);


                if (dl.GetLineStation(line.Id, line.LastStation).LineStationIndex == 1)
                {
                    //delete DO Line
                    dl.DeleteLine(line.Id);
                    dl.DeleteAlLineStationslBy(station => station.LineId == line.Id);
                }
                else
                {
                    //delete DO.LineStation
                    dl.DeleteLineStation(lineStation.DOLineId, lineStation.Code);

                    //update all line stations indexes.
                    DO.LineStation tempLineStation = DOlineStation;

                    while (tempLineStation.Station != line.LastStation)
                    {
                        tempLineStation = dl.GetLineStation(line.Id, (int)tempLineStation.NextStation);
                        tempLineStation.LineStationIndex -= 1;
                        dl.UpdateLineStation(tempLineStation);
                    }

                    //update Line first/last startion
                    if (lineStation.Code == line.FirstStation)
                    {
                        //update line
                        line.FirstStation = (int)DOlineStation.NextStation;
                        dl.UpdateLine(line);

                        //update next station
                        tempLineStation = dl.GetLineStation(line.Id, (int)DOlineStation.NextStation);
                        tempLineStation.PrevStation = null;
                        dl.UpdateLineStation(tempLineStation);
                    }
                    else if (lineStation.Code == line.LastStation)
                    {
                        //update line
                        line.LastStation = (int)DOlineStation.PrevStation;
                        dl.UpdateLine(line);

                        //update next station
                        tempLineStation = dl.GetLineStation(line.Id, (int)DOlineStation.PrevStation);
                        tempLineStation.NextStation = null;
                        dl.UpdateLineStation(tempLineStation);
                    }
                    else
                    {
                        //update next station
                        tempLineStation = dl.GetLineStation(line.Id, (int)DOlineStation.NextStation);
                        tempLineStation.PrevStation = DOlineStation.PrevStation;
                        dl.UpdateLineStation(tempLineStation);

                        //update prev station
                        tempLineStation = dl.GetLineStation(line.Id, (int)DOlineStation.PrevStation);
                        tempLineStation.NextStation = DOlineStation.NextStation;
                        dl.UpdateLineStation(tempLineStation);

                        double dis = CalcDis((int)DOlineStation.PrevStation, (int)DOlineStation.NextStation);
                        TimeSpan t = CalcTime(dis);
                        dl.AddAdjacentStation(new DO.AdjacentStation()
                        {
                            Station1 = (int)DOlineStation.PrevStation,
                            Station2 = (int)DOlineStation.NextStation,
                            Distance = dis,
                            Time = t
                        });
                    }
                }
            }
            catch (DO.LineExceptions ex)
            {
                if (ex.IsExists)
                    throw new BO.BusLineExists("Line is already exists!", ex.Id, ex);
                else
                    throw new BO.BusLineNotFound("Bus line could not be found!", ex.Id, ex);
            }
            catch(DO.LineStationExceptions ex)
            {
                if (ex.IsExists)
                    throw new BO.StationExists("Line station already exists!", ex.Station, ex);
                else
                    throw new BO.StationNotFound("Line staton could not be found!", ex.Station, ex);
            }
        }
        public bool IsTwoStationsInLine(int DOLineId)//IBL function
        {
            try
            {
                DO.Line line = dl.GetLine(DOLineId);
                DO.LineStation lastStation = dl.GetLineStation(line.Id, line.LastStation);
                return lastStation.LineStationIndex == 1;
            }
            catch(DO.LineExceptions ex)//dl exception
            {
                throw new BO.BusLineNotFound("Line could not be found!", ex.Id, ex);
            }
        }
        #endregion

        #region BO.LineTrip
        public void AddLineTrip(BO.LineTrip lineTrip)//IBL function
        {
            try
            {
                dl.AddLineTrip(new DO.LineTrip()
                {
                    LineId = lineTrip.LineInTrip.DOLineId,
                    StartAt = lineTrip.StartAt,
                    Frequency = lineTrip.Frequency,
                    FinishAt = lineTrip.FinishAt
                });
            }
            catch(DO.LineTripExceptions ex)//dl exception
            {
                throw new BO.LineTripExists("This Line on this time already exists!", ex.LineNumber, ex.StartTime, ex);
            }
        }
        public void DeleteLineTrip(BO.LineTrip lineTrip)//IBL function
        {
            try
            {
                dl.DeleteLineTrip(lineTrip.LineInTrip.DOLineId, lineTrip.StartAt);
            }
            catch(DO.LineTripExceptions ex)//dl exception
            {
                throw new BO.LineTripNotFound("Trip could not be found to delete!", ex.LineNumber, ex.StartTime, ex);
            }
        }
        public IEnumerable<BO.LineTrip> GetAllLineTripsInLine(BO.BusLine busLine)//IBL function
        {
            return from DOlineTrip in dl.GetAllLineTripsBy(ltrip => ltrip.LineId == busLine.DOLineId)
                   orderby DOlineTrip.StartAt
                   select DOLineTripToBOLineTrip(DOlineTrip);
        }
        /// <summary>
        /// Converts DO.LineTrip into BO.LineTrip.
        /// </summary>
        /// <param name="DOLineTrip">The DO.LineTrip ro convert</param>
        /// <returns>The Converted BO.Line.trip</returns>
        private BO.LineTrip DOLineTripToBOLineTrip(DO.LineTrip DOLineTrip)
        {
            try
            {
                DO.Line line = dl.GetLine(DOLineTrip.LineId);
                return new BO.LineTrip()
                {
                    LineInTrip = LineDOToBusLineBO(line),
                    StartAt = DOLineTrip.StartAt,
                    Frequency = DOLineTrip.Frequency,
                    FinishAt = DOLineTrip.FinishAt
                };
            }
            catch(DO.LineExceptions ex)//dl exception
            {
                throw new BO.BusLineNotFound("Line could not be found!", ex.Id, ex);
            }
        }
        #endregion

        #region BO.BusStaion
        public IEnumerable<BO.Station> GetAllStationsNotInLine(int DOLineId)//IBL function
        {
            var baseStations = dl.GetAllStations();
            var lineStations = dl.GetAllLineStations(DOLineId).ToList();
            return from item1 in baseStations
                   where (lineStations.FirstOrDefault(st => st.Station == item1.Code) == null)
                   select DOStationToBOStation(item1);
        }
        /// <summary>
        /// Converts DO.Station into BO.Station
        /// </summary>
        /// <param name="st">The DO.Station to convert</param>
        /// <returns>The converted BO.Station</returns>
        private BO.Station DOStationToBOStation(DO.Station st)
        {
            return new BO.Station()
            {
                Code = st.Code,
                Name = st.Name
            };
        }
        public BO.LineStation StationToLineStation(BO.Station st)//IBL function
        {
            return new BO.LineStation()
            {
                Code = st.Code, 
                Name = st.Name, 
                DistanceToNext = null,
                TimeToNext = null
            };
        }
        public IEnumerable<BO.BusStation> GetAllBusStations()//IBL function
        {
            return from item in dl.GetAllStations()
                   orderby item.Code
                   select GetBusStation(item.Code);
        }
        public BO.BusStation GetBusStation(int code)//IBL function
        {
            try
            {
                DO.Station DOstation = dl.GetStation(code);
                BO.BusStation busStation = new BO.BusStation()
                {
                    Code = DOstation.Code,
                    Name = DOstation.Name,
                    Position = new GeoCoordinate(DOstation.Latitude, DOstation.Longitude)
                };
                busStation.LinesInstation = from line in dl.GetAllLines()
                                            from lineStation in dl.GetAllLineStations(line.Id)
                                            where lineStation.Station == code
                                            select DOLineToBOLine(line);
                return busStation;
            }
            catch(DO.StationExceptions ex)//dl exception
            {
                throw new BO.StationNotFound("Station not Found!", ex.Code, ex);
            }
        }
        /// <summary>
        /// Converts DO.Line into BO.Line
        /// </summary>
        /// <param name="line">The DO.Line to convert</param>
        /// <returns>The converted BO.Line</returns>
        private BO.Line DOLineToBOLine(DO.Line line)
        {
            try
            {
                DO.Station lastStation = dl.GetStation(line.LastStation);
                return new BO.Line()
                {
                    DOLineId = line.Id,
                    LineNumber = line.Code,
                    EndStation = new BO.Station()
                    {
                        Code = lastStation.Code,
                        Name = lastStation.Name
                    }
                };
            }
            catch(DO.StationExceptions ex)//dl exception
            {
                throw new BO.StationNotFound("Station not Found!", ex.Code, ex);
            }
        }
        public void DeleteBusStation(BO.BusStation busStation)//IBL function
        {
            //delete all do line stations 
            //need to convert to list in order to change and delete the line stations.
            var DOLineStations = dl.GetAllLineStationsBy(lineStation => lineStation.Station == busStation.Code).ToList();
            foreach(var DOLineStation in DOLineStations)
            {
                BO.LineStation tempBOLineStation = new BO.LineStation()
                {
                    Code = DOLineStation.Station,
                    //Name - not importanat
                    DOLineId = DOLineStation.LineId
                    //Distance- not important
                    //time - not important
                };
                DeleteLineStation(tempBOLineStation);
            }
            try
            {
                //delete DO.station
                dl.DeleteStation(busStation.Code);
            }
            catch(DO.StationExceptions ex)//dl exception
            {
                throw new BO.StationNotFound("Station not Found!", ex.Code, ex);
            }
        }
        public void UpdateBusStation(BO.BusStation busStation)//IBL function
        {
            try
            {
                dl.UpdateStation(new DO.Station()
                {
                    Code = busStation.Code,
                    Name = busStation.Name,
                    Latitude = busStation.Position.Latitude,
                    Longitude = busStation.Position.Longitude
                });
            }
            catch(DO.StationExceptions ex)//dl exception
            {
                throw new BO.StationNotFound("Station to update not found!", ex.Code, ex);
            }
        }
        public void AddBusStation(BO.BusStation busStation)//IBL function
        {
            try
            {
                dl.AddStation(
                    new DO.Station()
                    {
                        Code = busStation.Code,
                        Name = busStation.Name, 
                        Latitude = busStation.Position.Latitude, 
                        Longitude = busStation.Position.Longitude
                    }
                    );
            }
            catch(DO.StationExceptions ex)//dl exception
            {
                throw new BO.StationExists("Station already exists!", ex.Code, ex);
            }
        }
        #endregion

        #region BO.Bus
        public void AddBus(BO.Bus bus)//IBL function
        {
            try
            {
                dl.AddBus(new DO.Bus()
                {
                    LicenseNum = bus.LicenseNum,
                    FromDate = bus.FromDate,
                    LastTreatmentDate = bus.LastTreatmentDate,
                    TripSinceTreatment = bus.TripSinceTreatment,
                    TotalTrip = bus.TotalTrip,
                    BusStatus = (DO.Status)(int)bus.BusStatus,
                    FuelRemain = bus.FuelRemain
                });
            }
            catch(DO.BusExceptions ex)//dl exception
            {
                throw new BO.BusExists("Bus is already exists!", ex.License, ex);
            }
        }
        public IEnumerable<BO.Bus> GetAllBuses()//IBL function
        {
            return from bus in dl.GettAllBuses()
                   orderby bus.FromDate
                   select DOBusToBOBus(bus);
        }
        /// <summary>
        /// Converts DO.Bus into BO.Bus
        /// </summary>
        /// <param name="bus">The DO.Bus to convert</param>
        /// <returns>The converted BO.Bus</returns>
        private BO.Bus DOBusToBOBus(DO.Bus bus)
        {
            return new BO.Bus()
            {
                 LicenseNum = bus.LicenseNum,
                 FromDate = bus.FromDate,
                 LastTreatmentDate = bus.LastTreatmentDate,
                 TotalTrip = bus.TotalTrip,
                 TripSinceTreatment = bus.TripSinceTreatment,
                 FuelRemain = bus.FuelRemain,
                 BusStatus = (BO.Status)(int)bus.BusStatus
            };

        }
        public void Refuel(BO.Bus bus)//IBL function
        {
            try
            {
                DO.Bus DObus = dl.GetBus(bus.LicenseNum);
                DObus.FuelRemain = 400;
                dl.UpdateBus(DObus);
            }
            catch(DO.BusExceptions ex)//dl exception
            {
                throw new BO.BusNotFound("Bus could not be found for refueling!", ex.License, ex);
            }
        }
        public void Treatment(BO.Bus bus)//IBL function
        {
            try
            {
                DO.Bus DObus = dl.GetBus(bus.LicenseNum);
                DObus.LastTreatmentDate = DateTime.Now;
                DObus.TripSinceTreatment = 0;
                dl.UpdateBus(DObus);
            }
            catch (DO.BusExceptions ex)//dl exception
            {
                throw new BO.BusNotFound("Bus could not be found for treatment!", ex.License, ex);
            }
        }
        public void DeleteBus(BO.Bus bus)//IBL function
        {
            try
            {
                dl.DeleteBus(bus.LicenseNum);
            }
            catch(DO.BusExceptions ex)//dl exception
            {
                throw new BO.BusNotFound("Bus was not found to selete!", ex.License, ex);
            }
        }
        #endregion

        #region BO.User
        public BO.User GetUser(string userName)//IBL function
        {
            try
            {
                DO.User user = dl.GetUser(userName);
                return new BO.User()
                {
                    UserName = user.UserName,
                    Password = user.Password,
                    Admin = user.Admin, 
                    Cash = user.Cash
                };
            }
            catch(DO.UserExceptions ex)//dl exception
            {
                 throw new BO.UserNotFound("User Not Found!", ex.Name, ex);
            }
        }
        public void AddUser(BO.User user)//IBL function
        {
            try
            {
                dl.AddUser(new DO.User()
                {
                    UserName = user.UserName,
                    Password = user.Password,
                    Admin = user.Admin, 
                    Cash = user.Cash
                });
            }
            catch(DO.UserExceptions ex)//dl exception
            {
                throw new BO.UserExists("User Already Exists.", ex.Name, ex);
            }
        }
        public int ClosestStationIndex(List<BO.BusStation> stations, GeoCoordinate coordinate)//IBL function
        {
            if (stations.Count == 0)
                throw new BO.MissingData("Not enough stations!");
            var sorted = stations.OrderBy(st => st.Position.GetDistanceTo(coordinate));
            return stations.FindIndex(st => st.Code == sorted.First().Code);
        }
        public IEnumerable<BO.UserLineTrip> GetUserLineTrips(BO.BusStation firstStation, BO.BusStation lastStation)//IBL function
        {
            List<BO.UserLineTrip> lines = new List<BO.UserLineTrip>();
            foreach(var line in dl.GetAllLines())
            {
                var stations = GetAllBOLineStationsInDOLine(line.Id).ToList();
                int ind1 = stations.FindIndex(st => st.Code == firstStation.Code);
                int ind2 = stations.FindIndex(st => st.Code == lastStation.Code);
                if (ind1 != -1 && ind2 > ind1)
                {
                    double p = 0;
                    for (int i = ind1; i < ind2; i++)
                        p += (double)stations[i].DistanceToNext * 2;//2$ to km
                    lines.Add(new BO.UserLineTrip() { Code = line.Code, Price = p });  
                }
            }
            return lines;
        }
        public void UserTravel(BO.User user, double price)//IBL function
        {
            if (user.Cash < price)
                throw new BO.NotEnoughMoney("You don't have enough money for this travel!", price - user.Cash);
            try
            {
                var temp = dl.GetUser(user.UserName);
                temp.Cash -= price;
                dl.UpdateUser(temp);
            }
            catch(DO.UserExceptions ex)//dl exception
            {
                throw new BO.MissingData("User not Found!", ex);
            }
        }
        public void AddCash(BO.User user, double cash)//IBL function
        {
            try
            {
                var temp = dl.GetUser(user.UserName);
                temp.Cash += cash;
                dl.UpdateUser(temp);
            }
            catch (DO.UserExceptions ex)//dl exception
            {
                throw new BO.MissingData("User not Found!", ex);
            }
        }
        #endregion

        #region Time & Distance
        /// <summary>
        /// This function calculates the distance between 2 stations.
        /// The calculation is by air distance time 1-1.5 to make it real
        /// </summary>
        /// <param name="StCode1">The first station's code</param>
        /// <param name="StCode2">The second station's code</param>
        /// <returns>The distance between the stations in Km</returns>
        private double CalcDis(int StCode1, int StCode2)
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            DO.Station st1, st2;
            double dis;
            try
            {
                st1 = dl.GetStation(StCode1);
                st2 = dl.GetStation(StCode2);

                GeoCoordinate point1 = new GeoCoordinate(st1.Latitude, st1.Longitude);
                GeoCoordinate point2 = new GeoCoordinate(st2.Latitude, st2.Longitude);
                dis = point1.GetDistanceTo(point2) / 1000;
                dis *= rand.NextDouble() / 2 + 1; // real. random between 1 and 1.5
                return dis;
            }
            catch (DO.StationExceptions ex)// dl exception
            {
                throw new BO.StationNotFound("Station was not found to calculate distance!", ex.Code, ex);
            }
        }
        /// <summary>
        /// Calculate the time to travel a distance.
        /// The calculation is with speed between 20-60 Km per hour.
        /// </summary>
        /// <param name="distance">The distance in Km</param>
        /// <returns>The time to drive the distance</returns>
        private TimeSpan CalcTime(double distance)
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            int speed = rand.Next(20, 60); // km/h
            double time = distance / speed; // h
            int h = (int)time;
            int m = (int)(time * 60 - h * 60);
            int s = (int)(time * 360) % 60;
            return new TimeSpan(h, m, s);
        }
        #endregion

    }
}
