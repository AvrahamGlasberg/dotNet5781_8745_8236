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
        ObservableCollection<BO.BusStation> NewbusStationsList;
        ObservableCollection<BO.BusStation> busStationsList;
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
            
            busStationsList = new ObservableCollection<BO.BusStation>(bl.GetAllBusStations()); // for hidden the station that choose
            NewbusStationsList = new ObservableCollection<BO.BusStation>();
            StationsListBox.ItemTemplate = (DataTemplate)this.Resources["StationDataTemplate"];
            StationsListBox.DataContext = busStationsList;
            NewStationsListBox.ItemTemplate = (DataTemplate)this.Resources["NewStationDataTemplate"];
            NewStationsListBox.DataContext = NewbusStationsList;
        }

        private void Add_Line(object sender, RoutedEventArgs e)
        {
            int code;
            string message = "";
            if (!int.TryParse(LineCodeTB.Text, out code) || (NewbusStationsList.Count <= 2))
            {
                if (!int.TryParse(LineCodeTB.Text, out code))
                    message += "Please enter station's code!\n";
                if (NewbusStationsList.Count <= 2)
                    message += "Please choose 3 station at least!";
                MessageBox.Show(message);
            }
            else
            {
                MessageBox.Show("Not implamented yet!");
                // more check that the code number is exist? 
                //bl.addBusLine(new BO.BusLine() { });
            }

        }

        private void Add_StationToBusLine(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            BO.BusStation busStation = bt.DataContext as BO.BusStation;
            NewbusStationsList.Add(busStation);
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
            NewbusStationsList.Remove(busStation);
            busStationsList.Add(busStation);
        }
    }
}
