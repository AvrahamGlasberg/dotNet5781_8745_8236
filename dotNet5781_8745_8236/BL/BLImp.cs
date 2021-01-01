using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                                     from item2 in Linestations
                                     where item2.LineStationIndex == item1.LineStationIndex + 1
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
                Id = BO.Config.LineStationId,
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
                Id = BO.Config.LineStationId,
                DistanceFromPrev = nearStations.Distance,
                TimeFromPrev = nearStations.Time
            };
        }
        public void DeleteBusLine(BO.BusLine line)
        {
            dl.DeleteLine(line.DOLineId);
            dl.DeleteAlLineStationslBy(lineStation => lineStation.LineId == line.DOLineId);
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
                Name = DOstation.Name
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
            //all DO.line s in that station
            var DOlines = from item in busStation.LinesInstation
                          select dl.GetLine(item.DOLineId);
            foreach (DO.Line line in DOlines)
            {
                //delete line if too short - less than 3 stations
                if(dl.GetLineStation(line.Id, line.LastStation).LineStationIndex == 2)
                {
                    //delete DO Line
                    dl.DeleteLine(line.Id);
                    dl.DeleteAlLineStationslBy(station => station.LineId == line.Id);
                }
                else
                {
                    DO.LineStation lineStation = dl.GetLineStation(line.Id, busStation.Code);

                    //delete DO.LineStation
                    dl.DeleteLineStation(lineStation.LineId, lineStation.Station);

                    //update all line stations indexes.
                    DO.LineStation tempLineStation = lineStation;
                    while (tempLineStation.Station != line.LastStation)
                    {
                        tempLineStation = dl.GetLineStation(line.Id, (int)tempLineStation.NextStation);
                        tempLineStation.LineStationIndex -= 1;
                        dl.UpdateLineStation(tempLineStation);
                    }

                    //update Line first/last startion
                    if (busStation.Code == line.FirstStation)
                    {
                        //update line
                        DO.Line tempLine = line;
                        tempLine.FirstStation = (int)lineStation.NextStation;
                        dl.UpdateLine(tempLine);

                        //update next station
                        tempLineStation = dl.GetLineStation(line.Id, (int)lineStation.NextStation);
                        tempLineStation.PrevStation = null;
                        dl.UpdateLineStation(tempLineStation);
                    }
                    else if(busStation.Code == line.LastStation)
                    {
                        //update line
                        DO.Line tempLine = line;
                        tempLine.LastStation = (int)lineStation.PrevStation;
                        dl.UpdateLine(tempLine);

                        //update prev station
                        tempLineStation = dl.GetLineStation(line.Id, (int)lineStation.PrevStation);
                        tempLineStation.NextStation = null;
                        dl.UpdateLineStation(tempLineStation);
                    }
                    else
                    {
                        //update next station
                        tempLineStation = dl.GetLineStation(line.Id, (int)lineStation.NextStation);
                        tempLineStation.PrevStation = lineStation.PrevStation;
                        dl.UpdateLineStation(tempLineStation);

                        //update prev station
                        tempLineStation = dl.GetLineStation(line.Id, (int)lineStation.PrevStation);
                        tempLineStation.NextStation = lineStation.NextStation;
                        dl.UpdateLineStation(tempLineStation);

                        //update DO.adj stations
                        dl.AddAdjacentStation(new DO.AdjacentStation()
                        {
                            Station1 = (int)lineStation.PrevStation,
                            Station2 = (int)lineStation.NextStation,
                            Distance = 500,
                            Time =new TimeSpan(0, 10, 0),
                        });
                    }
                }
            }
            //delete DO.station
            dl.DeleteStation(busStation.Code);
        }
        #endregion
    }
}
