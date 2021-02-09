using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLAPI;

namespace PL
{
    /// <summary>
    /// Interaction logic for NewLineInfo.xaml
    /// </summary>
    public partial class NewLineInfo : Window
    {
        /// <summary>
        /// object that implement IBL
        /// </summary>
        IBL bl;
        /// <summary>
        /// Collection of the station to add to the new line
        /// </summary>
        ObservableCollection<BO.Station> newbusStationsList; 
        /// <summary>
        /// Collection of the remain station to add to the list station of the new line
        /// </summary>
        ObservableCollection<BO.Station> busStationsList;  
        /// <summary>
        /// window ctor
        /// </summary>
        public NewLineInfo()
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
            
            busStationsList = new ObservableCollection<BO.Station>(bl.GetAllBusStations()); // for hidden the station that choose
            newbusStationsList = new ObservableCollection<BO.Station>();
            StationsListBox.DataContext = busStationsList;
            NewStationsListBox.DataContext = newbusStationsList;
            AreasCB.ItemsSource = Enum.GetValues(typeof(BO.Areas));
        }
        /// <summary>
        /// Add the Line to the data
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Add_Line(object sender, RoutedEventArgs e)
        {
            int code;
            string message = "";
            if (!int.TryParse(LineCodeTB.Text, out code) || (newbusStationsList.Count < 2) || AreasCB.SelectedItem == null) // when some data is missing/invalid jumping massage to the user and asking it
            {
                if (!int.TryParse(LineCodeTB.Text, out code))
                    message += "Please enter station's code!\n";
                if (newbusStationsList.Count < 2)
                    message += "Please choose 2 station at least!\n";
                if(AreasCB.SelectedItem == null)
                    message += "Please choose area!\n";
                MessageBox.Show(message, "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                BO.BusLine line = new BO.BusLine();
                line.Area = (BO.Areas)AreasCB.SelectedItem;
                line.LineNumber = int.Parse(LineCodeTB.Text);
                line.LineStations = from station in newbusStationsList
                                    select bl.StationToLineStation(station);
                try
                {
                    bl.AddBusLine(line);
                    this.Close();
                }
                catch(BO.BusLineExists ex) // the BusLine already exists
                {
                    MessageBox.Show(ex.Message + string.Format(" Choose different line number than {0} or change it's route!", ex.LineNumber), "Object already exists", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
        /// <summary>
        /// add the selected station to the Collection of station of the new line
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Add_StationToBusLine(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            BO.BusStation busStation = bt.DataContext as BO.BusStation;
            newbusStationsList.Add(busStation);
            busStationsList.Remove(busStation);
        }

        /// <summary>
        /// allow the user to insert only numbers
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Numbers_Enter_Only(object sender, KeyEventArgs e)
        {
            if (((int)e.Key < (int)Key.D0 || (int)e.Key > (int)Key.D9) && ((int)e.Key < (int)Key.NumPad0 || (int)e.Key > (int)Key.NumPad9) && e.Key != Key.Enter && e.Key != Key.Escape && e.Key != Key.Back)
                e.Handled = true;
        }
        /// <summary>
        /// remove the selected station from the Collection of station of the new line
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Remove_Station(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            BO.BusStation busStation = bt.DataContext as BO.BusStation;
            newbusStationsList.Remove(busStation);
            busStationsList.Add(busStation);
        }
    }
}
