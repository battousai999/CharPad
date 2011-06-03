using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using CharPad.Framework;

namespace CharPad.ValueConverters
{
    public class PowerAttackTypeConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Power power = (value as Power);

            if (power == null)
                return "";

            if (power.AttackType == PowerAttackType.Weapon)
            {
                if (power.AttackWeapon == WeaponSlot.OffhandWeapon)
                    return "Weapon (off-hand)";
                else if (power.AttackWeapon == WeaponSlot.RangedWeapon)
                    return "Ranged Weapon";
                else
                    return "Weapon";
            }
            else if (power.AttackType == PowerAttackType.Implement)
            {
                if (power.AttackWeapon == WeaponSlot.MainWeapon)
                    return "Implement (main weapon)";
                else if (power.AttackWeapon == WeaponSlot.OffhandWeapon)
                    return "Implement (off-hand)";
                else if (power.AttackWeapon == WeaponSlot.RangedWeapon)
                    return "Implement (ranged weapon)";
                else
                    return "Implement";
            }
            else
                return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
