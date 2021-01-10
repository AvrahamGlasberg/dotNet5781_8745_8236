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
        ObservableCollection<BO.Station> newStations;
        BO.BusLine curBusLine;
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
            curBusLine = busLine;
            lineStations = new ObservableCollection<BO.LineStation>(busLine.LineStations);
            newStations = new ObservableCollection<BO.Station>(bl.GetAllStationsNotInLine(curBusLine.DOLineId));
            AreasCB.ItemsSource = Enum.GetValues(typeof(BO.Areas));
            mainGrid.DataContext = busLine;
            StationsListBox.DataContext = lineStations;
            NewStationsComboBox.DataContext = newStations;
            ExistingStations.DataContext = lineStations;
        }

        private void Delete_station(object sender, RoutedEventArgs e)
        {
            if(bl.IsTwoStationsInLine(curBusLine.DOLineId))
            {
                var answer = MessageBox.Show(string.Format("Are you sure you want to delete? this line will be deleted!"), "Attention!", MessageBoxButton.YesNo);

                if (answer == MessageBoxResult.Yes)
                {
                    bl.DeleteBusLine(curBusLine);
                    this.Close();
                }
            }
            else
            {
                Button bt = sender as Button;
                BO.LineStation stToDelete = bt.DataContext as BO.LineStation;
                bl.DeleteLineStation(stToDelete);
                lineStations.Remove(stToDelete);
            }
            //MessageBox.Show("Not implamented yet!");
        }

        private void Update_Line(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implamented yet!");
        }

        private void UpdateTimeDistance(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implamented yet!");
        }

        private void AddStation(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implamented yet!");
        }

        private void Add_Staton(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implamented yet!");
        }
    }
}
