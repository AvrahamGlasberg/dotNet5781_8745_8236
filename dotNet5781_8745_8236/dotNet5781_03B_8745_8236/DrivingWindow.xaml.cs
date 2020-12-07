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
    /// Interaction logic for DrivingWindow.xaml
    /// </summary>
    public partial class DrivingWindow : Window
    {
        /// <summary>
        /// current bus about to drive
        /// </summary>
        Bus curBus;
        /// <summary>
        /// random 
        /// </summary>
        Random Rand;
        /// <summary>
        /// driving window constructor
        /// </summary>
        /// <param name="cur">the bus about to drive</param>
        public DrivingWindow(Bus cur)
        {
            InitializeComponent();
            curBus = cur;
            Rand = new Random(DateTime.Now.Millisecond);
        }
        /// <summary>
        /// Preview key down event, ignore all keys that aren't numbers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxtBox_Prev_Key_Down(object sender, KeyEventArgs e)
        {
            if (((int)e.Key < (int)Key.D0 || (int)e.Key > (int)Key.D9) && ((int)e.Key < (int)Key.NumPad0 || (int)e.Key > (int)Key.NumPad9) && e.Key != Key.Enter && e.Key != Key.Escape && e.Key != Key.Back)
                e.Handled = true;
        }
        /// <summary>
        /// key pressed event. check for "enter" and to starts the drive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Key_Pressed(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                //missing information from user
                if (txtBox.Text == "")
                    MessageBox.Show("Enter driving distance!");
                else if (passTxtBox.Text == "")
                    MessageBox.Show("Enter number of passengers!");
                else
                {
                    bool canDrive = false;
                    string message = "";
                    int distance = int.Parse(txtBox.Text);
                    int pass = int.Parse(passTxtBox.Text);
                    if (curBus.State == BusState.Driving)
                        message = "Bus is currently driving!";
                    if (curBus.State == BusState.Refueling)
                        message = "Bus is currently Refueling!";
                    if (curBus.State == BusState.Treatment)
                        message = "Bus is currently in Treatment!";
                    if (curBus.State == BusState.Ready)
                    {
                        if (curBus.KmFromFuel + distance > 1200)
                            message = "Bus does not have enough fuel!";
                        else if (curBus.KmFromtreat + distance > 20000)
                            message = "Bus cannot drive this far without treatment!";
                        else if (DateTime.Now.AddYears(-1) > curBus.LastTreat)
                            message = "Bus needs treatment!";
                        else
                        {
                            canDrive = true;
                            int speed = Rand.Next(20, 51);
                            int time = (int)(((double)distance / speed) * 6);
                            curBus.AddKm(distance);
                            //starts drive
                            curBus.Drive(time, pass);
                            //play sound
                            ((MainWindow)(Application.Current.MainWindow)).DriveSound();
                            this.Close();
                        }
                    }
                    if (!canDrive)
                        MessageBox.Show(message);//error message
                }
            }
        }
        /// <summary>
        /// window key down event, checks for esc and shutting down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Key_Down(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Application.Current.Shutdown();
        }
    }
}
