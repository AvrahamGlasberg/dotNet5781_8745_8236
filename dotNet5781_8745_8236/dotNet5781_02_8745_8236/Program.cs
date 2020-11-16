using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Itamar Cohen 318558236 & Avraham Glasberg 206218745
namespace dotNet5781_02_8745_8236
{
	/// <summary>
	/// This program manages bus line list.
	/// As said Dan Z. it is possible in real life and in this program to have 2 identical stations at different locations.
	/// Because the user can create new bus line it is possible for a new line to have less than 3 stations, but the user can't delete exsiting stations to get there.
	/// As said Dan Z. it is possible in real life and in this program to have 2 or more different bus lines with the same bus number.
	/// some of the initial data is random.
	/// </summary>
    class Program
    {
		/// <summary>
		/// The list of all bus lines.
		/// </summary>
        static BusLineList busLst = new BusLineList();

		/// <summary>
		/// random varieble
		/// </summary>
		static Random rand = new Random();

		/// <summary>
		/// enum type for possible options
		/// </summary>
        enum Menu { Add = 1, Delete, Search, Print, Exit }

		/// <summary>
		/// main function
		/// </summary>
		/// <param name="args"></param>
        static void Main(string[] args)
        {
            RestartList();//restarting dataof buses
			int choice = 0;
            do
            {
                Console.WriteLine("\nEnter 1 to add information to the list.\n" +
					"Enter 2 to delete information from the list.\n" +
					"Enter 3 to search information in the list.\n" +
					"Enter 4 to print information from the list.\n" +
					"Enter 5 to exit.");
				int.TryParse(Console.ReadLine(), out choice);//reading choice from user
                try
                {
					switch ((Menu)choice)
					{
						case Menu.Add:
							Addinfo();
							break;
						case Menu.Delete:
							Deleteinfo();
							break;
						case Menu.Search:
							Searchinfo();
							break;
						case Menu.Print:
							Printinfo();
							break;
						case Menu.Exit:
							break;
						default:
							Console.WriteLine("Illegal input!");
							break;
					}
				}
				catch(ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                }
            } while (choice != 5);
        }

