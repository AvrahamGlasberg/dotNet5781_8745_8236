using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DLAPI;
using DO;
namespace DL
{
    sealed class DLXml : IDL
    {
        #region singelton
        static readonly DLXml instance = new DLXml();
        static DLXml() { }// static ctor to ensure instance init is done just before first usage
        DLXml() { } // default => private
        public static DLXml Instance { get => instance; }// The public Instance property to use
        #endregion


        string adjacentStationPath = @"AdjacentStationXml.xml"; //XElement
        string busPath = @"BusXml.xml"; //XMLSerializer
        string busOnTripPath = @"BusOnTripXml.xml"; //XElement
        string linePath = @"LineXml.xml"; //XMLSerializer
        string lineStationPath = @"LineStationXml.xml"; //XMLSerializer
        string lineTripPath = @"LineTripXml.xml"; //
        string stationPath = @"StationXml.xml"; //XElement
        string userPath = @"UserXml.xml"; //XMLSerializer

        //CRUD
        #region AdjacentStation
        public void AddAdjacentStation(AdjacentStation adjacentStation)
        {
            XElement adjacentStationRootElem = XMLTools.LoadListFromXMLElement(adjacentStationPath);

            XElement adj1 = (from adj in adjacentStationRootElem.Elements()
                             let st1 = int.Parse(adj.Element("Station1").Value)
                             let st2 = int.Parse(adj.Element("Station2").Value)
                             where ((st1 == adjacentStation.Station1 && st2 == adjacentStation.Station2) || (st1 == adjacentStation.Station2 && st2 == adjacentStation.Station1)) &&
                             bool.Parse(adj.Element("Deleted").Value) == false
                             select adj).FirstOrDefault();
            if (adj1 == null)
            {

                XElement adjacentStationElem = new XElement("AdjacentStation",
                                       new XElement("Station1", adjacentStation.Station1),
                                       new XElement("Station2", adjacentStation.Station2),
                                       new XElement("Distance", adjacentStation.Distance),
                                       new XElement("Time", adjacentStation.Time.ToString()),
                                       new XElement("Deleted", adjacentStation.Deleted));

                adjacentStationRootElem.Add(adjacentStationElem);

                XMLTools.SaveListToXMLElement(adjacentStationRootElem, adjacentStationPath);
            }
        }

        public AdjacentStation GetAdjacentStation(int station1, int station2)
        {
            XElement adjacentStationRootElem = XMLTools.LoadListFromXMLElement(adjacentStationPath);

            AdjacentStation adjStations = (from adj in adjacentStationRootElem.Elements()
                                           let st1 = int.Parse(adj.Element("Station1").Value)
                                           let st2 = int.Parse(adj.Element("Station2").Value)
                                           where ((st1 == station1 && st2 == station2) || (st1 == station2 && st2 == station1)) &&
                             bool.Parse(adj.Element("Deleted").Value) == false
                             select new AdjacentStation()
                             {
                                 Station1 = int.Parse(adj.Element("Station1").Value),
                                 Station2 = int.Parse(adj.Element("Station2").Value),
                                 Distance = double.Parse(adj.Element("Distance").Value),
                                 Time = TimeSpan.Parse(adj.Element("Time").Value),
                                 Deleted = bool.Parse(adj.Element("Deleted").Value),
                             }).FirstOrDefault();
            if (adjStations == null)
                throw new AdjacentStationExceptions(station1, station2, false);
            return adjStations;
        }

