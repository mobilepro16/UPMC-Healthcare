using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Xamarin.Forms;

namespace UPMC_App.Converters
{
    public class PinLenghtToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                int pinLenght = (int)value;
                int numOfPoint = int.Parse(parameter.ToString());

                if (pinLenght >= numOfPoint)
                    return Color.Green;
                else
                    return Color.Transparent;
            }
            else
            {
                return Color.Transparent;
            }

        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 0;
        }
    }
}
