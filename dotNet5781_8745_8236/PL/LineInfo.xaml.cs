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
        ObservableCollection<BO.LineStation> lineStations; // Collection of stations of the line
        ObservableCollection<BO.Station> newStations; // Collection of stations for adding to the line
        ObservableCollection<BO.LineTrip> trips; // Collection of lineTrip of the line
        BO.BusLine curBusLine;
        /// <summary>
        /// ctor of the window that get BO.bus
        /// </summary>
        /// <param name="busLine">bus that sending from the win that call this win</param>
        public LineInfo(BusLine busLine)
        {
            InitializeComponent();
            try
            {
                bl = BLFactory.GetBL();
            }
            catch (BO.MissingData ex) // create BO failed
            {
                MessageBox.Show(ex.Message);
            }
            curBusLine = busLine;
            AreasCB.ItemsSource = Enum.GetValues(typeof(BO.Areas));
            UpdateData();
        }
        /// <summary>
        /// update the data according to the data of the bus that sending to the window
        /// </summary>
        private void UpdateData()
        {
            try
            {
                curBusLine = bl.GetUpdatedBOBusLine(curBusLine.DOLineId);
                lineStations = new ObservableCollection<BO.LineStation>(curBusLine.LineStations);
                newStations = new ObservableCollection<BO.Station>(bl.GetAllStationsNotInLine(curBusLine.DOLineId));
                trips = new ObservableCollection<LineTrip>(bl.GetAllLineTripsInLine(curBusLine));
                mainGrid.DataContext = curBusLine;

                LineDataGrid.ItemsSource = lineStations;
                TripsDataGrid.ItemsSource = trips;
                NewStationsComboBox.DataContext = newStations;
                ExistingStations.DataContext = lineStations;
            }
            catch (BO.BusLineNotFound ex) // can't get the updated bus from bl
            {
                MessageBox.Show(ex.Message + string.Format(" Line: {0}", ex.LineNumber), "Object Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.MissingData ex) // missing data
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// delete station from the line
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Delete_station(object sender, RoutedEventArgs e)
        {
            try
            {
                if (bl.IsTwoStationsInLine(curBusLine.DOLineId)) // delete station cause to delete the line because it one of the last tow station of the line
                {
                    var answer = MessageBox.Show(string.Format("Are you sure you want to delete? this line will be deleted!"), "Attention!", MessageBoxButton.YesNo, MessageBoxImage.Warning); // jumping massage box of warning

                    if (answer == MessageBoxResult.Yes) // when the user aprove the delete, delete the line
                    {
                        bl.DeleteBusLine(curBusLine);
                        this.Closing -= UpdateArea; // no needing event if line is being deleted
                        this.Close();
                    }
                }
                else // the station to delete not one of the two last stations of the line
                {
                    Button bt = sender as Button;
                    BO.LineStation stToDelete = bt.DataContext as BO.LineStation;
                    bl.DeleteLineStation(stToDelete);
                    UpdateData(); // updating the data of the window
                }
            }
            catch (BO.BusLineExists ex) // the line already exist in bl
            {
                MessageBox.Show(ex.Message + string.Format(" wrong line:{0}", ex.LineNumber), "Data error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.BusLineNotFound ex) // can't find the line in bl
            {
                MessageBox.Show(ex.Message + string.Format(" wrong line:{0}", ex.LineNumber), "Data error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.StationExists ex) //  the station already exist in bl
            {
                MessageBox.Show(ex.Message + string.Format(" wrong station:{0}", ex.Code), "Data error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.StationNotFound ex) // can't find the station in bl
            {
                MessageBox.Show(ex.Message + string.Format(" wrong station:{0}", ex.Code), "Data error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// close this window an move back to the sending window 
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void GoBack(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Update manually time and distance between two stations 
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void UpdateTimeDistance(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            BO.LineStation firstSt = bt.DataContext as BO.LineStation;
            var lst = lineStations.ToList<BO.LineStation>();
            int ind = lst.FindIndex(st => st.Code == firstSt.Code);
            if (ind == lst.Count - 1) // when try to update the last station
                MessageBox.Show("Cannot update time & distance to last station!", "Wrong action", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                BO.LineStation secSt = lst[ind + 1];
                UpdateTimeAndDistance win = new UpdateTimeAndDistance(firstSt, secSt); // call to UpdateTimeAndDistance window for update the date there
                win.ShowDialog();
                UpdateData();
            }
        }
        /// <summary>
        /// Update Area of the line
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void UpdateArea(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                bl.UpdateBusLineArea(curBusLine);
            }
            catch (BO.BusLineNotFound ex) // can't find the line
            {
                MessageBox.Show(ex.Message + string.Format(" {0}", ex.LineNumber), "Object not found", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// Add Station before the selected station
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Add_Station_Before(object sender, RoutedEventArgs e)
        {
            AddStation(ExistingStations.SelectedIndex);
        }
        /// <summary>
        /// Add Station after the selected station
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Add_Station_After(object sender, RoutedEventArgs e)
        {
            AddStation(ExistingStations.SelectedIndex + 1);
        }
        /// <summary>
        /// add station to the line after index
        /// </summary>
        /// <param name="index">the index that need to add the station after it</param>
        private void AddStation(int index)
        {
            try
            {
                string message = "";
                if (NewStationsComboBox.SelectedItem == null || ExistingStations.SelectedItem == null) // jumping massage to the user whem some data is missed
                {
                    if (NewStationsComboBox.SelectedItem == null)
                        message = "Please choose station to add!\n";
                    if (ExistingStations.SelectedItem == null)
                        message += "Please choose exisiting station for index!";
                    MessageBox.Show(message, "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
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
        /// <summary>
        /// add index in the dataGrid
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
        /// <summary>
        /// open the adding trip window 
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void AddTrip(object sender, RoutedEventArgs e)
        {
            NewTripInfo win = new NewTripInfo(curBusLine);
            win.ShowDialog();
            UpdateData();
        }
        /// <summary>
        /// open the Delete trip window
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void DeleteTrip(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.DeleteLineTrip((sender as Button).DataContext as BO.LineTrip);
                UpdateData();
            }
            catch (BO.LineTripNotFound ex) // can't find the line in bl
            {
                MessageBox.Show(ex.Message + string.Format(" could not find line {0} at {1} to delete", ex.LineNumber, ex.Start), "Data error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
