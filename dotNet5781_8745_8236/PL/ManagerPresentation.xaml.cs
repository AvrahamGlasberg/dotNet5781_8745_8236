using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BLAPI;
using BO;
namespace PL
{
    /// <summary>
    /// Interaction logic for ManagerPresentation.xaml
    /// </summary>
    public partial class ManagerPresentation : Window
    {
        /// <summary>
        /// object that implement IBL
        /// </summary>
        IBL bl;
        /// <summary>
        /// Collection of BusStation for Presentation
        /// </summary>
        private ObservableCollection<BO.BusStation> Stations;
        /// <summary>
        /// Collection of BusLine for Presentation
        /// </summary>
        private ObservableCollection<BO.BusLine> Lines;
        /// <summary>
        /// Collection of Bus for Presentation
        /// </summary>
        private ObservableCollection<BO.Bus> Buses;
        /// <summary>
        /// ctor of the window 
        /// </summary>
        public ManagerPresentation()
        {
            InitializeComponent();
            try
            {
                bl = BLFactory.GetBL();
            }
            catch (BO.MissingData ex) //creating BO failed
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// show the info in the window according to the selected in the radio button
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Radio_Button_Changed(object sender, RoutedEventArgs e)
        {
            if (StationsRB.IsChecked == true)
                ShowStations();
            else if (LinesRB.IsChecked == true)
                ShowLines();
            else if (BusesRB.IsChecked == true)
                ShowBuses();
        }
        #region stations 
        /// <summary>
        /// open window for adding station
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Add_Station(object sender, RoutedEventArgs e)
        {
            NewStationInfo win = new NewStationInfo();
            win.ShowDialog();
            ShowStations();
        }
        /// <summary>
        /// open the window of station info
        /// </summary>
        private void ShowStationsInfo()
        {
            BO.BusStation busStation = StationsDataGrid.SelectedItem as BO.BusStation;
            if (busStation != null)//prevent delete+double click
            {
                StationInfo win = new StationInfo(busStation);
                win.ShowDialog();
                ShowStations();
            }
        }
        /// <summary>
        /// Show the stations on the dataGrid
        /// </summary>
        private void ShowStations()
        {
            try
            {
                Stations = new ObservableCollection<BO.BusStation>(bl.GetAllBusStations());
                StationsDataGrid.ItemsSource = Stations;
            }
            catch (BO.MissingData ex) // missing data 
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        ///  delete station from the data
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void DeleteStation(object sender, RoutedEventArgs e)
        {
            try
            {
                Button bt = sender as Button;
                BO.BusStation station = bt.DataContext as BO.BusStation;

                bool check = true;
                string lines = "";
                foreach (var line in station.LinesInstation)
                {
                    if (bl.IsTwoStationsInLine(line.DOLineId))
                    {
                        check = false;
                        lines += line.LineNumber.ToString() + ' ';
                    }
                }
                if (!check) // warning the user from delete station the cause to delete line 
                {
                    var answer = MessageBox.Show(string.Format("Are you sure you want to delete? line/s {0} will be deleted", lines), "Attention!", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (answer == MessageBoxResult.Yes)
                    {
                        check = true;
                    }
                }
                if (check)
                {
                    bl.DeleteBusStation(station);
                    ShowStations();
                }
            }
            catch (BO.BusLineNotFound ex)
            {
                MessageBox.Show(ex.Message + string.Format(" wrong Line {0}.", ex.LineNumber), "Object not found", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BO.StationNotFound ex)
            {
                MessageBox.Show(ex.Message + string.Format(" wrong station{0}", ex.Code), "Object not found", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region buses
        /// <summary>
        /// open window for adding Line
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Add_Line(object sender, RoutedEventArgs e)
        {
            NewLineInfo win = new NewLineInfo();
            win.ShowDialog();
            ShowLines();
        }
        /// <summary>
        /// delete Bus from the data
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void DeleteBus(object sender, RoutedEventArgs e)
        {
            try
            {
                Button bt = sender as Button;
                bl.DeleteBus(bt.DataContext as BO.Bus);
                ShowBuses();
            }
            catch (BO.BusNotFound ex) // can't find the bus
            {
                MessageBox.Show(ex.Message + string.Format(" wrong license: {0}", ex.License), "Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// open the window of the bus info
        /// </summary>
        private void ShowBusesInfo()
        {
            BO.Bus bus = BusesDataGrid.SelectedItem as BO.Bus;
            if (bus != null)
            {
                BusInfo win = new BusInfo(bus);
                win.ShowDialog();
                ShowBuses();
            }
        }
        /// <summary>
        /// Show the Buses on the dataGrid
        /// </summary>
        private void ShowBuses()
        {
            try
            {
                Buses = new ObservableCollection<Bus>(bl.GetAllBuses());
                BusesDataGrid.ItemsSource = Buses;
            }
            catch (BO.MissingData ex) // missing data
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region lines
        /// <summary>
        /// Show the Lines on the dataGrid
        /// </summary>
        private void ShowLines()
        {
            try
            {
                Lines = new ObservableCollection<BO.BusLine>(bl.GetAllBusLines());
                LinesDataGrid.ItemsSource = Lines;
            }
            catch (BO.MissingData ex) // missing data
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// open the window of the busLine info
        /// </summary>
        private void ShowLinesInfo()
        {
            BO.BusLine busLine = LinesDataGrid.SelectedItem as BO.BusLine;
            
            if (busLine != null)//prevent delete+double click
            {
                LineInfo win = new LineInfo(busLine);
                win.ShowDialog();
                ShowLines();
            }
        }
        /// <summary>
        /// open window for adding Bus
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Add_Bus(object sender, RoutedEventArgs e)
        {
            NewBusInfo win = new NewBusInfo();
            win.ShowDialog();
            ShowBuses();
        }
        /// <summary>
        /// delete BusLine from the data
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void DeleteLine(object sender, RoutedEventArgs e)
        {
            try
            {
                Button bt = sender as Button;
                BO.BusLine LineToDel = bt.DataContext as BO.BusLine;
                bl.DeleteBusLine(LineToDel);
                ShowLines();
            }
            catch (BO.BusLineNotFound ex) // can't find the BusLine
            {
                MessageBox.Show(ex.Message + string.Format(" wrong {0} Line to delete", ex.LineNumber), "Object not found", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion
        /// <summary>
        /// open the window info of the selected checked in the radio button
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Open_Info(object sender, MouseButtonEventArgs e)
        {
            if (BusesRB.IsChecked == true)
                ShowBusesInfo();
            else if (LinesRB.IsChecked == true)
                ShowLinesInfo();
            else if (StationsRB.IsChecked == true)
                ShowStationsInfo();
        }
        /// <summary>
        /// close this window an move back to the sending window 
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
        /// <summary>
        /// adding index to the dataGrid
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

    }
}
