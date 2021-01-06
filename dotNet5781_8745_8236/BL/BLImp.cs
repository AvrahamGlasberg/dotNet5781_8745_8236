using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using BLAPI;
using DLAPI;
namespace BL
{
    class BLImp : IBL
    {
        IDL dl = DLFactory.GetDL();

        #region BO.BusLine
        public IEnumerable<BO.BusLine> GetAllBusLines()
        {
            return from CurLine in dl.GetAllLines()
                   select LineDOToBusLineBO(CurLine);
        }
        private BO.BusLine LineDOToBusLineBO(DO.Line line)
        {
            BO.BusLine newBusLine = new BO.BusLine() { DOLineId = line.Id, LineNumber = line.Code };
            newBusLine.LineStations = GetAllBOLineStationsInDOLine(line.Id);
            newBusLine.EndStation = newBusLine.LineStations.Last<BO.LineStation>();
            return newBusLine;
        }
        private IEnumerable<BO.LineStation> GetAllBOLineStationsInDOLine(int DOLineId)
        {
            
            var Linestations = dl.GetAllLineStations(DOLineId);
            IEnumerable<BO.LineStation> firstStation = from DOstation in Linestations
                                                       where DOstation.LineStationIndex == 0
                                                       select DOLineStationsToBoFirstLineStation(DOstation);
            IEnumerable<BO.LineStation> stations = from item1 in Linestations
                                                   let ind1 = item1.LineStationIndex
                                                   from item2 in Linestations
                                                   let ind2 = item2.LineStationIndex
                                                   where ind2 == ind1 + 1
                                                   orderby ind1 
                                                   select DOLineStationsToBoLineStation(item1, item2);
            return firstStation.Concat(stations);
        }
        private BO.LineStation DOLineStationsToBoFirstLineStation(DO.LineStation DOLineStation)
        {
            try
            {
                DO.Station station = dl.GetStation(DOLineStation.Station);
                return new BO.LineStation()
                {
                    Code = station.Code,
                    Name = station.Name,
                    DOLineId = DOLineStation.LineId,
                    DistanceFromPrev = null,
                    TimeFromPrev = null
                };
            }
            catch(DO.StationExceptions ex)
            {
                throw new BO.StationNotFound("Station not found!", DOLineStation.Station, ex);
            }
            
        }
        private BO.LineStation DOLineStationsToBoLineStation(DO.LineStation FirstStation, DO.LineStation NextStation)
        {
            try
            {
                DO.Station baseStation = dl.GetStation(NextStation.Station);
                DO.AdjacentStation nearStations = dl.GetAdjacentStation(FirstStation.Station, NextStation.Station);
                return new BO.LineStation()
                {
                    Code = baseStation.Code,
                    Name = baseStation.Name,
                    DOLineId = NextStation.LineId,
                    DistanceFromPrev = nearStations.Distance,
                    TimeFromPrev = nearStations.Time
                };
            }
            catch(DO.StationExceptions ex)
            {
                throw new BO.StationNotFound("Station not found!", NextStation.Station, ex);
            }
            catch(DO.AdjacentStationExceptions ex)
            {
                throw new BO.MissingData(string.Format("Time and distance between {0} and {1} stations could not be found!", FirstStation.Station, NextStation.Station), ex);
            }
        }
        public void DeleteBusLine(BO.BusLine line)
        {
            try
            {
                dl.DeleteLine(line.DOLineId);
                dl.DeleteAlLineStationslBy(lineStation => lineStation.LineId == line.DOLineId);
            }
            catch(DO.LineExceptions ex)
            {
                throw new BO.BusLineNotFound("Line could not be found!", line.LineNumber, ex);
            }
        }