		/// <summary>
		/// this function adds new line if enter 1
		/// or new station to the list of bus lines if enter 2.
		/// </summary>
		static void Addinfo()
        {
			//variebles to read from user.
			int choice;
			int secChoice;
			int busKey;
			int busindex;//bus index in the list. in case of more than 1 bus with the same bus number.
			int area;
			int statNum;
			double latit;
			double longt;
			string statName;
			int disToNext;
			int timeToNext;
			int prevStatNum;
			int disToPrev;
			int timeFromPrev;
			Console.WriteLine("Enter 1 to add new bus line.\n" +
				"Enter 2 to add new station into existing bus.");
			int.TryParse(Console.ReadLine(), out choice);
            switch (choice)
            {
				//adding new bus line
				case 1:
                    Console.Write("Enter bus line number: ");
					if (!int.TryParse(Console.ReadLine(), out busKey))
						throw new ArgumentException("Illegal input!");

                    Console.Write("Enter area (0: General, 1:  North, 2: South, 3: Center, 4: Jerusalem): ");
					if (!int.TryParse(Console.ReadLine(), out area))
						throw new ArgumentException("Illegal input!");

					busLst.addLine(busKey, area);
					break;
				//adding new station
				case 2:
					//reading info
                    Console.WriteLine("Enter bus number and index (if there is only 1 with that number enter 0) ");
					if (!int.TryParse(Console.ReadLine(), out busKey) || !int.TryParse(Console.ReadLine(), out busindex))
						throw new ArgumentException("Illegal input!");

                    Console.WriteLine("Enter 1 to add new first station.\n" +
						"Enter 2 to add new station in the middle of the route.\n" +
						"Enter 3 to add new station to the last of the route.");
					if (!int.TryParse(Console.ReadLine(), out secChoice))
						throw new ArgumentException("Illegal input!");

                    Console.WriteLine("Enter station number, latitude, longitude and station name:");
					if(!int.TryParse(Console.ReadLine(), out statNum) || !double.TryParse(Console.ReadLine(), out latit) ||   !double.TryParse(Console.ReadLine(), out longt))
							throw new ArgumentException("Illegal input!");

					statName = Console.ReadLine();
					//adding first station
					if(secChoice == 1)
                    {
                        Console.WriteLine("Enter distance and driving time to the next station (if there isn't another station enter 0)");
						if (!int.TryParse(Console.ReadLine(), out disToNext) || !int.TryParse(Console.ReadLine(), out timeToNext))
							throw new ArgumentException("Illegal input!");
						busLst[busKey, busindex].addFirstStation(statNum, latit, longt, disToNext, timeToNext, statName);
					}
					//adding middle station
					else if(secChoice == 2)
                    {
						Console.WriteLine("Enter previos bus station number, distance and drivingTime from previos station, distance and drivingTime to next station");
						if (!int.TryParse(Console.ReadLine(), out prevStatNum) || !int.TryParse(Console.ReadLine(), out disToPrev) || !int.TryParse(Console.ReadLine(), out timeFromPrev) || !int.TryParse(Console.ReadLine(), out disToNext) || !int.TryParse(Console.ReadLine(), out timeToNext))
							throw new ArgumentException("Illegal input!");
						busLst[busKey, busindex].addStation(prevStatNum, statNum, latit, longt, disToPrev, timeFromPrev, disToNext, timeToNext, statName);
					}
					//adding last sation
					else if(secChoice == 3)
                    {
						Console.WriteLine("Enter distance and driving time from previos station.");
						if (!int.TryParse(Console.ReadLine(), out disToPrev) || !int.TryParse(Console.ReadLine(), out timeFromPrev))
							throw new ArgumentException("Illegal input!");
						busLst[busKey, busindex].addLastStation(statNum, latit, longt, disToPrev, timeFromPrev, statName);
					}
					else//incorrect input
						throw new ArgumentException("Illegal input!");
					break;
                default:
                    Console.WriteLine("Illegal input!");
                    break;
            }
        }
		/// <summary>
		/// this function delete a bus line if enter 1.
		/// or a station from the first bus line with that bus number if enter 2.
		/// </summary>
		static void Deleteinfo()
		{
			//info from user
			int choice;
			int secChoice;
			int busNum;
			int statNum;
			int newDis;
			int newTime;
			Console.WriteLine("Enter 1 to delete bus line.\n" +
				"Enter 2 to delete station from existing bus.");
			int.TryParse(Console.ReadLine(), out choice);

			Console.Write("Enter bus number: ");
			if (!int.TryParse(Console.ReadLine(), out busNum))
				throw new ArgumentException("Illegal input!");

			switch (choice)
			{
				//delete bus line
				case 1:
					busLst.delLine(busNum);
					break;
				//delete station
				case 2:
					Console.WriteLine("Enter 1 to delete first or last station.\n" +
						"Enter 2 to delete middle station");
					if (!int.TryParse(Console.ReadLine(), out secChoice))
						throw new ArgumentException("Illegal input!");

					Console.Write("Enter station number: ");
					if (!int.TryParse(Console.ReadLine(), out statNum))
						throw new ArgumentException("Illegal input!");
					//delete first/last station- no need for new info
					if (secChoice == 1)
						busLst[busNum, 0].deleteFirstOrLastStation(statNum);
					//deleting middle station - need info for new route
					else if (secChoice == 2)
					{
						Console.WriteLine("Enter distance and time for the new route.");
						if (!int.TryParse(Console.ReadLine(), out newDis) || !int.TryParse(Console.ReadLine(), out newTime))
							throw new ArgumentException("Illegal input!");
						busLst[busNum, 0].deleteStation(statNum, newDis, newTime);
					}
					else//incorrect input
						throw new ArgumentException("Illegal input!");
					break;
				default:
					Console.WriteLine("Illegal input!");
					break;
			}
		}
		/// <summary>
		/// this function searches and prints all bus lines in one station if enter 1, 
		/// or all buses in a route between 2 stations printing from shortest to longest if enter 2.
		/// </summary>
		static void Searchinfo()
        {
			int choice;
			int firstStationNum;
			int secStationNum;
			BusLineList lines;
			Console.WriteLine("Enter 1 to search bus lines that drive in a station.\n" +
				"Enter 2 to search all buses that drive between 2 stations.");
			int.TryParse(Console.ReadLine(), out choice);

            switch (choice)
            {
				//search and print all lines in one station.
				case 1:
                    Console.Write("Enter station number:");
					if(!int.TryParse(Console.ReadLine(), out firstStationNum))
						throw new ArgumentException("Illegal input!");
					lines = busLst.findBuses(firstStationNum);
					break;
				//search and prints all buses in a route between 2 stations printing from shortest to longest.
				case 2:
					Console.WriteLine("Enter first and second station numbers.");
					if (!int.TryParse(Console.ReadLine(), out firstStationNum) || !int.TryParse(Console.ReadLine(), out secStationNum))
						throw new ArgumentException("Illegal input!");
					lines = busLst.findBusesInTwoStations(firstStationNum, secStationNum).sortedList();
					break;
                default:
					throw new ArgumentException("Illegal input!");
            }
			foreach(BusLine cur in lines)
                Console.WriteLine(cur);
        }
		/// <summary>
		/// this function prints all buse lines if enter 1,
		/// or prints all buse lines numbers in all stations if enter 2.
		/// </summary>
		static void Printinfo()
		{
			int choice;
			BusLineList lines = new BusLineList();//all the lines in current stations
			List<BusLineStations> stations = new List<BusLineStations>();//all the stations
			Console.WriteLine("Enter 1 to print all bus lines.\n" +
				"Enter 2 to search all buses in all stations.");
			int.TryParse(Console.ReadLine(), out choice);
			switch (choice)
			{
				//prints all bus lines
				case 1:
					foreach(BusLine cur in busLst)
                        Console.WriteLine(cur);
					break;
				//prints all buse lines numbers in all stations.
				case 2:
					//adding all stations to the list once
					foreach (BusLine cur in busLst)
						foreach(BusLineStations curr in cur.Stations)
							if (!stations.Exists(station => station.BusStationKey == curr.BusStationKey))
								stations.Add(curr);
					//printing all bus line numbers
                    foreach(BusLineStations cur in stations)
                    {
						lines = busLst.findBuses(cur.BusStationKey);
                        Console.WriteLine("In bus station {0} driving the bus lines:", cur.BusStationKey);
						foreach(BusLine curr in lines)
                            Console.WriteLine(curr.BusNum);
					}
						break;
				default:
					throw new ArgumentException("Illegal input!");
			}
		}

