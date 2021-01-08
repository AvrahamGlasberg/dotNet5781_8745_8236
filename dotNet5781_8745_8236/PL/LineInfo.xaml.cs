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
    /// Interaction logic for LinesInfo.xaml
    /// </summary>

    
    public partial class LineInfo : Window
    {
        IBL bl;
        ObservableCollection<BO.LineStation> lineStations;
        public LineInfo(BusLine busLine)
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
            mainGrid.DataContext = busLine;
            lineStations = new ObservableCollection<BO.LineStation>(busLine.LineStations);
            StationsListBox.ItemTemplate = (DataTemplate)this.Resources["LineDataTemplate"];
            StationsListBox.DataContext = lineStations;
        }

        private void Delete_station(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implamented yet!");
        }

        private void station_Double_click(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Not implamented yet!");
            //BO.BusStation busStation = StationsListBox.SelectedItem as BO.BusStation;
            //if (busStation != null)//prevent delete+double click
            //{
            //    StationInfo win = new StationInfo(busStation);
            //    win.ShowDialog();
        
        }
    }
}
