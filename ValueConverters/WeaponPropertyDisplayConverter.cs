using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using CharPad.Framework;

namespace CharPad.ValueConverters
{
    public class WeaponPropertyDisplayConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is WeaponProperties))
                return "";

            WeaponProperties prop = (WeaponProperties)value;
            List<string> results = new List<string>();

            foreach (WeaponProperties iProp in Enum.GetValues(typeof(WeaponProperties)))
            {
                if (iProp == WeaponProperties.None)
                    continue;

                if ((prop & iProp) == iProp)
                    results.Add(Utility.GetWeaponPropertyName(iProp));
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
