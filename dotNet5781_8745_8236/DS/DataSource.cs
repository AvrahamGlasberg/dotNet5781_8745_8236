﻿using System;
using System.Collections.Generic;
using System.Linq;
using DO;
using System.Device.Location;
namespace DS
{
    public static class DataSource
    {
        /// <summary>
        /// list of DO.AdjacentStation
        /// </summary>
        public static List<AdjacentStation> AdjacentStations;
        /// <summary>
        /// list of DO.Bus
        /// </summary>
        public static List<Bus> Buses;
        /// <summary>
        /// list of DO.BusOnTrip
        /// </summary>
        public static List<BusOnTrip> BusesOnTrip;
        /// <summary>
        /// list of DO.Line
        /// </summary>
        public static List<Line> Lines;
        /// <summary>
        /// list of DO.LineStation
        /// </summary>
        public static List<LineStation> LineStations;
        /// <summary>
        /// list of DO.LineTrip
        /// </summary>
        public static List<LineTrip> LinesTrip;
        /// <summary>
        /// list of DO.Station
        /// </summary>
        public static List<Station> Stations;
        /// <summary>
        /// list of DO.Trip
        /// </summary>
        public static List<Trip> Trips;
        /// <summary>
        /// list of DO.User
        /// </summary>
        public static List<User> Users;
        /// <summary>
        /// dataSource ctor
        /// </summary>
        static DataSource()
        {
            InitLists();
        }
        /// <summary>
        /// intilize lists of data
        /// </summary>
        private static void InitLists()
        {
            Random rand = new Random(DateTime.Now.Millisecond);

            #region Stations
            //some stations's codes for inilaziling
            int[] Codes = {38831,
                            38832,
                            38833,
                            38834,
                            38836,
                            38837,
                            38838,
                            38839,
                            38840,
                            38841,
                            38842,
                            38844,
                            38845,
                            38846,
                            38847,
                            38848,
                            38849,
                            38852,
                            38854,
                            38855,
                            38856,
                            38859,
                            38860,
                            38861,
                            38862,
                            38863,
                            38864,
                            38865,
                            38866,
                            38867,
                            38869,
                            38870,
                            38872,
                            38873,
                            38875,
                            38876,
                            38877,
                            38878,
                            38879,
                            38880,
                            38881,
                            38883,
                            38884,
                            38885,
                            38886,
                            38887,
                            38888,
                            38889,
                            38890,
                            38891 };
            //some stations's names for inilaziling
            string[] Names =
            {
                "Bar Lev / Ben Yehuda School",
                "Herzl / Bilu Junction",
                "The surge / fishermen",
                "Fried / The Six Days",
                "A. Lod Central / Download",
                "Hannah Avrech / Vulcani",
                "Herzl / Moshe Sharet",
                "The boys / Eli Cohen",
                "Weizmann / The Boys",
                "The iris / anemone",
                "The anemone / daffodil",
                "Eli Cohen / Ghetto Fighters",
                "Shabazi / Shabbat brothers",
                "Shabazi / Weizmann",
                "Haim Bar Lev / Yitzhak Rabin Boulevard",
                "Lev Hasharon Mental Health Center",
                "Lev Hasharon Mental Health Center",
                "Holtzman / Science",
                "Zrifin Camp / Club",
                "Herzl / Golani",
                "Rotem / Deganiot",
                "The prairie",
                "Introduction to the vine / Slope of the fig",
                "Introduction to the vine / extension",
                "The extension a",
                "The extension b",
                "The extension / veterans",
                "Airports / Aliyah Authority",
                "Wing / Cypress",
                "The gang / Dov Hoz",
                "Beit Halevi e",
                "First / Route 5700",
                "The genius Ben Ish Chai / Ceylon",
                "Okashi / Levi Eshkol",
                "Rest and estate / Yehuda Gorodisky",
                "Gorodsky / Yechiel Paldi",
                "Derech Menachem Begin / Yaakov Hazan",
                "Through the Park / Rabbi Neria",
                "The fig / vine",
                "The fig / oak",
                "Through the Flowers / Jasmine",
                "Yitzhak Rabin / Pinchas Sapir",
                "Menachem Begin / Yitzhak Rabin",
                "Haim Herzog / Dolev",
                "Shades / Cedar School",
                "Through the trees / oak",
                "Through the Trees / Menachem Begin",
                "Independence / Weizmann",
                "Weizmann / The Magic Rug",
                "Tzala / Coral"
            };
            //some stations's Long coordinates for inilaziling
            double[] Longt =
            {
                34.917806,
                34.819541,
                34.782828,
                34.790904,
                34.898098,
                34.796071,
                34.824106,
                34.821857,
                34.822237,
                34.818957,
                34.818392,
                34.827023,
                34.828702,
                34.827102,
                34.763896,
                34.912708,
                34.912602,
                34.807944,
                34.836363,
                34.825249,
                34.81249 ,
                34.910842,
                34.948647,
                34.943393,
                34.940529,
                34.939512,
                34.938705,
                34.8976  ,
                34.879725,
                34.818708,
                34.926837,
                34.899465,
                34.775083,
                34.807039,
                34.816752,
                34.823461,
                34.904907,
                34.878765,
                34.859437,
                34.864555,
                34.784347,
                34.778239,
                34.782985,
                34.785069,
                34.786735,
                34.786623,
                34.785098,
                34.782252,
                34.779753,
                34.787199

            };
            //some stations's Lat coordinates for inilaziling
            double[] Latd =
            {
                32.183921,
                31.870034,
                31.984553,
                31.88855 ,
                31.956392,
                31.892166,
                31.857565,
                31.862305,
                31.865085,
                31.865222,
                31.867597,
                31.86244 ,
                31.863501,
                31.865348,
                31.977409,
                32.300345,
                32.301347,
                31.914255,
                31.963668,
                31.856115,
                31.874963,
                32.300035,
                32.305234,
                32.304022,
                32.302957,
                32.300264,
                32.298171,
                31.990876,
                31.998767,
                31.883019,
                32.349776,
                32.352953,
                31.897286,
                31.883941,
                31.896762,
                31.898463,
                32.076535,
                32.299994,
                31.865457,
                31.866772,
                31.809325,
                31.80037 ,
                31.799224,
                31.800334,
                31.802319,
                31.804595,
                31.805041,
                31.816751,
                31.816579,
                31.801182
            };
            // create list of 50 stations by the data above
            Stations = new List<Station>();
            for (int i = 0; i < 50; i++)
            {
                Station NewStation = new Station
                {
                    Code = Codes[i],
                    Name = Names[i],
                    Longitude = Longt[i],
                    Latitude = Latd[i]
                };
                Stations.Add(NewStation);
            }
            #endregion

            #region Lines
            //some LineNumbers for inilaziling
            int[] LineNumbers =
            {
                5,
                6,
                39,
                74,
                75,
                33,
                97,
                92,
                26,
                947
            };
            // create list of 10 lines
            Lines = new List<Line>();
            for (int i = 0; i < 10; i++)
            {
                Line NewLine = new Line()
                {
                    Id = i + 1,
                    Code = LineNumbers[i],
                    Area = Areas.Center,
                    FirstStation = Stations[3 * i].Code,
                    LastStation = Stations[3 * i + 9].Code
                };
                Lines.Add(NewLine);
            }
            #endregion

            #region LineStations
            //create list of Line Stations
            LineStations = new List<LineStation>();
            for (int i = 0; i <Lines.Count; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    LineStation NewLineStation = new LineStation()
                    {
                        LineId = Lines[i].Id,
                        Station = Stations[i * 3 + j].Code,
                        LineStationIndex = j
                    };
                    // first station 
                    if (j == 0)
                        NewLineStation.PrevStation = null;
                    else
                        NewLineStation.PrevStation = Stations[i * 3 + j - 1].Code;
                    // last station
                    if (j == 9) 
                        NewLineStation.NextStation = null;
                    else
                        NewLineStation.NextStation = Stations[i * 3 + j + 1].Code;
                    LineStations.Add(NewLineStation);
                }
            }
            #endregion

