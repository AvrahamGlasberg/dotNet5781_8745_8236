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
    /// Interaction logic for ManagerPresentation.xaml
    /// </summary>
    public partial class ManagerPresentation : Window
    {
        IBL bl;
        private ObservableCollection<BO.BusStation> Stations;
        private ObservableCollection<BO.BusLine> Lines;
        public ManagerPresentation()
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
        }

        private void Radio_Button_Changed(object sender, RoutedEventArgs e)
        {
            if (StationsRB.IsChecked == true)
                ShowStations();
            else if (LinesRB.IsChecked == true)
                ShowLines();
            else if (BusesRB.IsChecked == true)
            {
                ManagerListBox.DataContext = null;
                ShowBuses();
            }
            
        }
        private void HideButtons()
        {
            addStation.Visibility = Visibility.Collapsed;
            addLine.Visibility = Visibility.Collapsed;
            addBus.Visibility = Visibility.Collapsed;
        }
        private void ShowStations()
        {
            HideButtons();
            Stations = new ObservableCollection<BO.BusStation>(bl.GetAllBusStations());
            ManagerListBox.ItemTemplate = (DataTemplate)this.Resources["StationsDataTemplate"];
            ManagerListBox.DataContext = Stations;
            addStation.Visibility = Visibility.Visible;
        }
        private void ShowLines()
        {
            HideButtons();
            Lines = new ObservableCollection<BO.BusLine>(bl.GetAllBusLines());
            ManagerListBox.ItemTemplate = (DataTemplate)this.Resources["LinesDataTemplate"];
            ManagerListBox.DataContext = Lines;
            addLine.Visibility = Visibility.Visible;

        }
        private void ShowBuses()
        {
            HideButtons();
            MessageBox.Show("Not implented yet!");
        }



        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        //Line
        private void DeleteLine(object sender, RoutedEventArgs e)
        {
            try
            {
                Button bt = sender as Button;
                BO.BusLine LineToDel = bt.DataContext as BO.BusLine;
                bl.DeleteBusLine(LineToDel);
                Lines.Remove(LineToDel);
            }
            catch(BO.BusLineNotFound ex)
            {
                MessageBox.Show(ex.Message + string.Format(" wrong {0} Line to delete", ex.LineNumber));
            }
            
        }

        //Stations
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
                if (!check)
                {
                    var answer = MessageBox.Show(string.Format("Are you sure you want to delete? line/s {0} will be deleted", lines), "Attention!", MessageBoxButton.YesNo);

                    if (answer == MessageBoxResult.Yes)
                    {
                        check = true;
                    }
                }
                if (check)
                {
                    bl.DeleteBusStation(station);
                    Stations.Remove(station);
                }
            }
            catch(BO.BusLineNotFound ex)
            {
                MessageBox.Show(ex.Message + string.Format(" wrong Line {0}.", ex.LineNumber));
            }
            catch (BO.StationNotFound ex)
            {
                MessageBox.Show(ex.Message + string.Format(" wrong station{0}", ex.Code));
            }

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void Add_Station(object sender, RoutedEventArgs e)
        {
            NewStationInfo win = new NewStationInfo();
            win.ShowDialog();
            ShowStations();
        }

        private void Add_Line(object sender, RoutedEventArgs e)
        {
                NewLineInfo win = new NewLineInfo();
                win.ShowDialog();
                ShowStations();
        }

        private void Add_Bus(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implented yet!");
        }

        private void Open_Info(object sender, MouseButtonEventArgs e)
        {
            if (BusesRB.IsChecked == true)
                ShowBusesInfo();
            else if (LinesRB.IsChecked == true)
                ShowLinesInfo();
            else if (StationsRB.IsChecked == true)
                ShowStationsInfo();
        }
        private void ShowBusesInfo()
        {
            MessageBox.Show("Not implented yet!");
        }
        private void ShowLinesInfo()
        {
            BO.BusLine busLine = ManagerListBox.SelectedItem as BO.BusLine;
            if (busLine != null)//prevent delete+double click
            {
                LineInfo win = new LineInfo(busLine);
                win.ShowDialog();
                ShowLines();
            }
            // MessageBox.Show("Not implented yet!");
        }
        private void ShowStationsInfo()
        {
            BO.BusStation busStation = ManagerListBox.SelectedItem as BO.BusStation;
            if(busStation != null)//prevent delete+double click
            {
                StationInfo win = new StationInfo(busStation);
                win.ShowDialog();
                ShowStations();
            }
        }
    }
}