        public IEnumerable<DO.AdjacentStation> GetAdjacentStationsBy(Predicate<DO.AdjacentStation> predicate)
        {
            XElement adjacentStationRootElem = XMLTools.LoadListFromXMLElement(adjacentStationPath);

           return  from adj in adjacentStationRootElem.Elements()
                   let adjCur = new AdjacentStation()
                   {
                       Station1 = int.Parse(adj.Element("Station1").Value),
                       Station2 = int.Parse(adj.Element("Station2").Value),
                       Distance = double.Parse(adj.Element("Distance").Value),
                       Time = TimeSpan.Parse(adj.Element("Time").Value),
                       Deleted = bool.Parse(adj.Element("Deleted").Value),
                   }
                   where predicate(adjCur) && !adjCur.Deleted
                   select adjCur;
        }
        public void UpdateAdjacentStation(AdjacentStation newAdjacentStation)
        {
            XElement adjacentStationRootElem = XMLTools.LoadListFromXMLElement(adjacentStationPath);

            XElement adj1 = (from adj in adjacentStationRootElem.Elements()
                             let st1 = int.Parse(adj.Element("Station1").Value)
                             let st2 = int.Parse(adj.Element("Station2").Value)
                             where ((st1 == newAdjacentStation.Station1 && st2 == newAdjacentStation.Station2) || (st1 == newAdjacentStation.Station2 && st2 == newAdjacentStation.Station1)) &&
                             bool.Parse(adj.Element("Deleted").Value) == false
                             select adj).FirstOrDefault();
            if (adj1 == null)
                throw new AdjacentStationExceptions(newAdjacentStation.Station1, newAdjacentStation.Station2, false);

            adj1.Element("Station1").Value = newAdjacentStation.Station1.ToString();
            adj1.Element("Station2").Value = newAdjacentStation.Station2.ToString();
            adj1.Element("Distance").Value = newAdjacentStation.Distance.ToString();
            adj1.Element("Time").Value = newAdjacentStation.Time.ToString();
            adj1.Element("Deleted").Value = newAdjacentStation.Deleted.ToString();

            XMLTools.SaveListToXMLElement(adjacentStationRootElem, adjacentStationPath);
        }

        public void DeleteAdjacentStation(int station1, int station2)
        {
            XElement adjacentStationRootElem = XMLTools.LoadListFromXMLElement(adjacentStationPath);

            XElement adj1 = (from adj in adjacentStationRootElem.Elements()
                             let st1 = int.Parse(adj.Element("Station1").Value)
                             let st2 = int.Parse(adj.Element("Station2").Value)
                             where ((st1 == station1 && st2 == station2) || (st1 == station2 && st2 == station1)) &&
                             bool.Parse(adj.Element("Deleted").Value) == false
                             select adj).FirstOrDefault();
            if (adj1 == null)
                throw new AdjacentStationExceptions(station1, station2, false);

            adj1.Element("Deleted").Value = true.ToString();

            XMLTools.SaveListToXMLElement(adjacentStationRootElem, adjacentStationPath);
        }
        #endregion

        #region Bus
        public void AddBus(Bus bus)
        {
            List<DO.Bus> buses = XMLTools.LoadListFromXMLSerializer<DO.Bus>(busPath);

            if (buses.FirstOrDefault(curBus => curBus.LicenseNum == bus.LicenseNum && !curBus.Deleted) != null)
                throw new BusExceptions(bus.LicenseNum, true);
            
            buses.Add(bus);
            XMLTools.SaveListToXMLSerializer<DO.Bus>(buses, busPath);
        }

        public IEnumerable<Bus> GettAllBuses()
        {
            List<DO.Bus> buses = XMLTools.LoadListFromXMLSerializer<DO.Bus>(busPath);
            return from bus in buses
                   where !bus.Deleted
                   orderby bus.LicenseNum
                   select bus;
        }

        public Bus GetBus(int license)
        {
            List<DO.Bus> buses = XMLTools.LoadListFromXMLSerializer<DO.Bus>(busPath);
            Bus retValue = buses.FirstOrDefault(curBus => curBus.LicenseNum == license && !curBus.Deleted);
            if (retValue != null)
                return retValue;
            else throw new BusExceptions(license, false);
        }

        public void UpdateBus(Bus NewBus)
        {
            List<DO.Bus> buses = XMLTools.LoadListFromXMLSerializer<DO.Bus>(busPath);
            Bus cur =  buses.FirstOrDefault(curBus => curBus.LicenseNum == NewBus.LicenseNum && !curBus.Deleted);
            if (cur == null)
                throw new BusExceptions(NewBus.LicenseNum, false);
            buses.Remove(cur);
            buses.Add(NewBus);
            XMLTools.SaveListToXMLSerializer<DO.Bus>(buses, busPath);
        }
        public void DeleteBus(int license)
        {
            List<DO.Bus> buses = XMLTools.LoadListFromXMLSerializer<DO.Bus>(busPath);
            Bus cur = buses.FirstOrDefault(curBus => curBus.LicenseNum == license && !curBus.Deleted);
            if (cur == null)
                throw new BusExceptions(license, false);
            cur.Deleted = true;
            XMLTools.SaveListToXMLSerializer<DO.Bus>(buses, busPath);
        }
        #endregion

