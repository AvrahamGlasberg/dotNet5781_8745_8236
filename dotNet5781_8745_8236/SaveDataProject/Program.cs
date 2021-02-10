using System;
using DS;
using DL;
using System.Xml.Linq;
using System.IO;

namespace SaveDataProject
{
    class Program
    {
        static string dir = @"xml\";
        static private void SaveTimeSpan()
        {
            string adjacentStationPath = @"AdjacentStationXml.xml"; //XElement
            string path = dir + adjacentStationPath;
            XElement adjacentStationRootElem = XElement.Load(path);
            adjacentStationRootElem.RemoveAll();

            foreach (var item in DataSource.AdjacentStations)
            {
                adjacentStationRootElem.Add(new XElement("AdjacentStation",
                                       new XElement("Station1", item.Station1),
                                       new XElement("Station2", item.Station2),
                                       new XElement("Distance", item.Distance),
                                       new XElement("Time", item.Time.ToString()),
                                       new XElement("Deleted", item.Deleted)));
            }
            adjacentStationRootElem.Save(path);
        }
        static void Main(string[] args)
        {
            try
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                SaveTimeSpan();

                XMLTools.SaveListToXMLSerializer<DO.Bus>(DataSource.Buses, @"BusXml.xml");
                XMLTools.SaveListToXMLSerializer<DO.Line>(DataSource.Lines, @"LineXml.xml");
                XMLTools.SaveListToXMLSerializer<DO.LineStation>(DataSource.LineStations, @"LineStationXml.xml");
                XMLTools.SaveListToXMLSerializer<DO.Station>(DataSource.Stations, @"StationXml.xml");
                XMLTools.SaveListToXMLSerializer<DO.User>(DataSource.Users, @"UserXml.xml");
                XMLTools.SaveListToXMLSerializer<DO.BusOnTrip>(DataSource.BusesOnTrip, @"BusOnTripXml.xml");
                XMLTools.SaveListToXMLSerializer<DO.LineTrip>(DataSource.LinesTrip, @"LineTripXml.xml");
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
    }
}
