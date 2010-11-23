using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace CharPad.ValueConverters
{
    public class ZeroToBoolConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool matchValue = true;
            bool tempValue;

            if (Boolean.TryParse(System.Convert.ToString(parameter), out tempValue))
                matchValue = tempValue;

            if (!(value is int))
                return !matchValue;

            return ((int)value == 0 ? matchValue : !matchValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
