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
            else if (weapon == player.RangedWeapon)
                spec = player.RangedWeaponSpec;
            else if (weapon == player.Implement)
                spec = player.ImplementSpec;
            else
                throw new InvalidOperationException("Weapon given does not match one of the weapons wielded by the player.");

            if (weapon == player.Implement)
            {
                return String.Format("({0}) {1} to hit, {2} to damage",
                    weapon.Name,
                    spec.ToHitSpec,
                    spec.ImplementDamageSpec);
            }
            else
            {
                return String.Format("({0}) {1}{3} to hit, {2}{4} damage",
                    weapon.Name,
                    spec.ToHitSpec,
                    spec.DamageSpec,
                    (spec.Weapon.IsThrown && !spec.Weapon.IsRanged && (spec.TotalToHitBonus != spec.TotalThrownToHitBonus) ? String.Format(" ({0} thrown)", spec.ThrownToHitSpec) : ""),
                    (spec.Weapon.IsThrown && !spec.Weapon.IsRanged && (spec.TotalDamageBonus != spec.TotalThrownDamageBonus) ? String.Format(" ({0} thrown)", spec.ThrownDamageSpec) : ""));
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
