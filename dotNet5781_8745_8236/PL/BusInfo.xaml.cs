﻿using System;
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
using BO;
using BLAPI;
namespace PL
{
    /// <summary>
    /// Interaction logic for BusInfo.xaml
    /// </summary>
    public partial class BusInfo : Window
    {
        IBL bl;
        BO.Bus Curbus;
        public BusInfo(BO.Bus bus)
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
            Curbus = bus;
            MainGrid.DataContext = Curbus;
        }

        private void Start_Refuel(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.Refuel(Curbus);
                this.Close();
            }
            catch(BO.BusNotFound ex)
            {
                MessageBox.Show(ex.Message + string.Format(" Bus {0} could not be found.", ex.License));
            }
        }

        private void Start_Treatment(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.Treatment(Curbus);
                this.Close();
            }
            catch (BO.BusNotFound ex)
            {
                MessageBox.Show(ex.Message + string.Format(" Bus {0} could not be found.", ex.License));
            }
        }
    }
}