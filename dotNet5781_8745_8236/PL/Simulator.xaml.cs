using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for Simulator.xaml
    /// </summary>
    public partial class Simulator : Window
    {
        IBL bl;
        int rate;
        TimeSpan startTime;
        BackgroundWorker worker;
        List<BO.LineTiming> lineTimings = new List<LineTiming>();
        public Simulator()
        {
            InitializeComponent();
            try
            {
                bl = BLFactory.GetBL();
                StationsDataGrid.ItemsSource = bl.GetAllBusStations();
                //StationsDataGrid.ItemsSource = bl.GetAllBusStations();
                ElectronicDataGrid.ItemsSource = lineTimings;
            }
            catch (BO.MissingData ex) //creating bo failed
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Mouse_Enter(object sender, MouseEventArgs e)
        {
            (sender as Button).Increase(7);
        }

        private void Button_Mouse_Leave(object sender, MouseEventArgs e)
        {
            (sender as Button).Decrease();
        }

        private void Numbers_Enter_Only(object sender, KeyEventArgs e)
        {
            if (((int)e.Key < (int)Key.D0 || (int)e.Key > (int)Key.D9) && ((int)e.Key < (int)Key.NumPad0 || (int)e.Key > (int)Key.NumPad9) && e.Key != Key.Enter && e.Key != Key.Escape && e.Key != Key.Back)
                e.Handled = true;
        }

        private void Stop_sim(object sender, RoutedEventArgs e)
        {
            StopSimulation();
        }
        private void StopSimulation()
        {
            myTimePicker.IsEnabled = true;
            rateTB.IsEnabled = true;
            StartSimBtn.IsEnabled = true;
            StopSimBtn.IsEnabled = false;
            bl.StopSimulator();
        }
        private void Start_sim(object sender, RoutedEventArgs e)
        {
            if (myTimePicker.SelectedTime == null || !int.TryParse(rateTB.Text, out rate))
            {
                string message = "";
                if (myTimePicker.SelectedTime == null)
                    message += "Please select a time!\n";
                if (!int.TryParse(rateTB.Text, out rate) || rate == 0)
                    message += "Please enter valid rate!";

                MessageBox.Show(message, "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //myTimePicker.Text = myTimePicker.SelectedTime.Value.TimeOfDay.ToString();
                myTimePicker.IsEnabled = false;
                rateTB.IsEnabled = false;
                StartSimBtn.IsEnabled = false;
                StopSimBtn.IsEnabled = true;
                startTime = myTimePicker.SelectedTime.Value.TimeOfDay;
                worker = new BackgroundWorker();
                worker.DoWork += Worker_DoWork;
                worker.RunWorkerAsync();
            }
        }
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            bl.StartSimulator(startTime, rate, UpdateClock);
        }
        public void UpdateClock(TimeSpan newTime)
        {
            Action action = () => myTimePicker.Text = newTime.ToString().Substring(0, 8);
            Dispatcher.BeginInvoke(action);  // updating the clock
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            StopSimulation();
            bl.StopSimulator();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            AnimImage.StartAnimationForever(AnimCanvas, 5);
        }

        private void Station_Selected(object sender, SelectionChangedEventArgs e)
        {
            LinesDataGrid.ItemsSource = (StationsDataGrid.SelectedItem as BO.BusStation).LinesInstation;
            lineTimings.Clear();
            ElectronicDataGrid.ItemsSource = lineTimings;
            bl.SetStationPanel((StationsDataGrid.SelectedItem as BO.BusStation).Code, UpdateElectricPanel);
        }
        public void UpdateElectricPanel(BO.LineTiming lineTiming)
        {
            if (lineTiming.Time == TimeSpan.Zero)
            {
                Action action = () =>
                {
                    lineTimings.Remove(lineTimings.FirstOrDefault<BO.LineTiming>(l => l.Id == lineTiming.Id));
                    lineTimings = new List<LineTiming>(lineTimings.OrderBy(l => l.Time));
                    LastBus.Content = lineTiming.LineNumber;
                    ElectronicDataGrid.ItemsSource = lineTimings;
                };
                Dispatcher.BeginInvoke(action);
            }
            else if (lineTimings.FirstOrDefault<BO.LineTiming>(l => l.Id == lineTiming.Id) != null)
            {
                Action action = () =>
                {
                    lineTimings.Remove(lineTimings.First<BO.LineTiming>(l => l.Id == lineTiming.Id));
                    lineTimings.Add(lineTiming);
                    lineTimings = new List<LineTiming>(lineTimings.OrderBy(l => l.Time));
                    ElectronicDataGrid.ItemsSource = lineTimings;
                };
                Dispatcher.BeginInvoke(action);
            }
            else
            {
                Action action = () =>
                {
                    lineTimings.Add(lineTiming);
                    lineTimings = new List<LineTiming>(lineTimings.OrderBy(l => l.Time));
                    ElectronicDataGrid.ItemsSource = lineTimings;
                };
                Dispatcher.BeginInvoke(action);
            }
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {

            bl.StopSimulator();
            MainWindow window = new MainWindow();
            window.Show();
            this.Close();
        }
    }
}
