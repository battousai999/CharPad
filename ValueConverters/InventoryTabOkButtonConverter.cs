using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace CharPad.ValueConverters
{
    public class InventoryTabOkButtonConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((values == null) || (values.Length == 0) || (values.Length % 2 != 0))
                return false;

            for (int i = 0; i < values.Length; i += 2)
            {
                bool isTabSelected = (bool)values[i];

                if (isTabSelected)
                    return (values[i + 1] != null);
            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