            #region AdjacentStations
            // create list of all the AdjacentStation according to the stations in the lines
            AdjacentStations = new List<AdjacentStation>();
            for (int i = 0; i < LineStations.Count; i++)
            {
                if(LineStations[i].NextStation != null)
                {
                    {
                        GeoCoordinate p1;
                        GeoCoordinate p2;
                        DO.Station st1 = Stations.FirstOrDefault(st => st.Code == LineStations[i].Station);
                        DO.Station st2 = Stations.FirstOrDefault(st => st.Code == (int)LineStations[i].NextStation);
                        p1 = new GeoCoordinate(st1.Latitude, st1.Longitude);
                        p2 = new GeoCoordinate(st2.Latitude, st2.Longitude);
                        double dis = p1.GetDistanceTo(p2) / 1000; //m*1000= km
                        dis *= rand.NextDouble() / 2 + 1; // real. random between 1 and 1.5
                        int speed = rand.Next(20, 60); // km/h
                        double time = dis / speed; // h
                        int h = (int)time;
                        int m = (int)Math.Round(time * 60 - h * 60);//solves for close stations
                        int s = (int)(time * 360)%60 + 1;
                        TimeSpan t = new TimeSpan(h, m, s);
                        if (!AdjacentStations.Exists(stations => stations.Station1 == LineStations[i].Station && stations.Station2 == (int)LineStations[i].NextStation))
                            AdjacentStations.Add(new AdjacentStation() // add the station to the list
                            {
                                Station1 = LineStations[i].Station,
                                Station2 = (int)LineStations[i].NextStation,
                                Distance = dis,
                                Time = t
                            });
                    }
                }
            }
            #endregion

            #region Buses
            // create list of buses 
            Buses = new List<Bus>();
            for (int i = 0; i < 20; i++)
            {
                Bus NewBus = new Bus() // init the parmeters by random value
                {
                    FromDate = DateTime.Now,
                    LastTreatmentDate = DateTime.Now.AddYears(-1),
                    TotalTrip = 0,
                    TripSinceTreatment = 0,
                    FuelRemain = 200,
                    BusStatus = Status.Ready
                };
                int License;
                do // find empty license number
                {
                    License = rand.Next(10000000, 100000000);
                } while (Buses.Exists(bus=>bus.LicenseNum==License));
                NewBus.LicenseNum = License;
                Buses.Add(NewBus);
            }
            #endregion

            #region Trips
            // just create lists without init it 
            BusesOnTrip = new List<BusOnTrip>();
            LinesTrip = new List<LineTrip>();
            Trips = new List<Trip>();
            #endregion

            #region User
            // create list of user, for convenience adding one user
            Users = new List<User>()
            {
                new User()
                {
                    UserName = "Avraham", 
                    Password = "1", 
                    Admin = true,
                    Cash = 0
                }
            };
            #endregion
        }
    }
    
}
