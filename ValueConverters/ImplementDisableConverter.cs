using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using CharPad.Framework;

namespace CharPad.ValueConverters
{
    public class ImplementDisableConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is EditWeaponWindow.CategoryItem))
                return true;

            EditWeaponWindow.CategoryItem item = (EditWeaponWindow.CategoryItem)value;

            return (item.Category != WeaponCategory.PureImplement);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
