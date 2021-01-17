using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLAPI;
using DS;
using DL;
using DO;
using System.Xml.Linq;

namespace SaveDataProject
{
    class Program
    {
        static private void SaveTimeSpan()
        {
            string dir = @"xml\";
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
