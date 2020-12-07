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
    /// Interaction logic for NewBusInfo.xaml
    /// </summary>
    public partial class NewBusInfo : Window
    {
        public NewBusInfo()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool legalInput = false;
            int license = 0, km = 0;
            DateTime start = DateTime.Now;
            if (!int.TryParse(licenseNumber.Text, out license) || (license.ToString().Length != 7 && license.ToString().Length != 8)
                || license < 0)
                MessageBox.Show("Illegal licence number!");
            else if (!int.TryParse(totalKm.Text, out km) || km < 0)
                MessageBox.Show("Illegal Km!");
            else if (!DateTime.TryParse(startDate.Text, out start))
                MessageBox.Show("Illegal Date!");
            else if ((start.Year >= 2018 && license.ToString().Length == 7) ||
                (start.Year < 2018 && license.ToString().Length == 8))
                MessageBox.Show("License number does not match the bus's starting year!");
            else if (start > DateTime.Now || start.Year < 1900)
                MessageBox.Show("Input normal starting date!");
            else if (((MainWindow)System.Windows.Application.Current.MainWindow).BusExists(license))
                MessageBox.Show("Bus with that licesnse number already exists!");

            else
                legalInput = true;
            if (legalInput)
            {
                Bus newB = new Bus(license, start, km);
                ((MainWindow)System.Windows.Application.Current.MainWindow).AddBus(newB);
                this.Close();
            }
        }

        private void Prev_Key_Down_Number(object sender, KeyEventArgs e)
        {
            if(((int)e.Key < (int)Key.D0 || (int)e.Key > (int)Key.D9) && ((int)e.Key < (int)Key.NumPad0 || (int)e.Key > (int)Key.NumPad9) && e.Key != Key.Enter && e.Key != Key.Escape && e.Key != Key.Back)
                e.Handled = true;
        }

        private void Prev_Key_Down_Date(object sender, KeyEventArgs e)
        {
            if (((int)e.Key < (int)Key.D0 || (int)e.Key > (int)Key.D9) && ((int)e.Key < (int)Key.NumPad0 || (int)e.Key > (int)Key.NumPad9) && e.Key != Key.Enter && e.Key != Key.Escape && e.Key != Key.Back && e.Key != Key.OemQuestion)
                e.Handled = true;
        }

        private void Window_Key_Down(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Application.Current.Shutdown();
        }
    }
}
