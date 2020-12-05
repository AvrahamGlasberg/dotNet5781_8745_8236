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

namespace dotNet5781_03B_8745_8236
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// we need to check for 2 buses with same lic num.
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Bus> BusLst;
        int BusesInScreen = 0;
        Random Rand;
        public MainWindow()
        {
            InitializeComponent();
            Rand = new Random(DateTime.Now.Millisecond);
            BusLst = new List<Bus>();
            Restart();
        }
        private void Restart()
        {
            DateTime start;
            int range;
            int license;
            //Random 7 buses
            for (int i = 0; i < 7; i++)
            {
                start = new DateTime(2000, 1, 1);
                range = (DateTime.Now - start).Days;
                start = start.AddDays(Rand.Next(range));
                if (start.Year < 2018)
                    license = Rand.Next(1000000, 10000000);
                else
                    license = Rand.Next(10000000, 100000000);
                BusLst.Add(new Bus(license, start, Rand.Next(100000, 200000), Rand.Next(1201), Rand.Next(20001), new DateTime(DateTime.Now.Year, Rand.Next(1, DateTime.Now.Month + 1), Rand.Next(1, DateTime.Now.Day + 1))));
            }

            //Bus after treatment date
            start = new DateTime(2000, 1, 1);
            range = (DateTime.Now - start).Days - 365;
            start = start.AddDays(Rand.Next(range));
            if (start.Year < 2018)
                license = Rand.Next(1000000, 10000000);
            else
                license = Rand.Next(10000000, 100000000);
            BusLst.Add(new Bus(license, start, Rand.Next(100000, 200000), Rand.Next(1201), Rand.Next(20001), new DateTime(DateTime.Now.Year - 1, Rand.Next(1, DateTime.Now.Month + 1), Rand.Next(1, DateTime.Now.Day + 1))));

            //Bus near treatment
            start = new DateTime(2000, 1, 1);
            range = (DateTime.Now - start).Days;
            start = start.AddDays(Rand.Next(range));
            if (start.Year < 2018)
                license = Rand.Next(1000000, 10000000);
            else
                license = Rand.Next(10000000, 100000000);
            BusLst.Add(new Bus(license, start, Rand.Next(100000, 200000), Rand.Next(1201), Rand.Next(18000, 20001), new DateTime(DateTime.Now.Year, Rand.Next(1, DateTime.Now.Month + 1), Rand.Next(1, DateTime.Now.Day + 1))));
           
            //Bus with litte fuel
            start = new DateTime(2000, 1, 1);
            range = (DateTime.Now - start).Days;
            start = start.AddDays(Rand.Next(range));
            if (start.Year < 2018)
                license = Rand.Next(1000000, 10000000);
            else
                license = Rand.Next(10000000, 100000000);
            BusLst.Add(new Bus(license, start, Rand.Next(100000, 200000), Rand.Next(1100, 1201), Rand.Next(20001), new DateTime(DateTime.Now.Year, Rand.Next(1, DateTime.Now.Month + 1), Rand.Next(1, DateTime.Now.Day + 1))));
            foreach (Bus cur in BusLst)
            {
                ShowBus(cur);
            }
        }
        private void ShowBus(Bus bus)
        {
            RowDefinition row = new RowDefinition();
            row.Height = new GridLength(50, GridUnitType.Pixel);
            busGrid.RowDefinitions.Add(row);

            Grid innerGrid = new Grid();
            for (int i = 0; i < 4; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(1, GridUnitType.Star);
                innerGrid.ColumnDefinitions.Add(col);
            }
            Grid.SetRow(innerGrid, BusesInScreen++);
            busGrid.Children.Add(innerGrid);

            Label busNum = new Label();
            busNum.Content = bus.LicToString();
            busNum.FontSize = 22;
            busNum.FontFamily = new FontFamily("Ariel");
            busNum.HorizontalContentAlignment = HorizontalAlignment.Center;
            busNum.VerticalContentAlignment = VerticalAlignment.Center;
            busNum.Background = Brushes.LightBlue;
            busNum.Name = String.Format("Label{0}", BusesInScreen);
            Grid.SetColumn(busNum, 0);
            innerGrid.Children.Add(busNum);

            Button drive = new Button();
            drive.Content = "Start driving";
            drive.FontSize = 15;
            drive.FontFamily = new FontFamily("Ariel");
            drive.HorizontalContentAlignment = HorizontalAlignment.Center;
            drive.VerticalContentAlignment = VerticalAlignment.Center;
            drive.Name = String.Format("Btn{0}", bus.LicNum);
            drive.Click += Drive;
            Grid.SetColumn(drive, 1);
            
            innerGrid.Children.Add(drive);
        }
        private void Drive(object sender, RoutedEventArgs e)
        {
            int license = int.Parse(((Button)sender).Name.Substring(3));
            int ind;
            for (ind = 0; ind < BusLst.Count; ind++)
                if (BusLst[ind].LicNum == license)
                    break;
            DrivingWindow newDrive = new DrivingWindow(BusLst[ind]);
            newDrive.Show();
        }
        public void startDrive(int lic, int dis)
        {

        }
    }
}
