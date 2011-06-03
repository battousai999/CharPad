using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using CharPad.Framework;

namespace CharPad.ValueConverters
{
    public class PowerActionTypeConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is PowerActionType))
                return "";

            return Utility.GetPowerActionTypeName((PowerActionType)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
