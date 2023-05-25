using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace UPMC_App.Converters
{
    public class BoolToNegateBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return !((bool)value);
            }
            else
            {
                return false;
            }

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return !((bool)value);
            }
            else
            {
                return false;
            }
        }
    }
}
