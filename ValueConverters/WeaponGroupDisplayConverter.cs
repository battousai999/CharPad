using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using CharPad.Framework;

namespace CharPad.ValueConverters
{
    public class WeaponGroupDisplayConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is WeaponGroup))
                return "";

            WeaponGroup group = (WeaponGroup)value;
            List<string> results = new List<string>();

            foreach (WeaponGroup iGroup in Enum.GetValues(typeof(WeaponGroup)))
            {
                if ((group & iGroup) == iGroup)
                    results.Add(Utility.GetWeaponGroupName(iGroup));
            }

            return String.Join(", ", results.ToArray());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
