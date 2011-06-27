using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace CharPad.ValueConverters
{
    public class ParenthasizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string tempString;

            if (value is int)
            {
                int tempInt = (int)value;

                if (tempInt == 0)
                    return "";
                else
                    tempString = (tempInt > 0 ? "+" : "") + System.Convert.ToString(value);
            }
            else
                tempString = System.Convert.ToString(value);

            if (String.IsNullOrWhiteSpace(tempString))
                return "";

            return "(" + tempString + ")";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
