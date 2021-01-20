using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for NewLineInfo.xaml
    /// </summary>
    public partial class NewLineInfo : Window
    {
        IBL bl;
        ObservableCollection<BO.Station> newbusStationsList;
        ObservableCollection<BO.Station> busStationsList;
        public NewLineInfo()
        {
            InitializeComponent();
            try
            {
                bl = BLFactory.GetBL();
            }
            catch (BO.MissingData ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            busStationsList = new ObservableCollection<BO.Station>(bl.GetAllBusStations()); // for hidden the station that choose
            newbusStationsList = new ObservableCollection<BO.Station>();
            StationsListBox.DataContext = busStationsList;
            NewStationsListBox.DataContext = newbusStationsList;
            AreasCB.ItemsSource = Enum.GetValues(typeof(BO.Areas));
        }

        private void Add_Line(object sender, RoutedEventArgs e)
        {
            int code;
            string message = "";
            if (!int.TryParse(LineCodeTB.Text, out code) || (newbusStationsList.Count < 2) || AreasCB.SelectedItem == null)
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
                catch(BO.BusLineExists ex)
                {
                    MessageBox.Show(ex.Message + string.Format(" Choose different line number than {0} or change it's route!", ex.LineNumber), "Object already exists", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

        private void Add_StationToBusLine(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            BO.BusStation busStation = bt.DataContext as BO.BusStation;
            newbusStationsList.Add(busStation);
            busStationsList.Remove(busStation);
        }

    
        private void Numbers_Enter_Only(object sender, KeyEventArgs e)
        {
            if (((int)e.Key < (int)Key.D0 || (int)e.Key > (int)Key.D9) && ((int)e.Key < (int)Key.NumPad0 || (int)e.Key > (int)Key.NumPad9) && e.Key != Key.Enter && e.Key != Key.Escape && e.Key != Key.Back)
                e.Handled = true;
        }

        private void Remove_Station(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            BO.BusStation busStation = bt.DataContext as BO.BusStation;
            newbusStationsList.Remove(busStation);
            busStationsList.Add(busStation);
        }
    }
}