		/// <summary>
		/// restarting bus line list with 10 lines, 40 stations and 10 stations in more than one line.
		/// </summary>
		static void RestartList()
        {
            for (int i = 0; i < 10; i++)
				busLst.addLine(rand.Next(1000), rand.Next(5));//creating new line, number and are are random
			int ind = 0;//index in info - string array.
			int key;//station number
			string name;//station name
			foreach (BusLine cur in busLst)
            {
				//first station
				key = int.Parse(info[ind].Substring(info[ind].IndexOf(' ') + 1));
				name = info[ind].Substring(0, info[ind].IndexOf(' '));
				cur.addFirstStation(key, rand.NextDouble() * 2.3 + 31, rand.NextDouble() * 1.2 + 34.3, 0, 0, name);
				//another 4 stations to each line.
                for (int i = 0; i < 4; i++)
                {
					ind = (ind + 1) % 40;
					key = int.Parse(info[ind].Substring(info[ind].IndexOf(' ') + 1));
					name = info[ind].Substring(0, info[ind].IndexOf(' '));
					cur.addLastStation(key, rand.NextDouble() * 2.3 + 31, rand.NextDouble() * 1.2 + 34.3, rand.Next(200, 2000), rand.Next(3, 30), name);
                }
            }
            
        }
		//the starting information of 40 stations, name and number.
        static string[] info = new string[]
            {
			"Kiryat-Shemone 908444",
			"Kiryat-Yam 789700",
			"Lod 9028",
			"Maale-Adumim 18888",
			"Maalot-Tarshiha 788828",
			"Migdal-HaEmek 507777",
			"Modiin 726000",
			"Nahariya 879811",
			"Nazareth 459200",
			"Nes-Ziona 83838",
			"Nesher 5111",
			"Netanya 3131",
			"Netivot 938701",
			"Nof-Hagalil 478888",
			"Ofakim 923911",
			"Or-Akiva 108800",
			"Or-Yehuda 335758",
			"Petah-Tikva 52272",
			"Qalansawe 780388",
			"Raanana 10610",
			"Rahat 914811",
			"Ramat-Hasharon 543888",
			"Ramat-Gan 753555",
			"Ramla 771795",
			"Rehovot 92222",
			"Rishon-Lezion 682362",
			"Rosh-Ha'ayin 935656",
			"Sakhnin 788888",
			"Sderot 662222",
			"Shefaram 905111",
			"Taibeh 991131",
			"Tamra 8228",
			"Tel-Aviv 218438",
			"Tiberias 39555",
			"Tira 938432",
			"Tirat-Carmel 847000",
			"Tsfat 697402",
			"Umm-al-Fahm 119000",
			"Yavne 433300",
			"Yehud-Monosson 391212",
			"Yokneam 596000"
			};

    }
}
//Running example:

