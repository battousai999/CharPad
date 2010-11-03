using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace CharPad.ValueConverters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool flag = false;

            if (value is bool)
                flag = (bool)value;
            else if (value is bool?)
                flag = ((bool?)value).GetValueOrDefault();

            if (parameter != null)
            {
                if (System.Convert.ToString(parameter) == "0")
                    flag = !flag;

                bool tempBool;

                if (Boolean.TryParse(System.Convert.ToString(parameter), out tempBool))
                {
                    if (!tempBool)
                        flag = !flag;
                }
            }

            return (flag ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
