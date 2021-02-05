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
using System.Windows.Shapes;
using BLAPI;
using BO;
using Microsoft.Maps.MapControl.WPF;

namespace PL
{
    /// <summary>
    /// Interaction logic for UserInfo.xaml
    /// </summary>
    public partial class UserInfo : Window
    {
        IBL bl;
        GeoCoordinateToLocationConvertor convertor = new GeoCoordinateToLocationConvertor();
        BO.User user;
        public UserInfo(BO.User us)
        {
            InitializeComponent();
            try
            {
                bl = BLFactory.GetBL();
            }
            catch (BO.MissingData ex) //creating bo failed
            {
                MessageBox.Show(ex.Message);
            }
            user = us;
            MainGrid.DataContext = user;
            StartComboBox.DataContext = bl.GetAllBusStations();
            EndComboBox.DataContext = bl.GetAllBusStations();
        }
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
        private void UpdateStartMap()
        {
            StartMap.Center = convertor.Convert((StartComboBox.SelectedItem as BO.BusStation).Position, null, null, null) as Location;
            StartPin.Location = StartMap.Center;
        }
        private void UpdateEndMap()
        {
            EndMap.Center = convertor.Convert((EndComboBox.SelectedItem as BO.BusStation).Position, typeof(Location), null, null) as Location;
            EndPin.Location = EndMap.Center;
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            (new MainWindow()).Show();
            this.Close();
        }

        private void Start_Selected(object sender, RoutedEventArgs e)
        {
            UpdateStartMap();
            if (EndComboBox.SelectedItem != null)
                UpdateLines();
        }

        private void End_Selected(object sender, RoutedEventArgs e)
        {
            UpdateEndMap();
            if (StartComboBox.SelectedItem != null)
                UpdateLines();
        }

        private void End_Map_Double_click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point p = e.GetPosition(EndMap);
            Location l = EndMap.ViewportPointToLocation(p);
            EndPin.Location = l;

            UpdateEndComboBox();
        }

        private void Start_Map_Double_click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point p = e.GetPosition(StartMap);
            Location l = StartMap.ViewportPointToLocation(p);
            StartPin.Location = l;

            UpdateStartComboBox();
        }

        private void Drive(object sender, RoutedEventArgs e)
        {
            double price = ((sender as Button).DataContext as BO.UserLineTrip).Price;
            try
            {
                bl.UserTravel(user, price);
                (new MainWindow()).Show();
                this.Close();
            }
            catch(BO.NotEnoughMoney ex)
            {
                MessageBox.Show(ex.Message + " You are short of " + ex.shortOf.ToString().Substring(0, 5) + "$", "Money issues", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(BO.MissingData ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void UpdateLines()
        {
            if ((StartComboBox.SelectedItem as BO.BusStation).Code == (EndComboBox.SelectedItem as BO.BusStation).Code)
                MessageBox.Show("Source and destination are too close!", "Invalid input", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                LinesDataGrid.ItemsSource = bl.GetUserLineTrips(StartComboBox.SelectedItem as BO.BusStation, EndComboBox.SelectedItem as BO.BusStation);
        }

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
            catch(BO.MissingData ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void Prev_Key_Down(object sender, KeyEventArgs e)
        {
            if (((int)e.Key < (int)Key.D0 || (int)e.Key > (int)Key.D9) && ((int)e.Key < (int)Key.NumPad0 || (int)e.Key > (int)Key.NumPad9) && e.Key != Key.Enter && e.Key != Key.Escape && e.Key != Key.Back && e.Key != Key.OemPeriod && e.Key != Key.Oem1)
                e.Handled = true;
        }
    }
}