//Enter 1 to add information to the list.
//Enter 2 to delete information from the list.
//Enter 3 to search information in the list.
//Enter 4 to print information from the list.
//Enter 5 to exit.
//4
//Enter 1 to print all bus lines.
//Enter 2 to search all buses in all stations.
//1
//Bus line number: 348, Area: Jerusalem
//1: Bus Station Code: 908444, 31.9564026017936°N 35.2722938588691°E
//2: Bus Station Code: 789700, 31.3277887134477°N 34.9702386607743°E
//3: Bus Station Code: 9028, 32.3697297538024°N 35.1665121492308°E
//4: Bus Station Code: 18888, 33.2706598530852°N 35.1105491992135°E
//5: Bus Station Code: 788828, 33.0513851102215°N 34.8882305872572°E

//Bus line number: 397, Area: Center
//1: Bus Station Code: 788828, 32.173463632154°N 34.82524329616°E
//2: Bus Station Code: 507777, 31.3516244552339°N 35.1600145040359°E
//3: Bus Station Code: 726000, 32.2964896850737°N 34.4629295741035°E
//4: Bus Station Code: 879811, 31.4356037678828°N 35.3314533957427°E
//5: Bus Station Code: 459200, 31.6004790619949°N 34.6595783136597°E

//Bus line number: 442, Area: North
//1: Bus Station Code: 459200, 32.572489415562°N 35.3068338266606°E
//2: Bus Station Code: 83838, 32.2297256109909°N 34.6980849064878°E
//3: Bus Station Code: 5111, 32.6669283232032°N 35.2067147994911°E
//4: Bus Station Code: 3131, 31.9943351375378°N 34.3826387888205°E
//5: Bus Station Code: 938701, 32.4686417879856°N 35.2227211343696°E

//Bus line number: 917, Area: North
//1: Bus Station Code: 938701, 31.1728826891505°N 35.1389781577694°E
//2: Bus Station Code: 478888, 32.1260633557225°N 34.7468102088416°E
//3: Bus Station Code: 923911, 32.0417953043905°N 34.5236542296706°E
//4: Bus Station Code: 108800, 31.0394224008729°N 34.4635640852915°E
//5: Bus Station Code: 335758, 31.0589208719129°N 34.7241170045101°E

//Bus line number: 654, Area: General
//1: Bus Station Code: 335758, 32.1468644731431°N 34.5141569885491°E
//2: Bus Station Code: 52272, 32.5788793172589°N 35.4494664031777°E
//3: Bus Station Code: 780388, 32.4238849722892°N 34.4738138799434°E
//4: Bus Station Code: 10610, 31.8783078713707°N 34.6092018138194°E
//5: Bus Station Code: 914811, 31.4197586146741°N 34.7092269318221°E

//Bus line number: 94, Area: Jerusalem
//1: Bus Station Code: 914811, 31.1124308291881°N 34.6736190514516°E
//2: Bus Station Code: 543888, 32.9416304196891°N 35.0047974196751°E
//3: Bus Station Code: 753555, 32.5232693310004°N 35.4880711005945°E
//4: Bus Station Code: 771795, 31.0102728453513°N 34.7338414181181°E
//5: Bus Station Code: 92222, 31.2880144147147°N 34.7038892967645°E

//Bus line number: 560, Area: Jerusalem
//1: Bus Station Code: 92222, 31.5372481254103°N 34.4407332607269°E
//2: Bus Station Code: 682362, 32.6266086595257°N 35.4094435726802°E
//3: Bus Station Code: 935656, 31.5609173437864°N 35.3363523035479°E
//4: Bus Station Code: 788888, 32.1814814932092°N 35.3849803221808°E
//5: Bus Station Code: 662222, 32.9464966741141°N 34.6106873636649°E

