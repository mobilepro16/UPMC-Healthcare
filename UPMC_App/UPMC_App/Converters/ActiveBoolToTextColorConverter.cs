using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace UPMC_App.Converters
{
    public class ActiveBoolToTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if ((bool)value)
                    return (Color)App.Current.Resources["MainAccentColor"];
                else
                    return (Color)App.Current.Resources["HeaderTextColor"];
            }
            else
            {
                return (Color)App.Current.Resources["MainAccentColor"];
            }

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}
