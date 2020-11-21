using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using dotNet5781_02_8745_8236;

namespace dotNet5781_03A_8745_8236
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		private BusLineList busLines;
		private Random rand;
		private BusLine currentDisplayBusLine;
		public MainWindow()
        {
            InitializeComponent();
			busLines = new BusLineList();
			rand = new Random();
			cbBusLines.ItemsSource = busLines;
			cbBusLines.DisplayMemberPath = "BusNum";
			cbBusLines.SelectedIndex = 0;
            RestartList();
		}
		public void RestartList()
		{
			for (int i = 0; i < 10; i++)
				busLines.addLine(rand.Next(1000), rand.Next(5));//creating new line, number and are are random
			int ind = 0;//index in info - string array.
			int key;//station number
			string name;//station name
			foreach (BusLine cur in busLines)
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

        private void cbBusLines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			ShowBusLine((cbBusLines.SelectedValue as BusLine).BusNum);
		}
		private void ShowBusLine(int index)
        {
			currentDisplayBusLine = busLines[index];
			UpGrid.DataContext = currentDisplayBusLine;
			tbArea.Text = string.Format("{0}", currentDisplayBusLine.Area);

			lbBusLineStations.DataContext = currentDisplayBusLine.Stations;
        }

	}
}