//Bus line number: 421, Area: Center
//1: Bus Station Code: 662222, 32.0528902161647°N 35.3935666532691°E
//2: Bus Station Code: 905111, 31.6892667950081°N 34.4459371776068°E
//3: Bus Station Code: 991131, 32.9011822099337°N 35.0251471103752°E
//4: Bus Station Code: 8228, 32.2559832492638°N 35.1496247936271°E
//5: Bus Station Code: 218438, 32.9381924156277°N 35.1310379658039°E

//Bus line number: 802, Area: General
//1: Bus Station Code: 218438, 31.1368280986961°N 34.4492159903744°E
//2: Bus Station Code: 39555, 32.283601920625°N 35.4930291868714°E
//3: Bus Station Code: 938432, 32.7488036580145°N 34.7647104118321°E
//4: Bus Station Code: 847000, 31.1402804619355°N 35.4173309016588°E
//5: Bus Station Code: 697402, 31.8494002541292°N 35.0560861147736°E

//Bus line number: 357, Area: North
//1: Bus Station Code: 697402, 31.4376690518752°N 34.4343661921724°E
//2: Bus Station Code: 119000, 32.8898641413031°N 35.1464237179823°E
//3: Bus Station Code: 433300, 32.6321207203121°N 35.3823479212273°E
//4: Bus Station Code: 391212, 31.327951979883°N 35.0551089172974°E
//5: Bus Station Code: 908444, 33.2216184565898°N 35.1869180931183°E


//Enter 1 to add information to the list.
//Enter 2 to delete information from the list.
//Enter 3 to search information in the list.
//Enter 4 to print information from the list.
//Enter 5 to exit.
//4
//Enter 1 to print all bus lines.
//Enter 2 to search all buses in all stations.
//2
//In bus station 908444 driving the bus lines:
//348
//357
//In bus station 789700 driving the bus lines:
//348
//In bus station 9028 driving the bus lines:
//348
//In bus station 18888 driving the bus lines:
//348
//In bus station 788828 driving the bus lines:
//348
//397
//In bus station 507777 driving the bus lines:
//397
//In bus station 726000 driving the bus lines:
//397
//In bus station 879811 driving the bus lines:
//397
//In bus station 459200 driving the bus lines:
//397
//442
//In bus station 83838 driving the bus lines:
//442
//In bus station 5111 driving the bus lines:
//442
//In bus station 3131 driving the bus lines:
//442
//In bus station 938701 driving the bus lines:
//442
//917
//In bus station 478888 driving the bus lines:
//917
//In bus station 923911 driving the bus lines:
//917
//In bus station 108800 driving the bus lines:
//917
//In bus station 335758 driving the bus lines:
//917
//654
//In bus station 52272 driving the bus lines:
//654
//In bus station 780388 driving the bus lines:
//654
//In bus station 10610 driving the bus lines:
//654
//In bus station 914811 driving the bus lines:
//654
//94
//In bus station 543888 driving the bus lines:
//94
//In bus station 753555 driving the bus lines:
//94
//In bus station 771795 driving the bus lines:
//94
//In bus station 92222 driving the bus lines:
//94
//560
//In bus station 682362 driving the bus lines:
//560
//In bus station 935656 driving the bus lines:
//560
//In bus station 788888 driving the bus lines:
//560
//In bus station 662222 driving the bus lines:
//560
//421
//In bus station 905111 driving the bus lines:
//421
//In bus station 991131 driving the bus lines:
//421
//In bus station 8228 driving the bus lines:
//421
//In bus station 218438 driving the bus lines:
//421
//802
//In bus station 39555 driving the bus lines:
//802
//In bus station 938432 driving the bus lines:
//802
//In bus station 847000 driving the bus lines:
//802
//In bus station 697402 driving the bus lines:
//802
//357
//In bus station 119000 driving the bus lines:
//357
//In bus station 433300 driving the bus lines:
//357
//In bus station 391212 driving the bus lines:
//357

//Enter 1 to add information to the list.
//Enter 2 to delete information from the list.
//Enter 3 to search information in the list.
//Enter 4 to print information from the list.
//Enter 5 to exit.

