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
        }
        public void UpdateInfo()
        {
            license.Text = curBus.LicToString();
            totalKm.Text = curBus.KmTotal.ToString();
            startD.Text = curBus.Start.ToShortDateString();
            KmRef.Text = curBus.KmFromFuel.ToString();
            Kmtreat.Text = curBus.KmFromtreat.ToString();
            treatD.Text = curBus.LastTreat.ToShortDateString();
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
                curBus.Treatment();
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
                curBus.ReFual();
            else
                MessageBox.Show(message);
        }

        private void Window_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            curBus.BusDat = null;
        }
    }
}
