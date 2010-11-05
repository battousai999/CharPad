using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using CharPad.Framework;

namespace CharPad.ValueConverters
{
    public class CreatureSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<EditCharacterRaceWindow.CreatureSizeItem> list = EditCharacterRaceWindow.CreatureSizeItem.GetSizeList();

            if (!(value is CreatureSize))
                return list.Find(x => x.Size == CreatureSize.Meduim);

            return list.Find(x => x.Size == (CreatureSize)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is EditCharacterRaceWindow.CreatureSizeItem))
                return CreatureSize.Meduim;

            return ((EditCharacterRaceWindow.CreatureSizeItem)value).Size;
        }
    }
}
