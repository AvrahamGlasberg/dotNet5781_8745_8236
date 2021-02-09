using System;
using System.Windows;
using System.Windows.Media;
using BLAPI;
namespace PL
{
    /// <summary>
    /// Interaction logic for BusInfo.xaml
    /// </summary>
    public partial class BusInfo : Window
    {
        /// <summary>
        /// object that implement IBL
        /// </summary>
        IBL bl;
        /// <summary>
        /// the Bus object that send to the window
        /// </summary>
        BO.Bus Curbus;
        /// <summary>
        /// Media Player object for Animation
        /// </summary>
        MediaPlayer sound = new MediaPlayer();
        /// <summary>
        /// window ctor
        /// </summary>
        /// <param name="bus"></param>
        public BusInfo(BO.Bus bus)
        {
            InitializeComponent();
            try
            {
                bl = BLFactory.GetBL();
            }
            catch (BO.MissingData ex) //creating BO failed
            {
                MessageBox.Show(ex.Message);
            }
            Curbus = bus;
            MainGrid.DataContext = Curbus;
        }
        /// <summary>
        /// refueling the bus by operate the BL's Refuel func and make sound
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Start_Refuel(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.Refuel(Curbus);
                sound.Open(new Uri("../../PL/Sounds/refuel.mp3", UriKind.Relative));
                sound.Volume = 1;
                sound.Play();
                this.Close();
            }
            catch(BO.BusNotFound ex) // can't found the bus for refueling
            {
                MessageBox.Show(ex.Message + string.Format(" Bus {0} could not be found.", ex.License), "Object Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// treatmenting the bus by operate the BL's Treatment func and make sound
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="e">e of the argument</param>
        private void Start_Treatment(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.Treatment(Curbus);
                sound.Open(new Uri("../../PL/Sounds/treatment.mp3", UriKind.Relative));
                sound.Volume = 1;
                sound.Play();
                this.Close();
            }
            catch (BO.BusNotFound ex) // can't found the bus for refueling
            {
                MessageBox.Show(ex.Message + string.Format(" Bus {0} could not be found.", ex.License), "Object Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
