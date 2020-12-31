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
        public string Check()
        {
            //return dl.Check();
            string str = "";
            var lines = GetAllBusLines();
            foreach (BO.BusLine line in lines)
                str += line.ToString() + "\n\n";
            return str;
        }
        public IEnumerable<BO.BusLine> GetAllBusLines()
        {
            return from CurLine in dl.GetAllLines()
                   select LineDOToBusLineBO(CurLine);
        }
        private BO.BusLine LineDOToBusLineBO(DO.Line line)
        {
            BO.BusLine newBusLine = new BO.BusLine() { LineNumber = line.Code };
            newBusLine.LineStations = GetAllBOLineStationsInDOLine(line.Id);
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
                Id = BO.Config.LineId,
                LineNumber = line.Code,
                EndStation = new BO.Station()
                {
                    Code = lastStation.Code,
                    Name = lastStation.Name
                }
            };
        }
    }
}
