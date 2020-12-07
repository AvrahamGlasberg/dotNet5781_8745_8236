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
using System.Windows.Shapes;

namespace dotNet5781_03B_8745_8236
{
    /// <summary>
    /// Interaction logic for BusData.xaml
    /// </summary>
    public partial class BusData : Window
    {
        Bus curBus;
        public BusData(Bus cur)
        {
            InitializeComponent();
            curBus = cur;
            UpdateInfo();
            updateColor();
        }
        public void UpdateInfo()
        {
            license.Text = curBus.LicToString();
            totalKm.Text = curBus.KmTotal.ToString();
            startD.Text = curBus.Start.ToShortDateString();
            KmRef.Text = curBus.KmFromFuel.ToString();
            Kmtreat.Text = curBus.KmFromtreat.ToString();
            treatD.Text = curBus.LastTreat.ToShortDateString();
            totalE.Text = curBus.TotalEarnings.ToString();
            time.Text = "0";
        }
        public void UpdateTime(int timeToReady)
        {
            time.Text = timeToReady.ToString();
        }
        private void Treatment_Click(object sender, RoutedEventArgs e)
        {
            string message = "";
            bool ready = false;
            if (curBus.State == BusState.Driving)
                message = "Bus is currently driving!";
            else if (curBus.State == BusState.Refueling)
                message = "Bus is currently refueling!";
            else if (curBus.State == BusState.Treatment)
                message = "Bus is currently in treatment!";
            else
                ready = true;
            if (ready)
            {
                curBus.Treatment();
                ((MainWindow)Application.Current.MainWindow).TreatSound();
            }
            else
                MessageBox.Show(message);
            
        }
        private void Refuel_Click(object sender, RoutedEventArgs e)
        {
            string message = "";
            bool ready = false;
            if (curBus.State == BusState.Driving)
                message = "Bus is currently driving!";
            else if (curBus.State == BusState.Refueling)
                message = "Bus is currently refueling!";
            else if (curBus.State == BusState.Treatment)
                message = "Bus is currently in treatment!";
            else
                ready = true;
            if (ready)
            {
                curBus.ReFual();
                ((MainWindow)Application.Current.MainWindow).RefuelSound();
            }
            else
                MessageBox.Show(message);
            
        }

        private void Window_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            curBus.BusDat = null;
        }

        private void Window_Key_Down(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Application.Current.Shutdown();
        }
        public void updateColor()
        {
            Brush br = (default);
            switch (curBus.State)
            {
                case BusState.Ready:
                    br = Brushes.LightGreen;
                    break;
                case BusState.Driving:
                    br = Brushes.LightYellow;
                    break;
                case BusState.Treatment:
                    br = Brushes.PaleVioletRed;
                    break;
                case BusState.Refueling:
                    br = Brushes.LightBlue;
                    break;
            }
            this.Background = br;
        }
    }
}
