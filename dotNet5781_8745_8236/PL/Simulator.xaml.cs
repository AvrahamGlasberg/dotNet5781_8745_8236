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
        MediaPlayer sound = new MediaPlayer();
        int rate;
        TimeSpan startTime;
        BackgroundWorker worker;
        List<BO.LineTiming> lineTimingsList = new List<LineTiming>();
        IEnumerable<BO.LineTiming> lineTimings;
        public Simulator()
        {
            InitializeComponent();
            try
            {
                bl = BLFactory.GetBL();
                StationsDataGrid.ItemsSource = bl.GetAllBusStations();
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
            bl.StopSimulator();
            myTimePicker.IsEnabled = true;
            rateTB.IsEnabled = true;
            StartSimBtn.IsEnabled = true;
            StopSimBtn.IsEnabled = false;
            lineTimingsList = new List<LineTiming>();
            lineTimings = (IEnumerable<LineTiming>)lineTimingsList;
            ElectronicDataGrid.ItemsSource = lineTimings;
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
                sound.Open(new Uri("../../PL/Sounds/drive.mp3", UriKind.Relative));
                sound.Volume = 1;
                sound.Play();

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
            LinesDataGrid.ItemsSource = new ObservableCollection<BO.Line>((StationsDataGrid.SelectedItem as BO.BusStation).LinesInstation);
            lineTimingsList = new List<LineTiming>();
            lineTimings = (IEnumerable<BO.LineTiming>)(lineTimingsList);
            ElectronicDataGrid.ItemsSource = lineTimings;

            LastBus.Content = "";

            bl.SetStationPanel((StationsDataGrid.SelectedItem as BO.BusStation).Code, UpdateElectricPanel);
        }
        public void UpdateElectricPanel(BO.LineTiming lineTiming)
        {
            if (lineTiming.Time == TimeSpan.Zero)
            {
                Action action = () =>
                {
                    lineTimingsList = lineTimings.ToList();
                    lineTimingsList.Remove(lineTimingsList.FirstOrDefault<BO.LineTiming>(l => l.Id == lineTiming.Id));
                    lineTimingsList = new List<LineTiming>(lineTimingsList.OrderBy(l => l.Time));
                    lineTimings = (IEnumerable<BO.LineTiming>)(lineTimingsList);
                    LastBus.Content = lineTiming.LineNumber;
                    ElectronicDataGrid.ItemsSource = lineTimings;
                };
                Dispatcher.BeginInvoke(action);
            }
            else if (lineTimings.FirstOrDefault<BO.LineTiming>(l => l.Id == lineTiming.Id) != null)
            {
                Action action = () =>
                {
                    lineTimingsList = lineTimings.ToList();
                    lineTimingsList.Remove(lineTimingsList.First<BO.LineTiming>(l => l.Id == lineTiming.Id));
                    lineTimingsList.Add(lineTiming);
                    lineTimingsList = new List<LineTiming>(lineTimingsList.OrderBy(l => l.Time));
                    lineTimings = (IEnumerable<BO.LineTiming>)(lineTimingsList);
                    ElectronicDataGrid.ItemsSource = lineTimings;
                };
                Dispatcher.BeginInvoke(action);
            }
            else
            {
                Action action = () =>
                {
                    lineTimingsList = lineTimings.ToList();
                    lineTimingsList.Add(lineTiming);
                    lineTimingsList = new List<LineTiming>(lineTimingsList.OrderBy(l => l.Time));
                    lineTimings = (IEnumerable<BO.LineTiming>)(lineTimingsList);
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
