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
            bl = BLFactory.GetBL();
        }

        private void Radio_Button_Changed(object sender, RoutedEventArgs e)
        {
            if (StationsRB.IsChecked == true)
                ShowStations();
            else if (LinesRB.IsChecked == true)
                ShowLines();
            else if (BusesRB.IsChecked == true)
                ShowBuses();
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
            MessageBox.Show("Not implented yet!");
        }



        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        //Line
        private void DeleteLine(object sender, RoutedEventArgs e)
        {
            Button bt = sender as Button;
            BO.BusLine LineToDel = bt.DataContext as BO.BusLine;
            bl.DeleteBusLine(LineToDel);
            Lines.Remove(LineToDel);
        }

        //Stations
        private void DeleteStation(object sender, RoutedEventArgs e)
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

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }

        private void Add_Station(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implented yet!");
        }

        private void Add_Line(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implented yet!");
        }

        private void Add_Bus(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implented yet!");
        }
    }
}
