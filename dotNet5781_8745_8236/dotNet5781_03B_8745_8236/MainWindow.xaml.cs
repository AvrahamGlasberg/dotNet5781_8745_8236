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
using System.ComponentModel;
using System.Windows.Media.Animation;
using System.Media;

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
        List<ProgressBar> pBars;
        List<TextBlock> tBlocks;
        List<Label> Liclbl;
        List<Grid> busesGrids;
        Random Rand;
        static DoubleAnimation anim = new DoubleAnimation();
        MediaPlayer sound = new MediaPlayer();
        public MainWindow()
        {
            InitializeComponent();
            Rand = new Random(DateTime.Now.Millisecond);
            BusLst = new List<Bus>();
            Bus.UpdateWin(this);
            pBars = new List<ProgressBar>();
            tBlocks = new List<TextBlock>();
            Liclbl = new List<Label>();
            busesGrids = new List<Grid>();
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
                if (BusExists(license))
                    i--;
                else
                    BusLst.Add(new Bus(license, start, Rand.Next(100000, 200000), Rand.Next(1201), Rand.Next(20001), new DateTime(DateTime.Now.Year, Rand.Next(1, DateTime.Now.Month + 1), Rand.Next(1, DateTime.Now.Day + 1))));
            }

            //Bus after treatment date
            do
            {
                start = new DateTime(2000, 1, 1);
                range = (DateTime.Now - start).Days - 365;
                start = start.AddDays(Rand.Next(range));
                if (start.Year < 2018)
                    license = Rand.Next(1000000, 10000000);
                else
                    license = Rand.Next(10000000, 100000000);
            } while (BusExists(license));
            BusLst.Add(new Bus(license, start, Rand.Next(100000, 200000), Rand.Next(1201), Rand.Next(20001), new DateTime(DateTime.Now.Year - 1, Rand.Next(1, DateTime.Now.Month + 1), Rand.Next(1, DateTime.Now.Day + 1))));

            //Bus near treatment
            do
            {
                start = new DateTime(2000, 1, 1);
                range = (DateTime.Now - start).Days;
                start = start.AddDays(Rand.Next(range));
                if (start.Year < 2018)
                    license = Rand.Next(1000000, 10000000);
                else
                    license = Rand.Next(10000000, 100000000);
            } while (BusExists(license));
            BusLst.Add(new Bus(license, start, Rand.Next(100000, 200000), Rand.Next(1201), Rand.Next(18000, 20001), new DateTime(DateTime.Now.Year, Rand.Next(1, DateTime.Now.Month + 1), Rand.Next(1, DateTime.Now.Day + 1))));

            //Bus with litte fuel
            do
            {
                start = new DateTime(2000, 1, 1);
                range = (DateTime.Now - start).Days;
                start = start.AddDays(Rand.Next(range));
                if (start.Year < 2018)
                    license = Rand.Next(1000000, 10000000);
                else
                    license = Rand.Next(10000000, 100000000);
            } while (BusExists(license));
            BusLst.Add(new Bus(license, start, Rand.Next(100000, 200000), Rand.Next(1100, 1201), Rand.Next(20001), new DateTime(DateTime.Now.Year, Rand.Next(1, DateTime.Now.Month + 1), Rand.Next(1, DateTime.Now.Day + 1))));
            foreach (Bus cur in BusLst)
            {
                ShowBus(cur);
            }
        }
        private void ShowBus(Bus bus)
        {
            RowDefinition row = new RowDefinition();
            ColumnDefinition col;
            row.Height = new GridLength(50, GridUnitType.Pixel);
            busGrid.RowDefinitions.Add(row);

            Grid innerGrid = new Grid();
            innerGrid.Name = string.Format("Gr{0}", bus.LicNum);
            innerGrid.Background = Brushes.LightGreen;
            for (int i = 0; i < 3; i++)
            {
                col = new ColumnDefinition();
                col.Width = new GridLength(5, GridUnitType.Star);
                innerGrid.ColumnDefinitions.Add(col);
            }
            col = new ColumnDefinition();
            col.Width = new GridLength(10, GridUnitType.Star);
            innerGrid.ColumnDefinitions.Add(col);

            col = new ColumnDefinition();
            col.Width = new GridLength(1, GridUnitType.Star);
            innerGrid.ColumnDefinitions.Add(col);

            Grid.SetRow(innerGrid, BusesInScreen++);
            busGrid.Children.Add(innerGrid);

            Label busNum = new Label();
            busNum.Content = bus.LicToString();
            busNum.Name = string.Format("lbl{0}", bus.LicNum);
            busNum.FontSize = 22;
            busNum.FontFamily = new FontFamily("Ariel");
            busNum.HorizontalContentAlignment = HorizontalAlignment.Center;
            busNum.VerticalContentAlignment = VerticalAlignment.Center;
            busNum.MouseDoubleClick += BusDoubleClick;
            busNum.Background = Brushes.LightGreen;
            busNum.Margin = new Thickness(5, 5, 5, 5);
            Liclbl.Add(busNum);
            Grid.SetColumn(busNum, 0);
            innerGrid.Children.Add(busNum);

            Button drive = new Button();
            drive.Content = "Start driving";
            drive.FontSize = 15;
            drive.FontFamily = new FontFamily("Ariel");
            drive.HorizontalContentAlignment = HorizontalAlignment.Center;
            drive.VerticalContentAlignment = VerticalAlignment.Center;
            drive.Name = String.Format("Btn{0}", bus.LicNum);
            drive.Margin = new Thickness(5, 5, 5, 5);
            drive.Click += Drive;
            drive.MouseEnter += Button_Mouse_Enter;
            drive.MouseLeave += Button_Mouse_Leave;
            Grid.SetColumn(drive, 1);
            innerGrid.Children.Add(drive);

            Button refuel = new Button();
            refuel.Content = "Refueling";
            refuel.FontSize = 15;
            refuel.FontFamily = new FontFamily("Ariel");
            refuel.HorizontalContentAlignment = HorizontalAlignment.Center;
            refuel.VerticalContentAlignment = VerticalAlignment.Center;
            refuel.Name = String.Format("BtnRef{0}", bus.LicNum);
            refuel.Margin = new Thickness(5, 5, 5, 5);
            refuel.Click += Refuel;
            refuel.MouseEnter += Button_Mouse_Enter;
            refuel.MouseLeave += Button_Mouse_Leave;
            Grid.SetColumn(refuel, 2);
            innerGrid.Children.Add(refuel);

            ProgressBar pB = new ProgressBar();
            pB.Name = String.Format("pB{0}", bus.LicNum);
            pB.Margin = new Thickness(5, 10, 5, 10);
            Grid.SetColumn(pB, 3);
            innerGrid.Children.Add(pB);
            pBars.Add(pB);

            TextBlock tB = new TextBlock();
            tB.Text = "";
            tB.FontSize = 20;
            tB.TextAlignment = TextAlignment.Center;
            tB.Name = String.Format("tB{0}", bus.LicNum);
            tB.Background = Brushes.LightGreen;
            tB.Margin = new Thickness(5, 5, 5, 5);
            Grid.SetColumn(tB, 4);
            innerGrid.Children.Add(tB);
            tBlocks.Add(tB);

            busesGrids.Add(innerGrid);
        }
        private void Button_Mouse_Enter(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            anim.From = bt.FontSize;
            anim.To = 25;
            anim.Duration = TimeSpan.FromMilliseconds(500);
            bt.BeginAnimation(Button.FontSizeProperty, anim);
        }
        private void Button_Mouse_Leave(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            anim.From = bt.FontSize;
            anim.To = 15;
            anim.Duration = TimeSpan.FromMilliseconds(500);
            bt.BeginAnimation(Button.FontSizeProperty, anim);
        }
        private void Drive(object sender, RoutedEventArgs e)
        {
            int license = int.Parse(((Button)sender).Name.Substring(3));
            int ind = FindBus(license);
            DrivingWindow newDrive = new DrivingWindow(BusLst[ind]);
            newDrive.Title = BusLst[ind].LicToString();
            newDrive.Show();
        }
        private void Refuel(object sender, RoutedEventArgs e)
        {
            int license = int.Parse(((Button)sender).Name.Substring(6));
            int ind = FindBus(license);
            string message = "";
            bool ready = false;
            if (BusLst[ind].State == BusState.Driving)
                message = "Bus is currently driving!";
            else if (BusLst[ind].State == BusState.Refueling)
                message = "Bus is currently refueling!";
            else if (BusLst[ind].State == BusState.Treatment)
                message = "Bus is currently in treatment!";
            else
                ready = true;
            if (ready)
            {
                BusLst[ind].ReFual();
                RefuelSound();
            }
            else
                MessageBox.Show(message);
        }
        private void BusDoubleClick(object sender, RoutedEventArgs e)
        {
            int licesnse = int.Parse(((Label)sender).Name.Substring(3));
            int ind = FindBus(licesnse);
            if (BusLst[ind].BusDat == null)
            {
                BusData busDataWin = new BusData(BusLst[ind]);
                BusLst[ind].BusDat = busDataWin;
                busDataWin.Show();
            }
            else
                MessageBox.Show("Bus Data window already open!");
            
        }
        public void UpdatePB(int lic, int progress)
        {
            int ind = FindPB(lic);
            pBars[ind].Value = progress;
        }
        public void UpdateTB(int lic, int time)
        {
            int ind = FindTB(lic);
            tBlocks[ind].Text = time.ToString();
        }
        private int FindBus(int lic)
        {
            int ind = BusLst.FindIndex(bus => bus.LicNum == lic);
            return ind;
        }
        private int FindLabel(int lic)
        {
            int ind = Liclbl.FindIndex(lbl => lic == int.Parse(lbl.Name.Substring(3)));
            return ind;
        }
        private int FindGrid(int lic)
        {
            int ind = busesGrids.FindIndex(grid => int.Parse(grid.Name.Substring(2)) == lic);
            return ind;
        }
        private int FindPB(int lic)
        {
            int ind = pBars.FindIndex(pB => lic == int.Parse(pB.Name.Substring(2)));
            return ind;
        }
        private int FindTB(int lic)
        {
            int ind = tBlocks.FindIndex(tB => lic == int.Parse(tB.Name.Substring(2)));
            return ind;
        }
        public void UpdateTime(int lic)
        {
            int ind = FindTB(lic);
            tBlocks[ind].Text = "";
        }

        private void Add_Bus(object sender, RoutedEventArgs e)
        {
            NewBusInfo newBusWin = new NewBusInfo();
            newBusWin.Show();
        }
        public void AddBus(Bus newBus)
        {
            BusLst.Add(newBus);
            ShowBus(newBus);
        }
        public bool BusExists(int lic)
        {
            int ind = BusLst.FindIndex(bus => bus.LicNum == lic);
            return ind != -1;
        }

        private void Window_Key_Down(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Application.Current.Shutdown();
        }
        public void UpdateColor(Bus curBus)
        {
            int ind = FindGrid(curBus.LicNum);
            int indlbl = FindLabel(curBus.LicNum);
            int indtB = FindTB(curBus.LicNum);
            Brush br = (default);
            switch (curBus.State)
            {
                case BusState.Ready:
                    br = Brushes.LightGreen;
                    break;
                case BusState.Driving:
                    br = Brushes.LightYellow;
                    break;
                case BusState.Treatment:
                    br = Brushes.PaleVioletRed;
                    break;
                case BusState.Refueling:
                    br = Brushes.LightBlue;
                    break;
            }
            Liclbl[indlbl].Background = br;
            tBlocks[indtB].Background = br;
            busesGrids[ind].Background = br;
        }
        public void DriveSound()
        {
            sound.Open(new Uri("../../Sounds/drive.mp3", UriKind.Relative));
            sound.Volume = 1;
            sound.Play();
        }
        public void RefuelSound()
        {
            sound.Open(new Uri("../../Sounds/refuel.mp3", UriKind.Relative));
            sound.Volume = 1;
            sound.Play();
        }
        public void TreatSound()
        {
            sound.Open(new Uri("../../Sounds/treatment.mp3", UriKind.Relative));
            sound.Volume = 1;
            sound.Play();
        }

        private void Open_statistics(object sender, RoutedEventArgs e)
        {
            Statistics st = new Statistics(BusLst);
            st.Show();
        }
    }
}
