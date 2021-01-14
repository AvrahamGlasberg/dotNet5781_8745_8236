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
using System.Collections.ObjectModel;
using Microsoft.Maps.MapControl.WPF;
using BLAPI;
using BO;
using System.Device.Location;
namespace PL
{
    /// <summary>
    /// Interaction logic for StationInfo.xaml
    /// </summary>
    public partial class StationInfo : Window
    {
        IBL bl;
        BO.BusStation busStation;
        ObservableCollection<BO.Line> lines;
        
        public StationInfo(BO.BusStation station)
        {
            InitializeComponent();
            try
            {
                bl = BLFactory.GetBL();
            }
            catch(BO.MissingData ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }
            busStation = station;
            lines = new ObservableCollection<BO.Line>(busStation.LinesInstation);
            MainGrid.DataContext = busStation;
            LinesDataGrid.ItemsSource = lines;
            //WRONG!!!
            MyMap.Center = new Location(busStation.Position.Latitude, busStation.Position.Longitude);
            UpdateBtn.IsEnabled = false;
        }
        private void Map_Double_click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point p = e.GetPosition(MyMap);
            Location l = MyMap.ViewportPointToLocation(p);
            Pin.Location = l;
            UpdateBtn.IsEnabled = true;
        }

        private void Update(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateBusStation(busStation);
                this.Close();
            }
            catch(BO.StationNotFound ex)
            {
                MessageBox.Show(ex.Message + string.Format(" wrong code: {0}", ex.Code), "data error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(BO.MissingData ex)
            {
                MessageBox.Show(ex.Message, "data error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangeText(object sender, TextChangedEventArgs e)
        {
            if((sender as TextBox).Text != busStation.Name)
                UpdateBtn.IsEnabled = true;
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
