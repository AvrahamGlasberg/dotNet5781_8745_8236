using System;
using System.Windows;
using System.Windows.Input;
using BO;
using BLAPI;
namespace PL
{
    /// <summary>
    /// Interaction logic for NewBusInfo.xaml
    /// </summary>
    public partial class NewBusInfo : Window
    {
        /// <summary>
        /// object that implement IBL
        /// </summary>
        IBL bl;
        /// <summary>
        /// Bus object of BO
        /// </summary>
        BO.Bus bus;
        /// <summary>
        /// window ctor
        /// </summary>
        public NewBusInfo()
        {
            InitializeComponent();
            try
            {
                bl = BLFactory.GetBL();
                bus = new BO.Bus()
                {
                    LastTreatmentDate = DateTime.Now,
                    TripSinceTreatment = 0,
                    FuelRemain = 400,
                    BusStatus = Status.Ready
                };
                date.DataContext = bus;
            }
            catch (BO.MissingData ex) //creating bo failed
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Add the bus to the data
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void AddBus(object sender, RoutedEventArgs e)
        {
            try
            {
                int lic;
                double trip;
                if (date.SelectedDate == null || !int.TryParse(licTB.Text, out lic) || !double.TryParse(kmTB.Text, out trip)) // when some data is missing / Invalid  jumping massage to the user and asking it
                {
                    string message = "";
                    if (!int.TryParse(licTB.Text, out lic))
                        message += "Please enter valid license!\n";
                    if (!double.TryParse(kmTB.Text, out trip))
                        message += "Please enter valid distance!\n";
                    if (date.SelectedDate == null)
                        message += "Please select a date!";
                    MessageBox.Show(message, "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else 
                {
                    if (lic.ToString().Length < 7) // check the input license
                        MessageBox.Show("License is too short!", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (lic.ToString().Length == 7 && bus.FromDate.Year >= 2018 || lic.ToString().Length == 8 && bus.FromDate.Year < 2018)
                        MessageBox.Show("License is not align with the bus's year!", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error); 
                    else
                    {
                        bus.LicenseNum = lic;
                        bus.TotalTrip = trip;
                        bl.AddBus(bus);
                        this.Close();
                    }
                }
            }
            catch(BO.BusExists ex) // the bus already exists
            {
                MessageBox.Show(ex.Message + string.Format("\nChoose different license than {0}", ex.License), "Invalid new object", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        /// <summary>
        /// allow the user to insert only numbers
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void TBPreview_key_Down(object sender, KeyEventArgs e)
        {
            if (((int)e.Key < (int)Key.D0 || (int)e.Key > (int)Key.D9) && ((int)e.Key < (int)Key.NumPad0 || (int)e.Key > (int)Key.NumPad9) && e.Key != Key.Enter && e.Key != Key.Escape && e.Key != Key.Back && e.Key != Key.OemPeriod && e.Key != Key.Oem1)
                e.Handled = true;
        }
        /// <summary>
        /// move to the Km textblock when press Enter key
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void TBFirstKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                kmTB.Focus();
        }
    }
}
