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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BLAPI;
namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenManagerOptions(object sender, RoutedEventArgs e)
        {
            ShowManagerOptions();
        }
        private void OpenUserOptions(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implented yet!");
        }
        private void ShowManagerOptions()
        {
            HideAll();
            BusesBtn.Visibility = Visibility.Visible;
            LinesBtn.Visibility = Visibility.Visible;
            StationsBtn.Visibility = Visibility.Visible;
            BackBtn.Visibility = Visibility.Visible;
        }
        private void HideAll()
        {
            ManagerBtn.Visibility = Visibility.Collapsed;
            UserBtn.Visibility = Visibility.Collapsed;
            ExitBtn.Visibility = Visibility.Collapsed;
            BusesBtn.Visibility = Visibility.Collapsed;
            LinesBtn.Visibility = Visibility.Collapsed;
            StationsBtn.Visibility = Visibility.Collapsed;
            BackBtn.Visibility = Visibility.Collapsed;
        }
        
        private void ShowBuses(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implented yet!");
        }
        private void ShowLines(object sender, RoutedEventArgs e)
        {
            LinesPresentaion window = new LinesPresentaion();
            window.Show();
            this.Close();
        }
        private void ShowStations(object sender, RoutedEventArgs e)
        {
            StationsPresentaion window = new StationsPresentaion();
            window.Show();
            this.Close();
        }
        private void Back(object sender, RoutedEventArgs e)
        {
            HideAll();
            ManagerBtn.Visibility = Visibility.Visible;
            UserBtn.Visibility = Visibility.Visible;
            ExitBtn.Visibility = Visibility.Visible;
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            ExitApp();
        }
        private void ExitApp()
        {
            Application.Current.Shutdown();
        }
    }
}
