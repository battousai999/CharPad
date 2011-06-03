using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using CharPad.Framework;
using System.Windows.Media;

namespace CharPad.ValueConverters
{
    public class PowerTypeColorConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is PowerType))
                return Brushes.Transparent;

            PowerType powerType = (PowerType)value;

            if (powerType == PowerType.AtWill)
                return Brushes.LimeGreen;
            else if (powerType == PowerType.Encounter)
                return Brushes.Red;
            else if (powerType == PowerType.Daily)
                return Brushes.Black;
            else
                return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
