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
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// list of buses in the screen
        /// </summary>
        public List<Bus> BusLst;
        /// <summary>
        /// counter for buses in the screen. - updates automatically in showbus!
        /// </summary>
        int BusesInScreen = 0;
        /// <summary>
        /// list of the progress bars in the screen.
        /// </summary>
        List<ProgressBar> pBars;
        /// <summary>
        /// list of the textboxes in the screen.
        /// </summary>
        List<TextBlock> tBlocks;
        /// <summary>
        /// list of the labels in the screen.
        /// </summary>
        List<Label> Liclbl;
        /// <summary>
        /// list of the inner grids in the screen.
        /// </summary>
        List<Grid> busesGrids;
        /// <summary>
        /// random
        /// </summary>
        Random Rand;
        /// <summary>
        /// double animation
        /// </summary>
        static DoubleAnimation anim = new DoubleAnimation();
        /// <summary>
        /// media player
        /// </summary>
        MediaPlayer sound = new MediaPlayer();
        /// <summary>
        /// constructor for main window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            //initialize all variebles
            Rand = new Random(DateTime.Now.Millisecond);
            BusLst = new List<Bus>();
            Bus.UpdateWin(this);
            pBars = new List<ProgressBar>();
            tBlocks = new List<TextBlock>();
            Liclbl = new List<Label>();
            busesGrids = new List<Grid>();
            //randomize 10 buses
            Restart();
        }
        /// <summary>
        /// this function restarts the bus list with 10 random buses, and shows them on the screen.
        /// </summary>
        private void Restart()
        {
            //starting date
            DateTime start;
            //possible range of days between starting date and now
            int range;
            //license of the bus
            int license;
            //Random 7 buses
            for (int i = 0; i < 7; i++)
            {
                start = new DateTime(2000, 1, 1);
                range = (DateTime.Now - start).Days;
                //random date between 1/1/2000 to now.
                start = start.AddDays(Rand.Next(range));
                //random license according to starting year
                if (start.Year < 2018)
                    license = Rand.Next(1000000, 10000000);
                else
                    license = Rand.Next(10000000, 100000000);
                //if bus already exists.
                if (BusExists(license))
                    i--;
                else//adding the random bus to the list.
                    BusLst.Add(new Bus(license, start, Rand.Next(100000, 200000), Rand.Next(1201), Rand.Next(20001), new DateTime(DateTime.Now.Year, Rand.Next(1, DateTime.Now.Month + 1), Rand.Next(1, DateTime.Now.Day + 1))));
            }
            //one Bus after treatment date - question demand
            do
            {
                start = new DateTime(2000, 1, 1);
                range = (DateTime.Now - start).Days - 365;
                start = start.AddDays(Rand.Next(range));
                if (start.Year < 2018)
                    license = Rand.Next(1000000, 10000000);
                else
                    license = Rand.Next(10000000, 100000000);
            } while (BusExists(license));//until randomize new bus
            //in the treatment date the random max is 1 year earlier from now. so bus needs treatment.
            BusLst.Add(new Bus(license, start, Rand.Next(100000, 200000), Rand.Next(1201), Rand.Next(20001), new DateTime(DateTime.Now.Year - 1, Rand.Next(1, DateTime.Now.Month + 1), Rand.Next(1, DateTime.Now.Day + 1))));

            //Bus near treatment - question demand
            do
            {
                start = new DateTime(2000, 1, 1);
                range = (DateTime.Now - start).Days;
                start = start.AddDays(Rand.Next(range));
                if (start.Year < 2018)
                    license = Rand.Next(1000000, 10000000);
                else
                    license = Rand.Next(10000000, 100000000);
            } while (BusExists(license));//until randomize new bus
            //the distance since last treatment is random between 18,000-20,000 so bus is near treatmment.
            BusLst.Add(new Bus(license, start, Rand.Next(100000, 200000), Rand.Next(1201), Rand.Next(18000, 20001), new DateTime(DateTime.Now.Year, Rand.Next(1, DateTime.Now.Month + 1), Rand.Next(1, DateTime.Now.Day + 1))));

            //Bus with litte fuel - question demand
            do
            {
                start = new DateTime(2000, 1, 1);
                range = (DateTime.Now - start).Days;
                start = start.AddDays(Rand.Next(range));
                if (start.Year < 2018)
                    license = Rand.Next(1000000, 10000000);
                else
                    license = Rand.Next(10000000, 100000000);
            } while (BusExists(license));//until randomize new bus
            //distance since last refueling is random between 1,100 - 1,200 so bus is near refueling.
            BusLst.Add(new Bus(license, start, Rand.Next(100000, 200000), Rand.Next(1100, 1201), Rand.Next(20001), new DateTime(DateTime.Now.Year, Rand.Next(1, DateTime.Now.Month + 1), Rand.Next(1, DateTime.Now.Day + 1))));
            //shows all buses on screen
            foreach (Bus cur in BusLst)
            {
                ShowBus(cur);
            }
        }
        /// <summary>
        /// this function shows the bus on the screen in new inner grid
        /// </summary>
        /// <param name="bus"></param>
        private void ShowBus(Bus bus)
        {
            //new row to main (in scroll view) grid
            RowDefinition row = new RowDefinition();
            //constatnt height of 50 pixels.
            row.Height = new GridLength(50, GridUnitType.Pixel);
            //adding new row.
            busGrid.RowDefinitions.Add(row);

            //the inner grid
            Grid innerGrid = new Grid();
            //column in the inner grid
            ColumnDefinition col;
            //saving grid's name with 2 latters and the bus license number.
            innerGrid.Name = string.Format("Gr{0}", bus.LicNum);
            //bus showed is ready.
            innerGrid.Background = Brushes.LightGreen;
            //adding 3 columns of 5* width for label (license), button (drive) and button (refuel).
            for (int i = 0; i < 3; i++)
            {
                col = new ColumnDefinition();
                col.Width = new GridLength(5, GridUnitType.Star);
                innerGrid.ColumnDefinitions.Add(col);
            }
            //adding new column of 10* width for progress bar.
            col = new ColumnDefinition();
            col.Width = new GridLength(10, GridUnitType.Star);
            innerGrid.ColumnDefinitions.Add(col);

            //adding new column of 1* for textblock (time until bus is ready)
            col = new ColumnDefinition();
            col.Width = new GridLength(1, GridUnitType.Star);
            innerGrid.ColumnDefinitions.Add(col);

            //the index of the new inner grid is as buses in screen var. and updates it(BusesInScreen).
            Grid.SetRow(innerGrid, BusesInScreen++);
            busGrid.Children.Add(innerGrid);

            //first to last means left to right in the  screen
            //first control - label that contains the bus's license
            Label busNum = new Label();
            busNum.Content = bus.LicToString();
            //saving the name as 3 latters and the bus's license number.
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

            //second control - button for starting new drive for the bus
            Button drive = new Button();
            drive.Content = "Start driving";
            drive.FontSize = 15;
            drive.FontFamily = new FontFamily("Ariel");
            drive.HorizontalContentAlignment = HorizontalAlignment.Center;
            drive.VerticalContentAlignment = VerticalAlignment.Center;
            //saving the name as 3 latters and the bus's license number.
            drive.Name = String.Format("Btn{0}", bus.LicNum);
            drive.Margin = new Thickness(5, 5, 5, 5);
            drive.Click += Drive;
            drive.MouseEnter += Button_Mouse_Enter;
            drive.MouseLeave += Button_Mouse_Leave;
            Grid.SetColumn(drive, 1);
            innerGrid.Children.Add(drive);

            //third control - button for refueling the bus
            Button refuel = new Button();
            refuel.Content = "Refueling";
            refuel.FontSize = 15;
            refuel.FontFamily = new FontFamily("Ariel");
            refuel.HorizontalContentAlignment = HorizontalAlignment.Center;
            refuel.VerticalContentAlignment = VerticalAlignment.Center;
            //saving the name as 6 latters and the bus's license number.
            refuel.Name = String.Format("BtnRef{0}", bus.LicNum);
            refuel.Margin = new Thickness(5, 5, 5, 5);
            refuel.Click += Refuel;
            refuel.MouseEnter += Button_Mouse_Enter;
            refuel.MouseLeave += Button_Mouse_Leave;
            Grid.SetColumn(refuel, 2);
            innerGrid.Children.Add(refuel);

            //fourth control - progress bar
            ProgressBar pB = new ProgressBar();
            //saving the name as 2 latters and the bus's license number.
            pB.Name = String.Format("pB{0}", bus.LicNum);
            pB.Margin = new Thickness(5, 10, 5, 10);
            Grid.SetColumn(pB, 3);
            innerGrid.Children.Add(pB);
            pBars.Add(pB);

            //last control - textblock for time until bus is ready
            TextBlock tB = new TextBlock();
            tB.Text = "";
            tB.FontSize = 20;
            tB.TextAlignment = TextAlignment.Center;
            //saving the name as 2 latters and the bus's license number.
            tB.Name = String.Format("tB{0}", bus.LicNum);
            tB.Background = Brushes.LightGreen;
            tB.Margin = new Thickness(5, 5, 5, 5);
            Grid.SetColumn(tB, 4);
            innerGrid.Children.Add(tB);
            tBlocks.Add(tB);

            //adding the new inner grid.
            busesGrids.Add(innerGrid);
        }
        /// <summary>
        /// Mouse enter event for one of the buttons. the function animate the font size to size of 25 in 1/2 second.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Mouse_Enter(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            anim.From = bt.FontSize;
            anim.To = 25;
            anim.Duration = TimeSpan.FromMilliseconds(500);
            bt.BeginAnimation(Button.FontSizeProperty, anim);
        }
        /// <summary>
        /// Mouse leave event for one of the buttons. the function animate the font size to size of 15 in 1/2 second.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Mouse_Leave(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            anim.From = bt.FontSize;
            anim.To = 15;
            anim.Duration = TimeSpan.FromMilliseconds(500);
            bt.BeginAnimation(Button.FontSizeProperty, anim);
        }
        /// <summary>
        /// click event for one of the "driving" buttons. the function opens new driving window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Drive(object sender, RoutedEventArgs e)
        {
            //getting the license from the button sender's name.
            int license = int.Parse(((Button)sender).Name.Substring(3));
            int ind = FindBus(license);
            DrivingWindow newDrive = new DrivingWindow(BusLst[ind]);
            newDrive.Title = BusLst[ind].LicToString();
            newDrive.Show();
        }
        /// <summary>
        /// click event for one of the "refueling" buttons. the function checks if possible and starts to refuel the bus.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refuel(object sender, RoutedEventArgs e)
        {
            //getting the license from the button sender's name.
            int license = int.Parse(((Button)sender).Name.Substring(6));
            int ind = FindBus(license);
            //checks if bus is ready to refuel.
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
                MessageBox.Show(message);//error message
        }
        /// <summary>
        /// double click event on one of the labels(bus's license number) the function open new bus data window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BusDoubleClick(object sender, RoutedEventArgs e)
        {
            //getting the license from the label sender's name.
            int licesnse = int.Parse(((Label)sender).Name.Substring(3));
            int ind = FindBus(licesnse);
            if (BusLst[ind].BusDat == null)
            {
                //updates the window and the bus.
                BusData busDataWin = new BusData(BusLst[ind]);
                BusLst[ind].BusDat = busDataWin;
                busDataWin.Show();
            }
            else
                MessageBox.Show("Bus Data window already open!");//can't open the same data window again
            
        }
        /// <summary>
        /// this function updates the progress bar
        /// </summary>
        /// <param name="lic">license number for the bus's progress bar to update</param>
        /// <param name="progress">progress to update. 0-100</param>
        public void UpdatePB(int lic, int progress)
        {
            int ind = FindPB(lic);
            pBars[ind].Value = progress;
        }
        /// <summary>
        /// this function updates the textblock of the time until the bus is ready
        /// </summary>
        /// <param name="lic">license number for the bus's text box to update</param>
        /// <param name="time">the new time to update</param>
        public void UpdateTB(int lic, int time)
        {
            int ind = FindTB(lic);
            tBlocks[ind].Text = time.ToString();
        }
        /// <summary>
        /// this function find the bus's index in the list according to the bus's license number.
        /// </summary>
        /// <param name="lic">the bus's license number</param>
        /// <returns>the index in the list. returns -1 if bus doesnot exsists.</returns>
        private int FindBus(int lic)
        {
            int ind = BusLst.FindIndex(bus => bus.LicNum == lic);
            return ind;
        }
        /// <summary>
        /// this function find the label's index in the list according to the bus's license number.
        /// </summary>
        /// <param name="lic">the bus's license number</param>
        /// <returns>the index in the list. returns -1 if label doesnot exsists.</returns>
        private int FindLabel(int lic)
        {
            int ind = Liclbl.FindIndex(lbl => lic == int.Parse(lbl.Name.Substring(3)));
            return ind;
        }
        /// <summary>
        /// this function find the grid's index in the list according to the bus's license number.
        /// </summary>
        /// <param name="lic">the bus's license number</param>
        /// <returns>the index in the list. returns -1 if grid doesnot exsists.</returns>
        private int FindGrid(int lic)
        {
            int ind = busesGrids.FindIndex(grid => int.Parse(grid.Name.Substring(2)) == lic);
            return ind;
        }
        /// <summary>
        /// this function find the progress bar's index in the list according to the bus's license number.
        /// </summary>
        /// <param name="lic">the bus's license number</param>
        /// <returns>the index in the list. returns -1 if progress bar doesnot exsists.</returns>
        private int FindPB(int lic)
        {
            int ind = pBars.FindIndex(pB => lic == int.Parse(pB.Name.Substring(2)));
            return ind;
        }
        /// <summary>
        /// this function find the textblock's index in the list according to the bus's license number.
        /// </summary>
        /// <param name="lic">the bus's license number</param>
        /// <returns>the index in the list. returns -1 if textblock doesnot exsists.</returns>
        private int FindTB(int lic)
        {
            int ind = tBlocks.FindIndex(tB => lic == int.Parse(tB.Name.Substring(2)));
            return ind;
        }
        /// <summary>
        /// this function delete the time in the text block into "".
        /// </summary>
        /// <param name="lic">the bus's license number</param>
        public void UpdateTime(int lic)
        {
            int ind = FindTB(lic);
            tBlocks[ind].Text = "";
        }
        /// <summary>
        /// Click event for add new bus button. the function opens new bus info window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Bus(object sender, RoutedEventArgs e)
        {
            NewBusInfo newBusWin = new NewBusInfo();
            newBusWin.Show();
        }
        /// <summary>
        /// adding new bus to the list
        /// </summary>
        /// <param name="newBus">new bus to add</param>
        public void AddBus(Bus newBus)
        {
            BusLst.Add(newBus);
            ShowBus(newBus);
        }
        /// <summary>
        /// function for checking if bus exists in the list
        /// </summary>
        /// <param name="lic">the bus's license number</param>
        /// <returns></returns>
        public bool BusExists(int lic)
        {
            return BusLst.Exists(bus => bus.LicNum == lic);
        }
        /// <summary>
        /// key down event in the window. shutting down if "esc" preseed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Key_Down(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Application.Current.Shutdown();
        }
        /// <summary>
        /// this function updates the back ground color on the screen according to the bus's current state.
        /// </summary>
        /// <param name="curBus">the current bus.</param>
        public void UpdateColor(Bus curBus)
        {
            //finding the grid, label and textblock to update thier back ground color.
            int ind = FindGrid(curBus.LicNum);
            int indlbl = FindLabel(curBus.LicNum);
            int indtB = FindTB(curBus.LicNum);
            Brush br = (default);
            //checking the bus's current state.
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
            //updates color.
            Liclbl[indlbl].Background = br;
            tBlocks[indtB].Background = br;
            busesGrids[ind].Background = br;
        }
        /// <summary>
        /// this function plays driving sound
        /// </summary>
        public void DriveSound()
        {
            sound.Open(new Uri("../../Sounds/drive.mp3", UriKind.Relative));
            sound.Volume = 1;
            sound.Play();
        }
        /// <summary>
        /// this function plays refueling sound
        /// </summary>
        public void RefuelSound()
        {
            sound.Open(new Uri("../../Sounds/refuel.mp3", UriKind.Relative));
            sound.Volume = 1;
            sound.Play();
        }
        /// <summary>
        /// this function plays treatment sound.
        /// </summary>
        public void TreatSound()
        {
            sound.Open(new Uri("../../Sounds/treatment.mp3", UriKind.Relative));
            sound.Volume = 1;
            sound.Play();
        }
        /// <summary>
        /// click event on the statistics button. the function opens new statistics window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_statistics(object sender, RoutedEventArgs e)
        {
            Statistics st = new Statistics(BusLst);
            st.Show();
        }
    }
}
