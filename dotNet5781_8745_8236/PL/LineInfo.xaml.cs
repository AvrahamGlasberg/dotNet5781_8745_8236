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
            AreasCB.ItemsSource = Enum.GetValues(typeof(BO.Areas));
            UpdateData();
        }
        private void UpdateData()
        {
            try
            {
                curBusLine = bl.GetUpdatedBOBusLine(curBusLine.DOLineId);
                lineStations = new ObservableCollection<BO.LineStation>(curBusLine.LineStations);
                newStations = new ObservableCollection<BO.Station>(bl.GetAllStationsNotInLine(curBusLine.DOLineId));
                mainGrid.DataContext = curBusLine;

                LineDataGrid.ItemsSource = lineStations;

                //StationsListBox.DataContext = lineStations;
                NewStationsComboBox.DataContext = newStations;
                ExistingStations.DataContext = lineStations;
            }
            catch(BO.BusLineNotFound ex)
            {
                MessageBox.Show(ex.Message + string.Format(" Line: {0}", ex.LineNumber));
            }
            catch(BO.MissingData ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Delete_station(object sender, RoutedEventArgs e)
        {
            if(bl.IsTwoStationsInLine(curBusLine.DOLineId))
            {
                var answer = MessageBox.Show(string.Format("Are you sure you want to delete? this line will be deleted!"), "Attention!", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (answer == MessageBoxResult.Yes)
                {
                    bl.DeleteBusLine(curBusLine);
                    this.Closing -= UpdateArea; // no needing event if line is being deleted
                    this.Close();
                }
            }
            else
            {
                Button bt = sender as Button;
                BO.LineStation stToDelete = bt.DataContext as BO.LineStation;
                bl.DeleteLineStation(stToDelete);
                UpdateData();
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateTimeDistance(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            BO.LineStation firstSt = bt.DataContext as BO.LineStation;
            var lst = lineStations.ToList<BO.LineStation>();
            int ind = lst.FindIndex(st=>st.Code==firstSt.Code);
            if (ind == lst.Count - 1)
                MessageBox.Show("Cannot update time & distance to last station!");
            else
            {
                BO.LineStation secSt = lst[ind + 1];
                UpdateTimeAndDistance win = new UpdateTimeAndDistance(firstSt, secSt);
                win.ShowDialog();
                UpdateData();
            }
        }
        private void UpdateArea(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                bl.UpdateBusLineArea(curBusLine);
            }
            catch (BO.BusLineNotFound ex)
            {
                MessageBox.Show(ex.Message + string.Format(" {0}", ex.LineNumber));
            }
        }

        private void Add_Station_Before(object sender, RoutedEventArgs e)
        {
            AddStation(ExistingStations.SelectedIndex);
        }
        private void Add_Station_After(object sender, RoutedEventArgs e)
        {
            AddStation(ExistingStations.SelectedIndex + 1);
        }
        private void AddStation(int index)
        {
            try
            {
                string message = "";
                if (NewStationsComboBox.SelectedItem == null || ExistingStations.SelectedItem == null)
                {
                    if (NewStationsComboBox.SelectedItem == null)
                        message = "Please choose station to add!\n";
                    if (ExistingStations.SelectedItem == null)
                        message += "Please choose exisiting station for index!";
                    MessageBox.Show(message);
                }
                else
                {
                    BO.Station newStation = NewStationsComboBox.SelectedItem as BO.Station;
                    bl.AddLineStationToBusLine(curBusLine, newStation, index);
                    UpdateData();
                }
            }
            catch (BO.MissingData ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
