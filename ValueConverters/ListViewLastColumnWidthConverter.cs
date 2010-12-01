using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Controls;

namespace CharPad.ValueConverters
{
    public class ListViewLastColumnWidthConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ListView listview = (value as ListView);

            if (listview == null)
                return null;

            GridView gridview = (listview.View as GridView);

            if (gridview == null)
                return null;

            double tempValue;
            double minValue = 100;
            if (Double.TryParse(System.Convert.ToString(parameter), out tempValue))
                minValue = tempValue;

            double otherColumnsWidth = 0;

            for (int i = 0; i < gridview.Columns.Count - 1; i++)
            {
                otherColumnsWidth += gridview.Columns[i].ActualWidth;
            }

            return Math.Max(listview.ActualWidth - otherColumnsWidth - 10, minValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
