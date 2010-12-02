using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using CharPad.Framework;

namespace CharPad.ValueConverters
{
    public class WeaponBonusConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((values == null) || (values.Length < 2) || (values[0] == null))
                return "(none)";

            Weapon weapon = (values[0] as Weapon);
            Player player = (values[1] as Player);

            if ((weapon == null) || (player == null))
                return "(none)";

            WeaponSpecValue spec;

            if (weapon == player.Weapon)
                spec = player.WeaponSpec;
            else if (weapon == player.WeaponOffhand)
                spec = player.WeaponOffhandSpec;
            else
                throw new InvalidOperationException("Weapon given does not match one of the weapons wielded by the player.");

            return String.Format("({0}) {1} to hit, {2} damage",
                weapon.Name,
                spec.ToHitSpec,
                spec.DamageSpec);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
