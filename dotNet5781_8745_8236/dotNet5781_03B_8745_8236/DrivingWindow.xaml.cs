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
        Bus curBus;
        Random Rand;
        public DrivingWindow(Bus cur)
        {
            InitializeComponent();
            curBus = cur;
            Rand = new Random(DateTime.Now.Millisecond);
        }

        private void TxtBox_Prev_Key_Down(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.D0 && e.Key != Key.D1 && e.Key != Key.D2 && e.Key != Key.D3 && e.Key != Key.D4 && e.Key != Key.D5 && e.Key != Key.D6 && e.Key != Key.D7 && e.Key != Key.D8 && e.Key != Key.D9 && e.Key != Key.Enter && e.Key != Key.Escape && e.Key != Key.Back)
                e.Handled = true;
        }

        private void Key_Pressed(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                bool canDrive = false;
                string message = "";
                int distance = int.Parse(txtBox.Text);
                if (curBus.State == BusState.Driving)
                    message = "Bus is currently driving!";
                if (curBus.State == BusState.Refueling)
                    message = "Bus is currently Refueling!";
                if (curBus.State == BusState.Treatment)
                    message = "Bus is currently in Treatment!";
                if(curBus.State == BusState.Ready)
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
                        curBus.Drive(time);
                        this.Close();
                    }
                }
                if (!canDrive)
                    MessageBox.Show(message);
            }
        }
    }
}
