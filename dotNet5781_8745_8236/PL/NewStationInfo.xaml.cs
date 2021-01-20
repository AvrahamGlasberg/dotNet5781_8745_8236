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
using Microsoft.Maps.MapControl.WPF;
using BLAPI;
using BO;
using System.Device.Location;

namespace PL
{
    /// <summary>
    /// Interaction logic for NewStationInfo.xaml
    /// </summary>
    public partial class NewStationInfo : Window
    {
        IBL bl;
        public NewStationInfo()
        {
            InitializeComponent();
            try
            {
                bl = BLFactory.GetBL();
            }
            catch(BO.MissingData ex)
            {
                MessageBox.Show(ex.Message);
            }
            CodeTB.Focus();
        }

        private void CodeTBKey_Down(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                NameTB.Focus();
        }

        private void Numbers_Enter_Only(object sender, KeyEventArgs e)
        {
            if (((int)e.Key < (int)Key.D0 || (int)e.Key > (int)Key.D9) && ((int)e.Key < (int)Key.NumPad0 || (int)e.Key > (int)Key.NumPad9) && e.Key != Key.Enter && e.Key != Key.Escape && e.Key != Key.Back)
                e.Handled = true;
        }
        private void Map_Double_click(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            Point p = e.GetPosition(MyMap);
            Location l = MyMap.ViewportPointToLocation(p);
            Pin.Location = l;
        }

        private void Add_Station(object sender, RoutedEventArgs e)
        {
            int code;
            string message = "";
            if (!int.TryParse(CodeTB.Text, out code) || NameTB.Text == "")
            {
                if(!int.TryParse(CodeTB.Text, out code))
                    message = "Please enter station's code!\n";
                if (NameTB.Text == "")
                    message += "Please enter station's name!";
                MessageBox.Show(message, "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    bl.AddBusStation(
                        new BO.BusStation()
                        {
                            Code = code,
                            Name = NameTB.Text,
                            Position = new GeoCoordinate(Pin.Location.Latitude, Pin.Location.Longitude),
                            LinesInstation = Enumerable.Empty<BO.Line>()
                        }
                        );
                    this.Close();
                }
                catch (BO.StationExists ex)
                {
                    MessageBox.Show(ex.Message + string.Format(" Please choose different code than {0}", ex.Code), "Object already exists", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
        }
    }
}
