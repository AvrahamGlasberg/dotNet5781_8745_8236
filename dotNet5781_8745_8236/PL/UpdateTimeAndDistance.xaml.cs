using System;
using System.Windows;
using System.Windows.Input;
using BLAPI;
namespace PL
{
    /// <summary>
    /// Interaction logic for UpdateTimeAndDistance.xaml
    /// </summary>
    public partial class UpdateTimeAndDistance : Window
    {
        /// <summary>
        /// object that implement IBL
        /// </summary>
        IBL bl;
        /// <summary>
        /// object of BO.LineStation
        /// </summary>
        BO.LineStation firstStation, secondStation;
        /// <summary>
        /// window ctor
        /// </summary>
        /// <param name="first">the first station</param>
        /// <param name="second">the second station</param>
        public UpdateTimeAndDistance(BO.LineStation first, BO.LineStation second)
        {
            InitializeComponent();
            try
            {
                bl = BLFactory.GetBL();
            }
            catch (BO.MissingData ex) // missing data
            {
                MessageBox.Show(ex.Message);
            }
            firstStation = first;
            secondStation = second;

            MainGrid.DataContext = first;
            SecondSt.DataContext = second;
        }
        /// <summary>
        /// allow the user to insert only numbers
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Numbers_Enter_Only(object sender, KeyEventArgs e)
        {
            if (((int)e.Key < (int)Key.D0 || (int)e.Key > (int)Key.D9) && ((int)e.Key < (int)Key.NumPad0 || (int)e.Key > (int)Key.NumPad9) && e.Key != Key.Enter && e.Key != Key.Escape && e.Key != Key.Back && e.Key != Key.OemPeriod && e.Key != Key.Oem1)
                e.Handled = true;
        }
        /// <summary>
        /// Update the info 
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void UpdateInfo(object sender, RoutedEventArgs e)
        {
            try
            {
                string message = "";
                if (!double.TryParse(DisTB.Text, out _) || !TimeSpan.TryParse(TimeTB.Text, out _))// when some data is missing/invalid jumping massage to the user and asking it
                {
                    if (!double.TryParse(DisTB.Text, out _))
                        message += "Please enter correct distance!\n";
                    if (!TimeSpan.TryParse(TimeTB.Text, out _))
                        message += "Please enter correct time!";
                    MessageBox.Show(message, "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    bl.UpdateTimeAndDis(firstStation, secondStation);
                    this.Close();
                }
            }
            catch (BO.MissingData ex) // missing data
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
