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
    /// </summary>
    public partial class Statistics : Window
    {
        List<Bus> busesLst;
        public Statistics(List<Bus> buses)
        {
            InitializeComponent();
            busesLst = buses;
            UpdateInfo();
        }
        private void UpdateInfo()
        {
            if(busesLst.Count > 0)
            {
                int busesCount = busesLst.Count;
                int totalE = 0;
                int totalP = 0;
                int totalD = 0;
                Bus best = busesLst[0];
                Bus worst = busesLst[0];
                foreach(Bus curBus in busesLst)
                {
                    totalE += curBus.TotalEarnings;
                    totalP += curBus.TotalPass;
                    totalD += curBus.Drives;
                    if (curBus.TotalEarnings > best.TotalEarnings)
                        best = curBus;
                    if (curBus.TotalEarnings < worst.TotalEarnings)
                        worst = curBus;
                }
                NumBuses.Text = busesCount.ToString();
                totalEarnings.Text = totalE.ToString();
                totalPass.Text = totalP.ToString();
                totalDrives.Text = totalD.ToString();
                if(totalD > 0)
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
