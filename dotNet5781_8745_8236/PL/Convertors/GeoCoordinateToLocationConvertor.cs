using System;
using System.Device.Location;
using System.Windows.Data;
using System.Globalization;
using Microsoft.Maps.MapControl.WPF;
namespace PL
{
    /// <summary>
    /// Class to convert GeoCoordinate into Location
    /// </summary>
    public class GeoCoordinateToLocationConvertor : IValueConverter
    {
        /// <summary>
        /// Converts GeoCoordinate into Location
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Parameters</param>
        /// <param name="culture">Culture</param>
        /// <returns>The converted Location</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            GeoCoordinate coordinate = value as GeoCoordinate;
            return new Location(coordinate.Latitude, coordinate.Longitude);
        }
        /// <summary>
        /// Converts back Location into GeoCoordinate
        /// </summary>
        /// <param name="value">The location to convert back</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Parameters</param>
        /// <param name="culture">Culture</param>
        /// <returns>The converted GeoCoordinate</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Location location = value as Location;
            return new GeoCoordinate(location.Latitude, location.Longitude);
        }
    }
}
