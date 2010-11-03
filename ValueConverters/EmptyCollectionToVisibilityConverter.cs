using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;

namespace CharPad.ValueConverters
{
    public class EmptyCollectionToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool reverse = (parameter != null);

            System.Collections.ICollection coll = (value as System.Collections.ICollection);

            if (coll == null)
                return Visibility.Collapsed;

            if (reverse)
                return (coll.Count == 0 ? Visibility.Collapsed : Visibility.Visible);
            else
                return (coll.Count == 0 ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
