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
using BO;
using BLAPI;
namespace PL
{
    /// <summary>
    /// Interaction logic for NewTripInfo.xaml
    /// </summary>
    public partial class NewTripInfo : Window
    {
        IBL bl;
        BO.BusLine busLine;
        public NewTripInfo(BO.BusLine busline)
        {
            InitializeComponent();
            try
            {
                bl = BLFactory.GetBL();
                busLine = busline;
            }
            catch (BO.MissingData ex) //creating bo failed
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddTrip(object sender, RoutedEventArgs e)
        {
            TimeSpan start, freq, finish;
            if (!TimeSpan.TryParse(startTB.Text, out start) || !TimeSpan.TryParse(freqTB.Text, out freq) || !TimeSpan.TryParse(finishTB.Text, out finish))
            {
                string message = "";
                if (!TimeSpan.TryParse(startTB.Text, out start))
                    message += "Input valid starting time!\n";
                if(!TimeSpan.TryParse(freqTB.Text, out freq))
                    message += "Input valid frequency!\n";
                if(!TimeSpan.TryParse(finishTB.Text, out finish))
                    message += "Input valid finishing time!";
                MessageBox.Show(message, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    bl.AddLineTrip(new LineTrip() { LineInTrip = busLine, StartAt = start, Frequency = freq, FinishAt = finish });
                    this.Close();
                }
                catch (BO.LineTripExists ex)
                {
                    MessageBox.Show(ex.Message + string.Format(" choose different start than{0} or different line than {0}", ex.Start, ex.LineNumber), "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
