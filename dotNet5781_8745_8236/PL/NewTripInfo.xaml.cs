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
        /// <summary>
        /// window ctor
        /// </summary>
        /// <param name="busline"></param>
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
        /// <summary>
        /// Add trip to the data
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void AddTrip(object sender, RoutedEventArgs e)
        {
            int freq;
            if (startTP.SelectedTime == null || !int.TryParse(freqTB.Text, out freq) || finishTP.SelectedTime == null) // when some data is missing / Invalid jumping massage to the user and asking it
            {
                string message = "";
                if (startTP.SelectedTime == null)
                    message += "Input valid starting time!\n";
                if(!int.TryParse(freqTB.Text, out freq))
                    message += "Input valid frequency!\n";
                if(finishTP.SelectedTime == null)
                    message += "Input valid finishing time!";
                MessageBox.Show(message, "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if(finishTP.SelectedTime.Value.TimeOfDay < startTP.SelectedTime.Value.TimeOfDay)
            {
                MessageBox.Show("Please select logical start & finish time!", "Inlogical input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    TimeSpan frequency;
                    if (freq == 0)
                        frequency = TimeSpan.Zero;
                    else
                        frequency = new TimeSpan(0, freq, 0);
                    bl.AddLineTrip(new LineTrip() { LineInTrip = busLine, StartAt = startTP.SelectedTime.Value.TimeOfDay, Frequency = frequency, FinishAt = finishTP.SelectedTime.Value.TimeOfDay });
                    this.Close();
                }
                catch (BO.LineTripExists ex) // the LineTrip already exists
                {
                    MessageBox.Show(ex.Message + string.Format(" choose different start than{0} or different line than {0}", ex.Start, ex.LineNumber), "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        /// <summary>
        /// allow the user to insert only numbers
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Numbers_Enter_Only(object sender, KeyEventArgs e)
        {
            if (((int)e.Key < (int)Key.D0 || (int)e.Key > (int)Key.D9) && ((int)e.Key < (int)Key.NumPad0 || (int)e.Key > (int)Key.NumPad9) && e.Key != Key.Enter && e.Key != Key.Escape && e.Key != Key.Back)
                e.Handled = true;
        }
    }
}
