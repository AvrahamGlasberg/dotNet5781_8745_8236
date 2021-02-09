using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
namespace PL
{
    /// <summary>
    /// Class to convert IsChecked To Visibility, 
    /// if IsChecked than Visibility = Visibility.Visible, else Visibility.Collapsed
    /// </summary>
    public class IsCheckedToVisibilityConvertor : IValueConverter
    {
        /// <summary>
        /// Converts IsChecked To Visibility.
        /// if IsChecked than Visibility = Visibility.Visible, else Visibility.Collapsed
        /// </summary>
        /// <param name="value">IsChecked to convert</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Parameters</param>
        /// <param name="culture">Culture</param>
        /// <returns>The converted Visibility</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? IsChecked = value as bool?;
            if (IsChecked == true)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }
        /// <summary>
        /// Converts back Visibility to IsChecked
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Parameters</param>
        /// <param name="culture">Culture</param>
        /// <returns>The converted Ischecked</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (((Visibility)value) == Visibility.Visible)
                return true;
            return false;
        }
    }
}
