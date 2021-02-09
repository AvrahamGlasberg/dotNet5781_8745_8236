using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLAPI;
using Microsoft.Maps.MapControl.WPF;

namespace PL
{
    /// <summary>
    /// Interaction logic for UserInfo.xaml
    /// </summary>
    public partial class UserInfo : Window
    {
        /// <summary>
        /// object that implement IBL
        /// </summary>
        IBL bl;
        /// <summary>
        /// object of GeoCoordinateToLocationConvertor
        /// </summary>
        GeoCoordinateToLocationConvertor convertor = new GeoCoordinateToLocationConvertor();
        BO.User user;
        /// <summary>
        /// win ctor
        /// </summary>
        /// <param name="us"></param>
        public UserInfo(BO.User us)
        {
            InitializeComponent();
            try
            {
                bl = BLFactory.GetBL();
            }
            catch (BO.MissingData ex) //creating BO failed
            {
                MessageBox.Show(ex.Message); 
            }
            user = us;
            MainGrid.DataContext = user;
            StartComboBox.DataContext = bl.GetAllBusStations();
            EndComboBox.DataContext = bl.GetAllBusStations();
        }
        /// <summary>
        /// find and update the StartComboBox to the closest Station according to the location in the map
        /// </summary>
        private void UpdateStartComboBox()
        {
            try
            {
                StartComboBox.SelectedIndex = bl.ClosestStationIndex(bl.GetAllBusStations().ToList(), convertor.ConvertBack(StartPin.Location, null, null, null) as System.Device.Location.GeoCoordinate);
            }
            catch(BO.MissingData ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// find and update the EndComboBox to the closest Station according to the location in the map
        /// </summary>
        private void UpdateEndComboBox()
        {
            try
            {
                EndComboBox.SelectedIndex = bl.ClosestStationIndex(bl.GetAllBusStations().ToList(), convertor.ConvertBack(EndPin.Location, null, null, null) as System.Device.Location.GeoCoordinate);
            }
            catch (BO.MissingData ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// update the location of the start map to the location of the station from the StartComboBox
        /// </summary>
        private void UpdateStartMap()
        {
            StartMap.Center = convertor.Convert((StartComboBox.SelectedItem as BO.BusStation).Position, null, null, null) as Location;
            StartPin.Location = StartMap.Center;
        }
        /// <summary>
        /// update the location of the end map to the location of the station from the EndComboBox
        /// </summary>
        private void UpdateEndMap()
        {
            EndMap.Center = convertor.Convert((EndComboBox.SelectedItem as BO.BusStation).Position, typeof(Location), null, null) as Location;
            EndPin.Location = EndMap.Center;
        }
        /// <summary>
        /// go back to the sender window and close the window
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            (new MainWindow()).Show();
            this.Close();
        }
        /// <summary>
        /// update the list of line according to selected station in the StartComboBox
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Start_Selected(object sender, RoutedEventArgs e)
        {
            UpdateStartMap();
            if (EndComboBox.SelectedItem != null)
                UpdateLines();
        }
        /// <summary>
        /// update the list of line according to selected station in the EndComboBox
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void End_Selected(object sender, RoutedEventArgs e)
        {
            UpdateEndMap();
            if (StartComboBox.SelectedItem != null)
                UpdateLines();
        }

        /// <summary>
        /// update the pin of location in the EndMap
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void End_Map_Double_click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point p = e.GetPosition(EndMap);
            Location l = EndMap.ViewportPointToLocation(p);
            EndPin.Location = l;

            UpdateEndComboBox();
        }
        /// <summary>
        /// update the pin of location in the StartMap
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Start_Map_Double_click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point p = e.GetPosition(StartMap);
            Location l = StartMap.ViewportPointToLocation(p);
            StartPin.Location = l;

            UpdateStartComboBox();
        }
        /// <summary>
        /// makes drive for the user
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Drive(object sender, RoutedEventArgs e)
        {
            double price = ((sender as Button).DataContext as BO.UserLineTrip).Price;
            try
            {
                bl.UserTravel(user, price);
                (new MainWindow()).Show();
                this.Close();
            }
            catch(BO.NotEnoughMoney ex) // the user can't take the drive
            {
                MessageBox.Show(ex.Message + " You are short of " + ex.shortOf.ToString().Substring(0, 5) + "$", "Money issues", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(BO.MissingData ex) // missing data
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Update lines according to the start and end station
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void UpdateLines()
        {
            if ((StartComboBox.SelectedItem as BO.BusStation).Code == (EndComboBox.SelectedItem as BO.BusStation).Code)
                MessageBox.Show("Source and destination are too close!", "Invalid input", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                LinesDataGrid.ItemsSource = bl.GetUserLineTrips(StartComboBox.SelectedItem as BO.BusStation, EndComboBox.SelectedItem as BO.BusStation);
        }
        /// <summary>
        /// add money to the user 
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Key_Down(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    double cash;
                    if (!double.TryParse(CashToAddTB.Text, out cash))
                        MessageBox.Show("Please enter valid amount of money!", "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        bl.AddCash(user, cash);
                        user = bl.GetUser(user.UserName);
                        MainGrid.DataContext = user;
                    }
                }
            }
            catch(BO.MissingData ex) // missing data
            {
                MessageBox.Show(ex.Message);
            }
           
        }
        /// <summary>
        /// allow the user to insert only numbers
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Prev_Key_Down(object sender, KeyEventArgs e)
        {
            if (((int)e.Key < (int)Key.D0 || (int)e.Key > (int)Key.D9) && ((int)e.Key < (int)Key.NumPad0 || (int)e.Key > (int)Key.NumPad9) && e.Key != Key.Enter && e.Key != Key.Escape && e.Key != Key.Back && e.Key != Key.OemPeriod && e.Key != Key.Oem1)
                e.Handled = true;
        }
    }
}
