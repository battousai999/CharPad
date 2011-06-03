using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace CharPad.ValueConverters
{
    public class PowerDamageLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is string))
                return Visibility.Collapsed;

            string str = (string)value;

            if (!String.IsNullOrWhiteSpace(str) && (str.Length <= 2) && Char.IsDigit(str[0]) && Char.IsDigit(str[1]))
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
