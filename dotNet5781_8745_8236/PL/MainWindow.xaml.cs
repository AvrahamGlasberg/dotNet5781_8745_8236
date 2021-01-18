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
using System.Windows.Navigation;
using System.Windows.Shapes;
using PL.Dialogs;
using BLAPI;
using System.Windows.Media.Animation;
using System.ComponentModel;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL bl;
        private BackgroundWorker worker;
        private TimeSpan startTime;
        int rate;
        public MainWindow()
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
            worker = new BackgroundWorker();
        }
        private void StartAnimation()
        {
            AnimImage.StartAnimationForever(AnimCanvas, 5);
        }
        private void OpenManagerOptions(object sender, RoutedEventArgs e)
        {
            Login dialog = new Login();
            if(dialog.ShowDialog() == true)
            {
                if (dialog.IsAdmin)
                {
                    ManagerPresentation window = new ManagerPresentation();
                    window.Show();
                }
                else
                    MessageBox.Show("You don't have access to manager app.");
            }
        }
        private void OpenUserOptions(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implented yet!");
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            ExitApp();
        }
        private void ExitApp()
        {
            Application.Current.Shutdown();
        }

        private void Button_Mouse_Enter(object sender, MouseEventArgs e)
        {
            (sender as Button).Increase(7);
        }
        private void Button_Mouse_Leave(object sender, MouseEventArgs e)
        {
            (sender as Button).Decrease();
        }

        private void WindowActivated(object sender, EventArgs e)
        {
            StartAnimation();
        }

        private void Start_click(object sender, RoutedEventArgs e)
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
                StartSim.IsEnabled = false;
                StopSim.IsEnabled = true;
                startTime = myTimePicker.SelectedTime.Value.TimeOfDay;
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
            Action action = () => myTimePicker.Text = newTime.ToString();
            Dispatcher.BeginInvoke(action);  // updating the clock
        }


        private void Stop_click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Not implamented yet!");

        }

        private void Numbers_Enter_Only(object sender, KeyEventArgs e)
        {
            if (((int)e.Key < (int)Key.D0 || (int)e.Key > (int)Key.D9) && ((int)e.Key < (int)Key.NumPad0 || (int)e.Key > (int)Key.NumPad9) && e.Key != Key.Enter && e.Key != Key.Escape && e.Key != Key.Back)
                e.Handled = true;
        }
    }
}
