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
        private BO.LineStation DOLineStationsToBoLineStation(DO.LineStation FirstStation, DO.LineStation NextStation)
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
        public void DeleteBusLine(BO.BusLine line)
        {
            dl.DeleteLine(line.DOLineId);
            dl.DeleteAlLineStationslBy(lineStation => lineStation.LineId == line.DOLineId);
        }

        public void DeleteLineStation(BO.LineStation lineStation)
        {
            DO.Line line = dl.GetLine(lineStation.DOLineId);
            DO.LineStation DOlineStation = dl.GetLineStation(lineStation.DOLineId, lineStation.Code);

            //delete line if too short - less than 2 stations
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
        }
        public bool IsTwoStationsInLine(int DOLineId)
        {
            DO.Line line = dl.GetLine(DOLineId);
            DO.LineStation lastStation = dl.GetLineStation(line.Id, line.LastStation);
            return lastStation.LineStationIndex == 1;
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
        private BO.Line DOLineToBOLine(DO.Line line)
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

            //delete DO.station
            dl.DeleteStation(busStation.Code);
        }
        #endregion

        #region BO.User
        public BO.User GetUser(string userName)
        {
            DO.User user = dl.GetUser(userName);
            return new BO.User()
            {
                UserName = user.UserName,
                Password = user.Password, 
                Admin = user.Admin
            };
        }
        public void AddUser(BO.User user)
        {
            dl.AddUser(new DO.User()
            {
                UserName = user.UserName,
                Password = user.Password,
                Admin = user.Admin
            });
        }
        #endregion

    }
}