        #region BusOnTrip
        public void AddBusOnTrip(BusOnTrip busOnTrip)
        {
            XElement busOnTripRootElem = XMLTools.LoadListFromXMLElement(busOnTripPath);

            XElement bus1 = (from b in busOnTripRootElem.Elements()
                             where int.Parse(b.Element("LicenseNum").Value) == busOnTrip.LicenseNum &&
                             int.Parse(b.Element("LineId").Value) == busOnTrip.LineId &&
                             TimeSpan.Parse(b.Element("PlannedTakeOff").Value) == busOnTrip.PlannedTakeOff &&
                             bool.Parse(b.Element("Deleted").Value) == false
                             select b).FirstOrDefault();

            if (bus1 != null)
                throw new BusOnTripExceptions(busOnTrip.LicenseNum, busOnTrip.LineId, busOnTrip.PlannedTakeOff, true);

            XElement busOnTripElem = new XElement("BusOnTrip",
                                   new XElement("Id", busOnTrip.Id),
                                   new XElement("LicenseNum", busOnTrip.LicenseNum),
                                   new XElement("LineId", busOnTrip.LineId),
                                   new XElement("PlannedTakeOff", busOnTrip.PlannedTakeOff.ToString()),
                                   new XElement("ActualTakeOff", busOnTrip.ActualTakeOff.ToString()),
                                   new XElement("PrevStation", busOnTrip.PrevStation),
                                   new XElement("PrevStationAt", busOnTrip.PrevStationAt.ToString()),
                                   new XElement("NextStationAt", busOnTrip.NextStationAt.ToString()),
                                   new XElement("Deleted", busOnTrip.Deleted));

            busOnTripRootElem.Add(busOnTripElem);

            XMLTools.SaveListToXMLElement(busOnTripRootElem, busOnTripPath);
        }

        public BusOnTrip GetBusOnTrip(int license, int lineID, TimeSpan takeOff)
        {
            XElement busOnTripRootElem = XMLTools.LoadListFromXMLElement(busOnTripPath);

            BusOnTrip bus = (from b in busOnTripRootElem.Elements()
                             where int.Parse(b.Element("LicenseNum").Value) == license &&
                             int.Parse(b.Element("LineId").Value) == lineID &&
                             TimeSpan.Parse(b.Element("PlannedTakeOff").Value) == takeOff &&
                             bool.Parse(b.Element("Deleted").Value) == false
                             select new BusOnTrip()
                             {
                                 Id  = int.Parse(b.Element("Id").Value),
                                 LicenseNum  = int.Parse(b.Element("LicenseNum").Value),
                                 LineId  = int.Parse(b.Element("LineId").Value),
                                 PlannedTakeOff  = TimeSpan.Parse(b.Element("PlannedTakeOff").Value),
                                 ActualTakeOff  = TimeSpan.Parse(b.Element("ActualTakeOff").Value),
                                 PrevStation  = int.Parse(b.Element("PrevStation").Value),
                                 PrevStationAt = TimeSpan.Parse(b.Element("PrevStationAt").Value),
                                 NextStationAt = TimeSpan.Parse(b.Element("NextStationAt").Value),
                                 Deleted = bool.Parse(b.Element("Deleted").Value),
                             }).FirstOrDefault();
            if(bus == null)
                throw new BusOnTripExceptions(license, lineID, takeOff, false);
            return bus;
        }