        public void DeleteLineStation(BO.LineStation lineStation)
        {
            DO.Line line;
            DO.LineStation DOlineStation;

            try
            {
                line = dl.GetLine(lineStation.DOLineId);
                DOlineStation = dl.GetLineStation(lineStation.DOLineId, lineStation.Code);
            }
            catch(DO.LineExceptions ex)
            {
                throw new BO.BusLineNotFound("Line from station could not be found!", lineStation.Code, ex);
            }
            catch(DO.LineStationExceptions ex)
            {
                throw new BO.StationNotFound("Line Station could not be found!", lineStation.Code, ex);
            }

            try
            {
                //delete line if too short - less than 2 stations
                if (dl.GetLineStation(line.Id, line.LastStation).LineStationIndex == 1)
                {
                    try
                    {
                        //delete DO Line
                        dl.DeleteLine(line.Id);
                        dl.DeleteAlLineStationslBy(station => station.LineId == line.Id);
                    }
                    catch(DO.LineExceptions ex)
                    {
                        throw new BO.BusLineNotFound("Line could not be found!", line.Code, ex);
                    }
                }
                else
                {
                    try
                    {
                        //delete DO.LineStation
                        dl.DeleteLineStation(lineStation.DOLineId, lineStation.Code);
                    }
                    catch(DO.LineStationExceptions ex)
                    {
                        throw new BO.StationNotFound("Line station could not be found!", lineStation.Code, ex);
                    }

                    //update all line stations indexes.
                    DO.LineStation tempLineStation = DOlineStation;
                    while (tempLineStation.Station != line.LastStation)
                    {
                        try
                        {
                            tempLineStation = dl.GetLineStation(line.Id, (int)tempLineStation.NextStation);
                            tempLineStation.LineStationIndex -= 1;
                            dl.UpdateLineStation(tempLineStation);
                        }
                        catch(DO.LineStationExceptions ex)
                        {
                            throw new BO.StationNotFound("Line station could not be found!", (int)tempLineStation.NextStation, ex);
                        }
                    }
                    try
                    {
                        //update Line first/last startion
                        if (lineStation.Code == line.FirstStation)
                        {
                            //update line
                            line.FirstStation = (int)DOlineStation.NextStation;
                            dl.UpdateLine(line);

                            try
                            {
                                //update next station
                                tempLineStation = dl.GetLineStation(line.Id, (int)DOlineStation.NextStation);
                                tempLineStation.PrevStation = null;
                                dl.UpdateLineStation(tempLineStation);
                            }
                            catch(DO.LineStationExceptions ex)
                            {
                                throw new BO.StationNotFound("Line station could not be found!", (int)DOlineStation.NextStation, ex);
                            }
                        }
                        else if (lineStation.Code == line.LastStation)
                        {
                            //update line
                            line.LastStation = (int)DOlineStation.PrevStation;
                            dl.UpdateLine(line);

                            try
                            {
                                //update next station
                                tempLineStation = dl.GetLineStation(line.Id, (int)DOlineStation.PrevStation);
                                tempLineStation.NextStation = null;
                                dl.UpdateLineStation(tempLineStation);
                            }
                            catch (DO.LineStationExceptions ex)
                            {
                                throw new BO.StationNotFound("Line station could not be found!", (int)DOlineStation.PrevStation, ex);
                            }
                        }
                        else
                        {
                            try
                            {
                                //update next station
                                tempLineStation = dl.GetLineStation(line.Id, (int)DOlineStation.NextStation);
                                tempLineStation.PrevStation = DOlineStation.PrevStation;
                                dl.UpdateLineStation(tempLineStation);
                            }
                            catch (DO.LineStationExceptions ex)
                            {
                                throw new BO.StationNotFound("Line station could not be found!", (int)DOlineStation.NextStation, ex);
                            }

                            try
                            {
                                //update prev station
                                tempLineStation = dl.GetLineStation(line.Id, (int)DOlineStation.PrevStation);
                                tempLineStation.NextStation = DOlineStation.NextStation;
                                dl.UpdateLineStation(tempLineStation);
                            }
                            catch (DO.LineStationExceptions ex)
                            {
                                throw new BO.StationNotFound("Line station could not be found!", (int)DOlineStation.PrevStation, ex);
                            }

                            //update DO.adj stations
                            dl.AddAdjacentStation(new DO.AdjacentStation()
                            {
                                Station1 = (int)DOlineStation.PrevStation,
                                Station2 = (int)DOlineStation.NextStation,
                                Distance = 500,
                                Time = new TimeSpan(0, 10, 0),
                            });
                        }
                    }
                    catch(DO.LineExceptions ex)
                    {
                        throw new BO.BusLineNotFound("Line could not be found!", line.Code, ex);
                    }
                    
                }
            }
            catch(DO.LineStationExceptions ex)
            {
                throw new BO.StationNotFound("Line Station could not be found!", line.LastStation, ex);
            }
        }
        public bool IsTwoStationsInLine(int DOLineId)
        {
            try
            {
                DO.Line line = dl.GetLine(DOLineId);
                DO.LineStation lastStation = dl.GetLineStation(line.Id, line.LastStation);
                return lastStation.LineStationIndex == 1;
            }
            catch(DO.LineExceptions ex)
            {
                throw new BO.BusLineNotFound("Line could not be found!", DOLineId, ex);
            }
        }
        #endregion

        #region BO.BusStaion
        public IEnumerable<BO.BusStation> GetAllBusStations()
        {
            return from item in dl.GetAllStations()
                   select GetBusStation(item.Code);
        }
        public BO.BusStation GetBusStation(int code)
        {
            try
            {
                DO.Station DOstation = dl.GetStation(code);
                BO.BusStation busStation = new BO.BusStation()
                {
                    Code = DOstation.Code,
                    Name = DOstation.Name,
                    Location = new GeoCoordinate(DOstation.Latitude, DOstation.Longitude)
                };
                busStation.LinesInstation = from line in dl.GetAllLines()
                                            from lineStation in dl.GetAllLineStations(line.Id)
                                            where lineStation.Station == code
                                            select DOLineToBOLine(line);
                return busStation;
            }
            catch(DO.StationExceptions ex)
            {
                throw new BO.StationNotFound("Station not Found!", code, ex);
            }
        }
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
            catch(DO.StationExceptions ex)
            {
                throw new BO.StationNotFound("Station not Found!", line.LastStation, ex);
            }
        }

        public void DeleteBusStation(BO.BusStation busStation)
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
            catch(DO.StationExceptions ex)
            {
                throw new BO.StationNotFound("Station not Found!", busStation.Code, ex);
            }
            
        }
        #endregion

        #region BO.User
        public BO.User GetUser(string userName)
        {
            try
            {
                DO.User user = dl.GetUser(userName);
                return new BO.User()
                {
                    UserName = user.UserName,
                    Password = user.Password,
                    Admin = user.Admin
                };
            }
            catch(DO.UserExceptions ex)
            {
                 throw new BO.UserNotFound("User Not Found!", userName, ex);
            }
        }
        public void AddUser(BO.User user)
        {
            try
            {
                dl.AddUser(new DO.User()
                {
                    UserName = user.UserName,
                    Password = user.Password,
                    Admin = user.Admin
                });
            }
            catch(DO.UserExceptions ex)
            {
                throw new BO.UserExists("User Already Exists.", user.UserName, ex);
            }
        }
        #endregion

    }
}
