using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharPad.Framework
{
    public static class Utility
    {
        public static string GetWeaponTypeName(WeaponType weaponType)
        {
            switch (weaponType)
            {
                case WeaponType.ThrowingHammer:
                    return "Throwing Hammer";
                case WeaponType.WarPick:
                    return "War Pick";
                case WeaponType.HeavyFlail:
                    return "Heavy Flail";
                case WeaponType.BastardSword:
                    return "Bastard Sword";
                case WeaponType.SpikedChain:
                    return "Spiked Chain";
                case WeaponType.HandCrossbow:
                    return "Hand Crossbow";
                default:
                    return Enum.Format(typeof(WeaponType), weaponType, "G");
            }
        }

        public static string GetWeaponCategoryName(WeaponCategory weaponCategory)
        {
            switch (weaponCategory)
            {
                case WeaponCategory.SimpleMelee:
                    return "Simple Melee";
                case WeaponCategory.SimpleRanged:
                    return "Simple Ranged";
                case WeaponCategory.MilitaryMelee:
                    return "Military Melee";
                case WeaponCategory.MilitaryRanged:
                    return "Military Ranged";
                case WeaponCategory.SuperiorMelee:
                    return "Superior Melee";
                case WeaponCategory.SuperiorRanged:
                    return "Superior Ranged";
                default:
                    return Enum.Format(typeof(WeaponCategory), weaponCategory, "G");
            }
        }

        public static string GetArmorTypeName(ArmorType armorType)
        {
            switch (armorType)
            {
                case ArmorType.LightShield:
                    return "Light Shield";
                case ArmorType.HeavyShield:
                    return "Heavy Shield";
                default:
                    return Enum.Format(typeof(ArmorType), armorType, "G");
            }
        }

        public static string GetSkillName(Skill skill)
        {
            return Enum.Format(typeof(Skill), skill, "G");
        }
    }
}