        public void UpdateBusOnTrip(BusOnTrip busOnTrip)
        {
            XElement busOnTripRootElem = XMLTools.LoadListFromXMLElement(busOnTripPath);

            XElement bus1 = (from b in busOnTripRootElem.Elements()
                             where int.Parse(b.Element("LicenseNum").Value) == busOnTrip.LicenseNum &&
                             int.Parse(b.Element("LineId").Value) == busOnTrip.LineId &&
                             TimeSpan.Parse(b.Element("PlannedTakeOff").Value) == busOnTrip.PlannedTakeOff &&
                             bool.Parse(b.Element("Deleted").Value) == false
                             select b).FirstOrDefault();
            if(bus1 == null)
                throw new BusOnTripExceptions(busOnTrip.LicenseNum, busOnTrip.LineId, busOnTrip.PlannedTakeOff, false);

            bus1.Element("Id").Value = busOnTrip.Id.ToString();
            bus1.Element("LicenseNum").Value = busOnTrip.LicenseNum.ToString();
            bus1.Element("LineId").Value = busOnTrip.LineId.ToString();
            bus1.Element("PlannedTakeOff").Value = busOnTrip.PlannedTakeOff.ToString();
            bus1.Element("ActualTakeOff").Value = busOnTrip.ActualTakeOff.ToString();
            bus1.Element("PrevStation").Value = busOnTrip.PrevStation.ToString();
            bus1.Element("PrevStationAt").Value = busOnTrip.PrevStationAt.ToString();
            bus1.Element("NextStationAt").Value = busOnTrip.NextStationAt.ToString();
            bus1.Element("Deleted").Value = busOnTrip.Deleted.ToString();

            XMLTools.SaveListToXMLElement(busOnTripRootElem, busOnTripPath);
        }
        public void DeleteBusOnTrip(int license, int lineID, TimeSpan takeOff)
        {
            XElement busOnTripRootElem = XMLTools.LoadListFromXMLElement(busOnTripPath);

            XElement bus1 = (from b in busOnTripRootElem.Elements()
                             where int.Parse(b.Element("LicenseNum").Value) == license &&
                             int.Parse(b.Element("LineId").Value) == lineID &&
                             TimeSpan.Parse(b.Element("PlannedTakeOff").Value) == takeOff &&
                             bool.Parse(b.Element("Deleted").Value) == false
                             select b).FirstOrDefault();
            if (bus1 == null)
                throw new BusOnTripExceptions(license, lineID, takeOff, false);

            bus1.Element("Deleted").Value = true.ToString();

            XMLTools.SaveListToXMLElement(busOnTripRootElem, busOnTripPath);
        }
        #endregion

        #region Line
        public void AddLine(Line line)
        {
            List<Line> lines = XMLTools.LoadListFromXMLSerializer<Line>(linePath);
            lines.Add(line);
            XMLTools.SaveListToXMLSerializer<Line>(lines, linePath);
        }

        public Line GetLine(int id)
        {
            List<Line> lines = XMLTools.LoadListFromXMLSerializer<Line>(linePath);
            Line retValue = lines.FirstOrDefault(curLine => curLine.Id == id && !curLine.Deleted);
            if (retValue != null)
                return retValue;
            else throw new LineExceptions(id, false);
        }

        public IEnumerable<Line> GetAllLines()
        {
            List<Line> lines = XMLTools.LoadListFromXMLSerializer<Line>(linePath);
            return from CurLine in lines
                   where !CurLine.Deleted
                   orderby CurLine.Code
                   select CurLine;
        }
        public void UpdateLine(Line newLine)
        {
            List<Line> lines = XMLTools.LoadListFromXMLSerializer<Line>(linePath);
            Line cur = lines.FirstOrDefault(curLine => curLine.Id == newLine.Id && !curLine.Deleted);
            if (cur == null)
                throw new LineExceptions(newLine.Id, false);
            lines.Remove(cur);
            lines.Add(newLine);
            XMLTools.SaveListToXMLSerializer<Line>(lines, linePath);
        }
        public void DeleteLine(int id)
        {
            List<Line> lines = XMLTools.LoadListFromXMLSerializer<Line>(linePath);
            Line cur = lines.FirstOrDefault(curLine => curLine.Id == id && !curLine.Deleted);
            if (cur == null)
                throw new LineExceptions(id, false);
            cur.Deleted = true;
            XMLTools.SaveListToXMLSerializer<Line>(lines, linePath);
        }
        #endregion

