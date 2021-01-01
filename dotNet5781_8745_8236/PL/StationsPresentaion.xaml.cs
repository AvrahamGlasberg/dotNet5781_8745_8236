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
using BLAPI;
namespace PL
{
    /// <summary>
    /// Interaction logic for StationsPresentaion.xaml
    /// </summary>
    public partial class StationsPresentaion : Window
    {
        IBL bl = BLFactory.GetBL();
        private ObservableCollection<BO.BusStation> Stations;
        public StationsPresentaion()
        {
            InitializeComponent();
            Stations = new ObservableCollection<BO.BusStation>(bl.GetAllBusStations());
            Start();
        }
        private void Start()
        {
            StationsListBox.DataContext = Stations;
        }
        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
        private void DeleteStation(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            BO.BusStation station = bt.DataContext as BO.BusStation;
            bl.DeleteBusStation(station);
            Stations.Remove(station);
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
    }
}
