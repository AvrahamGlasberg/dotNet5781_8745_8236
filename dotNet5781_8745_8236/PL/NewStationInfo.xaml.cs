﻿using System.Linq;
using System.Windows;
using System.Windows.Input;
using Microsoft.Maps.MapControl.WPF;
using BLAPI;
using System.Device.Location;

namespace PL
{
    /// <summary>
    /// Interaction logic for NewStationInfo.xaml
    /// </summary>
    public partial class NewStationInfo : Window
    {
        /// <summary>
        /// object that implement IBL
        /// </summary>
        IBL bl;
        /// <summary>
        /// window ctor
        /// </summary>
        public NewStationInfo()
        {
            InitializeComponent();
            try
            {
                bl = BLFactory.GetBL();
            }
            catch(BO.MissingData ex) //creating bo failed
            {
                MessageBox.Show(ex.Message);
            }
            CodeTB.Focus();
        }
        /// <summary>
        /// move to the nest textBlock when press enter key
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void CodeTBKey_Down(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                NameTB.Focus();
        }
        /// <summary>
        /// allow the user to insert only numbers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Numbers_Enter_Only(object sender, KeyEventArgs e)
        {
            if (((int)e.Key < (int)Key.D0 || (int)e.Key > (int)Key.D9) && ((int)e.Key < (int)Key.NumPad0 || (int)e.Key > (int)Key.NumPad9) && e.Key != Key.Enter && e.Key != Key.Escape && e.Key != Key.Back)
                e.Handled = true;
        }
        /// <summary>
        /// change the location from the map and save it for update
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Map_Double_click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point p = e.GetPosition(MyMap);
            Location l = MyMap.ViewportPointToLocation(p);
            Pin.Location = l;
        }
        /// <summary>
        /// Add the station to the data
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Add_Station(object sender, RoutedEventArgs e)
        {
            int code;
            string message = "";
            if (!int.TryParse(CodeTB.Text, out code) || NameTB.Text == "") // when some data is missing / Invalid jumping massage to the user and asking it
            {
                if(!int.TryParse(CodeTB.Text, out code))
                    message = "Please enter station's code!\n";
                if (NameTB.Text == "")
                    message += "Please enter station's name!";
                MessageBox.Show(message, "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    bl.AddBusStation(
                        new BO.BusStation()
                        {
                            Code = code,
                            Name = NameTB.Text,
                            Position = new GeoCoordinate(Pin.Location.Latitude, Pin.Location.Longitude),
                            LinesInstation = Enumerable.Empty<BO.Line>()
                        }
                        );
                    this.Close();
                }
                catch (BO.StationExists ex) // the Station already exists
                {
                    MessageBox.Show(ex.Message + string.Format(" Please choose different code than {0}", ex.Code), "Object already exists", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
        }
    }
}