        #region LineStation
        public void AddLineStation(LineStation lineStation)
        {
            List<LineStation> lineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationPath);
            if (lineStations.FirstOrDefault(curLineStation => curLineStation.LineId == lineStation.LineId && curLineStation.Station == lineStation.Station && !curLineStation.Deleted) != null)
                throw new LineStationExceptions(lineStation.LineId, lineStation.Station, true);
            
            lineStations.Add(lineStation);
            XMLTools.SaveListToXMLSerializer<LineStation>(lineStations, lineStationPath);
        }

        public LineStation GetLineStation(int lineId, int station)
        {
            List<LineStation> lineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationPath);
            LineStation retValue = lineStations.FirstOrDefault(curLineStation => curLineStation.LineId == lineId && curLineStation.Station == station && !curLineStation.Deleted);
            if (retValue != null)
                return retValue;
            else throw new LineStationExceptions(lineId, station, false);
        }

        public IEnumerable<LineStation> GetAllLineStations(int lineId)
        {
            List<LineStation> lineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationPath);
            return from item in lineStations
                   where item.LineId == lineId && !item.Deleted
                   orderby item.LineStationIndex
                   select item;
        }

        public void UpdateLineStation(LineStation newLineStation)
        {
            List<LineStation> lineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationPath);
            LineStation cur = lineStations.FirstOrDefault(curLineStation => curLineStation.LineId == newLineStation.LineId && curLineStation.Station == newLineStation.Station && !curLineStation.Deleted);
            if (cur == null)
                throw new LineStationExceptions(newLineStation.LineId, newLineStation.Station, false);
            lineStations.Remove(cur);
            lineStations.Add(newLineStation);
            XMLTools.SaveListToXMLSerializer<LineStation>(lineStations, lineStationPath);
        }

        public void DeleteLineStation(int lineId, int station)
        {
            List<LineStation> lineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationPath);
            LineStation cur = lineStations.FirstOrDefault(curLineStation => curLineStation.LineId == lineId && curLineStation.Station == station && !curLineStation.Deleted);
            if (cur == null)
                throw new LineStationExceptions(lineId, station, false);
            cur.Deleted = true;
            XMLTools.SaveListToXMLSerializer<LineStation>(lineStations, lineStationPath);
        }

        public void DeleteAlLineStationslBy(Predicate<DO.LineStation> predicate)
        {
            List<LineStation> lineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationPath);
            foreach (var station in lineStations)
                if (predicate(station))
                    station.Deleted = true;
            XMLTools.SaveListToXMLSerializer<LineStation>(lineStations, lineStationPath);
        }

        public IEnumerable<LineStation> GetAllLineStationsBy(Predicate<DO.LineStation> predicate)
        {
            List<LineStation> lineStations = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationPath);
            return from lineStation in lineStations
                   where predicate(lineStation) && !lineStation.Deleted
                   select lineStation;
        }
        #endregion

        #region LineTrip
        public void AddLineTrip(LineTrip lineTrip)
        {
            XElement lineTripRootElem = XMLTools.LoadListFromXMLElement(lineTripPath);

            XElement lineTrip1 = (from l in lineTripRootElem.Elements()
                                  where int.Parse(l.Element("LineId").Value) == lineTrip.LineId &&
                                  TimeSpan.Parse(l.Element("StartAt").Value) == lineTrip.StartAt &&
                                  bool.Parse(l.Element("Deleted").Value) == false
                                  select l).FirstOrDefault();
            if(lineTrip1 != null)
                throw new LineTripExceptions(lineTrip.LineId, lineTrip.StartAt, true);

            XElement lineTripElem = new XElement("LineTrip",
                                       new XElement("LineId", lineTrip.LineId),
                                       new XElement("StartAt", lineTrip.StartAt.ToString()),
                                       new XElement("Frequency", lineTrip.Frequency.ToString()),
                                       new XElement("FinishAt", lineTrip.FinishAt.ToString()),
                                       new XElement("Deleted", lineTrip.Deleted));

            lineTripRootElem.Add(lineTripElem);
            XMLTools.SaveListToXMLElement(lineTripRootElem, lineTripPath);
        }

        public LineTrip GetLineTrip(int lineId, TimeSpan start)
        {
            XElement lineTripRootElem = XMLTools.LoadListFromXMLElement(lineTripPath);
            LineTrip retValue = (from l in lineTripRootElem.Elements()
                                  where int.Parse(l.Element("LineId").Value) == lineId &&
                                  TimeSpan.Parse(l.Element("StartAt").Value) == start &&
                                  bool.Parse(l.Element("Deleted").Value) == false
                                  select new LineTrip()
                                  {
                                      LineId = int.Parse(l.Element("LineId").Value),
                                      StartAt = TimeSpan.Parse(l.Element("StartAt").Value),
                                      Frequency = TimeSpan.Parse(l.Element("Frequency").Value),
                                      FinishAt = TimeSpan.Parse(l.Element("FinishAt").Value),
                                      Deleted = bool.Parse(l.Element("Deleted").Value),
                                  }).FirstOrDefault();
            if(retValue == null)
                throw new LineTripExceptions(lineId, start, false);
            return retValue;
        }

        public IEnumerable<DO.LineTrip> GetAllLineTripsBy(Predicate<DO.LineTrip> predicate)
        {
            XElement lineTripRootElem = XMLTools.LoadListFromXMLElement(lineTripPath);
            return from l in lineTripRootElem.Elements()
                   let curLineTrip = new LineTrip()
                   {
                       LineId = int.Parse(l.Element("LineId").Value),
                       StartAt = TimeSpan.Parse(l.Element("StartAt").Value),
                       Frequency = TimeSpan.Parse(l.Element("Frequency").Value),
                       FinishAt = TimeSpan.Parse(l.Element("FinishAt").Value),
                       Deleted = bool.Parse(l.Element("Deleted").Value),
                   }
                   where predicate(curLineTrip) && !curLineTrip.Deleted
                   orderby curLineTrip.StartAt
                   select curLineTrip;
        }
        public void UpdateLineTrip(LineTrip newLineTrip)
        {
            XElement lineTripRootElem = XMLTools.LoadListFromXMLElement(lineTripPath);

            XElement lineTrip1 = (from l in lineTripRootElem.Elements()
                                  where int.Parse(l.Element("LineId").Value) == newLineTrip.LineId &&
                                  TimeSpan.Parse(l.Element("StartAt").Value) == newLineTrip.StartAt &&
                                  bool.Parse(l.Element("Deleted").Value) == false
                                  select l).FirstOrDefault();
            if(lineTrip1 == null)
                throw new LineTripExceptions(newLineTrip.LineId, newLineTrip.StartAt, false);

            lineTrip1.Element("LineId").Value = newLineTrip.LineId.ToString();
            lineTrip1.Element("StartAt").Value = newLineTrip.StartAt.ToString();
            lineTrip1.Element("Frequency").Value = newLineTrip.Frequency.ToString();
            lineTrip1.Element("FinishAt").Value = newLineTrip.FinishAt.ToString();
            lineTrip1.Element("Deleted").Value = newLineTrip.Deleted.ToString();

            XMLTools.SaveListToXMLElement(lineTripRootElem, lineTripPath);
        }

        public void DeleteLineTrip(int lineId, TimeSpan start)
        {
            XElement lineTripRootElem = XMLTools.LoadListFromXMLElement(lineTripPath);

            XElement lineTrip1 = (from l in lineTripRootElem.Elements()
                                  where int.Parse(l.Element("LineId").Value) == lineId &&
                                  TimeSpan.Parse(l.Element("StartAt").Value) == start &&
                                  bool.Parse(l.Element("Deleted").Value) == false
                                  select l).FirstOrDefault();
            if (lineTrip1 == null)
                throw new LineTripExceptions(lineId, start, false);


            lineTrip1.Element("Deleted").Value = true.ToString();

            XMLTools.SaveListToXMLElement(lineTripRootElem, lineTripPath);
        }
        #endregion

        #region Station
        public void AddStation(Station station)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(stationPath);
            if (stations.FirstOrDefault(curStation => curStation.Code == station.Code &&
            !curStation.Deleted) != null)
                throw new StationExceptions(station.Code, true);
            stations.Add(station);
            XMLTools.SaveListToXMLSerializer<Station>(stations, stationPath);
        }

        public Station GetStation(int code)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(stationPath);
            Station retValue = stations.FirstOrDefault(curStation => curStation.Code == code &&
            !curStation.Deleted);
            if (retValue != null)
                return retValue;
            else throw new StationExceptions(code, false);
        }
        public IEnumerable<DO.Station> GetAllStations()
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(stationPath);
            return from station in stations
                   where !station.Deleted
                   select station;
        }

        public void UpdateStation(Station newStation)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(stationPath);
            Station cur = stations.FirstOrDefault(curStation => curStation.Code == newStation.Code && !curStation.Deleted);
            if (cur == null)
                throw new StationExceptions(newStation.Code, false);
            stations.Remove(cur);
            stations.Add(newStation);
            XMLTools.SaveListToXMLSerializer<Station>(stations, stationPath);
        }

        public void DeleteStation(int code)
        {
            List<Station> stations = XMLTools.LoadListFromXMLSerializer<Station>(stationPath);
            Station cur = stations.FirstOrDefault(curStation => curStation.Code == code && !curStation.Deleted);
            if (cur == null)
                throw new StationExceptions(code, false);
            cur.Deleted = true;
            XMLTools.SaveListToXMLSerializer<Station>(stations, stationPath);
        }
        #endregion

        #region Trip
        //public void AddTrip(Trip trip)
        //{
        //    if (DataSource.Trips.FirstOrDefault(curTrip => curTrip.Id == trip.Id && !curTrip.Deleted) != null)
        //        throw new TripExceptions(trip.Id, true);
        //    else
        //        DataSource.Trips.Add(trip.Clone());
        //}

        //public Trip GetTrip(int id)
        //{
        //    Trip retValue = DataSource.Trips.FirstOrDefault(curTrip => curTrip.Id == id && !curTrip.Deleted);
        //    if (retValue != null)
        //        return retValue.Clone();
        //    else throw new TripExceptions(id, false);
        //}

        //public void UpdateTrip(Trip NewTrip)
        //{
        //    Trip cur = DataSource.Trips.FirstOrDefault(curTrip => curTrip.Id == NewTrip.Id && !curTrip.Deleted);
        //    if (cur == null)
        //        throw new TripExceptions(NewTrip.Id, false);
        //    DataSource.Trips.Remove(cur);
        //    DataSource.Trips.Add(NewTrip);
        //}

        //public void DeleteTrip(int id)
        //{
        //    Trip cur = DataSource.Trips.FirstOrDefault(curTrip => curTrip.Id == id && !curTrip.Deleted);
        //    if (cur == null)
        //        throw new TripExceptions(id, false);
        //    cur.Deleted = true;
        //}
        #endregion

        #region User
        public void AddUser(User user)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(userPath);
            if (users.FirstOrDefault(curUser => curUser.UserName == user.UserName && !curUser.Deleted) != null)
                throw new UserExceptions(user.UserName, true);
            users.Add(user);
            XMLTools.SaveListToXMLSerializer<User>(users, userPath);
        }

        public User GetUser(string name)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(userPath);
            User retValue = users.FirstOrDefault(curUser => curUser.UserName == name && !curUser.Deleted);
            if (retValue != null)
                return retValue;
            else throw new UserExceptions(name, false);
        }

        public void UpdateUser(User newUser)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(userPath);
            User cur = users.FirstOrDefault(curUser => curUser.UserName == newUser.UserName && !curUser.Deleted);
            if (cur == null)
                throw new UserExceptions(newUser.UserName, false);
            users.Remove(cur);
            users.Add(newUser);
            XMLTools.SaveListToXMLSerializer<User>(users, userPath);
        }

        public void DeleteUser(string userName)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(userPath);
            User cur = users.FirstOrDefault(curUser => curUser.UserName == userName && !curUser.Deleted);
            if (cur == null)
                throw new UserExceptions(userName, false);
            cur.Deleted = true;
            XMLTools.SaveListToXMLSerializer<User>(users, userPath);
        }
        #endregion

    }
}
