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
    /// Interaction logic for Statistics.xaml
    /// this class use for represent the Statistics data of the buses.
    /// </summary>
    public partial class Statistics : Window
    {
        /// <summary>
        /// the list of the bus for make the Statistics on them
        /// </summary>
        List<Bus> busesLst;
        /// <summary>
        /// ctor that get the list of buses
        /// </summary>
        /// <param name="buses">parameter of list of buses</param>
        public Statistics(List<Bus> buses)
        {
            InitializeComponent();
            busesLst = buses;
            UpdateInfo();
        }
        /// <summary>
        /// this function make all the calculate on the bus list
        /// </summary>
        private void UpdateInfo()
        {
            if(busesLst.Count > 0) // the list isn't empty
            {
                // initilizing the parameters:
                int busesCount = busesLst.Count;
                int totalE = 0;
                int totalP = 0;
                int totalD = 0;
                Bus best = busesLst[0];
                Bus worst = busesLst[0];
                foreach(Bus curBus in busesLst)
                {
                    totalE += curBus.TotalEarnings; // counting the amount of earning
                    totalP += curBus.TotalPass;// counting the amount of passengers
                    totalD += curBus.Drives;// counting the amount of drives
                    if (curBus.TotalEarnings > best.TotalEarnings) 
                        best = curBus; // put in the best parameter the most earning bus
                    if (curBus.TotalEarnings < worst.TotalEarnings)
                        worst = curBus;  // put in the best parameter the worst earning bus
                }
                // update the calculated parameters in the match places in the window:
                NumBuses.Text = busesCount.ToString();
                totalEarnings.Text = totalE.ToString();
                totalPass.Text = totalP.ToString();
                totalDrives.Text = totalD.ToString();
                if(totalD > 0) // if any drive happends yet cannot calculate the averge
                {
                    avgPass.Text = ((double)totalP / totalD).ToString();
                    avgEarn.Text = ((double)totalE / totalD).ToString();
                }
                else
                {
                    avgPass.Text = "No drives happened yet.";
                    avgEarn.Text = "No drives happened yet.";
                }
                bestBus.Text = best.LicToString();
                worstBus.Text = worst.LicToString();
            }
        }
    }
}
