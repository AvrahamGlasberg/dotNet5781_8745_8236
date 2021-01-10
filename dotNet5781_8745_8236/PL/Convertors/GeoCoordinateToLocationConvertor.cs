using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;
using System.Windows.Data;
using System.Globalization;
using Microsoft.Maps.MapControl.WPF;
namespace PL
{
    public class GeoCoordinateToLocationConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GeoCoordinate coordinate = value as GeoCoordinate;
            return new Location(coordinate.Latitude, coordinate.Longitude);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Location location = value as Location;
            return new GeoCoordinate(location.Latitude, location.Longitude);
        }
    }
}
