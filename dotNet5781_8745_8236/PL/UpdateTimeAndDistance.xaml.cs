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
using BLAPI;
using BO;
namespace PL
{
    /// <summary>
    /// Interaction logic for UpdateTimeAndDistance.xaml
    /// </summary>
    public partial class UpdateTimeAndDistance : Window
    {
        IBL bl;
        BO.LineStation firstStation, secondStation;
        public UpdateTimeAndDistance(BO.LineStation first, BO.LineStation second)
        {
            InitializeComponent();
            try
            {
                bl = BLFactory.GetBL();
            }
            catch (BO.MissingData ex)
            {
                MessageBox.Show(ex.Message);
            }
            firstStation = first;
            secondStation = second;

            MainGrid.DataContext = first;
            SecondSt.DataContext = second;
        }

        private void Numbers_Enter_Only(object sender, KeyEventArgs e)
        {
            if (((int)e.Key < (int)Key.D0 || (int)e.Key > (int)Key.D9) && ((int)e.Key < (int)Key.NumPad0 || (int)e.Key > (int)Key.NumPad9) && e.Key != Key.Enter && e.Key != Key.Escape && e.Key != Key.Back && e.Key != Key.OemPeriod && e.Key != Key.Oem1)
                e.Handled = true;
        }

        private void UpdateInfo(object sender, RoutedEventArgs e)
        {
            try
            {
                string message = "";
                if (!double.TryParse(DisTB.Text, out _) || !TimeSpan.TryParse(TimeTB.Text, out _))
                {
                    if (!double.TryParse(DisTB.Text, out _))
                        message += "Please enter correct distance!\n";
                    if (!TimeSpan.TryParse(TimeTB.Text, out _))
                        message += "Please enter correct time!";
                    MessageBox.Show(message);
                }
                else
                {
                    bl.UpdateTimeAndDis(firstStation, secondStation);
                    this.Close();
                }
            }
            catch (BO.MissingData ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
