﻿using System;
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
                case WeaponType.SpikedGauntlet:
                    return "Spiked Gauntlet";
                case WeaponType.LightWarPick:
                    return "Light War Pick";
                case WeaponType.HeavyWarPick:
                    return "Heavy War Pick";
                case WeaponType.ParryingDagger:
                    return "Parrying Dagger";
                case WeaponType.SpikedShield:
                    return "Spiked Shield";
                case WeaponType.TripleHeadedFlail:
                    return "Triple-headed Flail";
                case WeaponType.ExecutionAxe:
                    return "Execution Axe";
                case WeaponType.DoubleAxe:
                    return "Double Axe";
                case WeaponType.DoubleFlail:
                    return "Double Flail";
                case WeaponType.DoubleSword:
                    return "Double Sword";
                case WeaponType.RepeatingCrossbow:
                    return "Repeating Crossbow";
                case WeaponType.SuperiorCrossbow:
                    return "Superior Crossbow";
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
                case WeaponCategory.PureImplement:
                    return "Implement";
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

        public static string GetWeaponGroupName(WeaponGroup group)
        {
            switch (group)
            {
                case WeaponGroup.HeavyBlade:
                    return "Heavy Blade";
                case WeaponGroup.LightBlade:
                    return "Light Blade";
                default:
                    return Enum.Format(typeof(WeaponGroup), group, "G");
            }
        }

        public static string GetWeaponPropertyName(WeaponProperties property)
        {
            switch (property)
            {
                case WeaponProperties.Brutal_1:
                    return "Brutal 1";
                case WeaponProperties.Brutal_2:
                    return "Brutal 2";
                case WeaponProperties.HeavyThrown:
                    return "Heavy thrown";
                case WeaponProperties.HighCrit:
                    return "High crit";
                case WeaponProperties.LightThrown:
                    return "Light thrown";
                case WeaponProperties.LoadFree:
                    return "Load free";
                case WeaponProperties.LoadMinor:
                    return "Load minor";
                case WeaponProperties.OffHand:
                    return "Off-hand";
                default:
                    return Enum.Format(typeof(WeaponProperties), property, "G");
            }
        }

        public static string GetPowerActionTypeName(PowerActionType actionType)
        {
            switch (actionType)
            {
                case PowerActionType.ImmediateInterrupt:
                    return "Immediate Interrupt";
                case PowerActionType.ImmediateReaction:
                    return "Immediate Reaction";
                default:
                    return Enum.Format(typeof(PowerActionType), actionType, "G");
            }
        }
    }
}
