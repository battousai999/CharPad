using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharPad.Framework
{
    public static class WeaponDefinitions
    {
        private static List<Weapon> weapons;

        public static List<Weapon> Weapons
        {
            get { return weapons; }
        }

        static WeaponDefinitions()
        {
            weapons = new List<Weapon>();

            // PH_1
            // Simple Melee
            weapons.Add(new Weapon("Club", 2, Dice.Get(6), "", WeaponGroup.Mace, WeaponProperties.None, WeaponCategory.SimpleMelee, 1));
            weapons.Add(new Weapon("Dagger", 3, Dice.Get(4), "5/10", WeaponGroup.LightBlade, WeaponProperties.OffHand | WeaponProperties.LightThrown, WeaponCategory.SimpleMelee, 1));
            weapons.Add(new Weapon("Javalin", 2, Dice.Get(6), "10/20", WeaponGroup.Spear, WeaponProperties.HeavyThrown, WeaponCategory.SimpleMelee, 5));
            weapons.Add(new Weapon("Mace", 2, Dice.Get(8), "", WeaponGroup.Mace, WeaponProperties.Versatile, WeaponCategory.SimpleMelee, 5));
            weapons.Add(new Weapon("Sickle", 2, Dice.Get(6), "", WeaponGroup.LightBlade, WeaponProperties.OffHand, WeaponCategory.SimpleMelee, 2));
            weapons.Add(new Weapon("Spear", 2, Dice.Get(8), "", WeaponGroup.Spear, WeaponProperties.Versatile, WeaponCategory.SimpleMelee, 5));
            weapons.Add(new Weapon("Greatclub", 2, Dice.Get(2, 4), "", WeaponGroup.Mace, WeaponProperties.None, WeaponCategory.SimpleMelee, 1, true));
            weapons.Add(new Weapon("Morningstar", 2, Dice.Get(10), "", WeaponGroup.Mace, WeaponProperties.None, WeaponCategory.SimpleMelee, 10, true));
            weapons.Add(new Weapon("Quarterstaff", 2, Dice.Get(8), "", WeaponGroup.Staff, WeaponProperties.None, WeaponCategory.SimpleMelee, 5, true));
            weapons.Add(new Weapon("Scythe", 2, Dice.Get(2, 4), "", WeaponGroup.HeavyBlade, WeaponProperties.None, WeaponCategory.SimpleMelee, 5, true));

            // Military Melee
            weapons.Add(new Weapon("Battleaxe", 2, Dice.Get(10), "", WeaponGroup.Axe, WeaponProperties.Versatile, WeaponCategory.MilitaryMelee, 15));
            weapons.Add(new Weapon("Flail", 2, Dice.Get(10), "", WeaponGroup.Flail, WeaponProperties.Versatile, WeaponCategory.MilitaryMelee, 10));
            weapons.Add(new Weapon("Handaxe", 2, Dice.Get(6), "5/10", WeaponGroup.Axe, WeaponProperties.OffHand | WeaponProperties.HeavyThrown, WeaponCategory.MilitaryMelee, 5));
            weapons.Add(new Weapon("Longsword", 3, Dice.Get(8), "", WeaponGroup.HeavyBlade, WeaponProperties.Versatile, WeaponCategory.MilitaryMelee, 15));
            weapons.Add(new Weapon("Scimitar", 2, Dice.Get(8), "", WeaponGroup.HeavyBlade, WeaponProperties.HighCrit, WeaponCategory.MilitaryMelee, 10));
            weapons.Add(new Weapon("Short sword", 3, Dice.Get(6), "", WeaponGroup.LightBlade, WeaponProperties.OffHand, WeaponCategory.MilitaryMelee, 10));
            weapons.Add(new Weapon("Throwing hammer", 2, Dice.Get(6), "5/10", WeaponGroup.Hammer, WeaponProperties.OffHand | WeaponProperties.HeavyThrown, WeaponCategory.MilitaryMelee, 5));
            weapons.Add(new Weapon("Warhammer", 2, Dice.Get(10), "", WeaponGroup.Hammer, WeaponProperties.Versatile, WeaponCategory.MilitaryMelee, 15));
            weapons.Add(new Weapon("War pick", 2, Dice.Get(8), "", WeaponGroup.Pick, WeaponProperties.HighCrit | WeaponProperties.Versatile, WeaponCategory.MilitaryMelee, 15));
            weapons.Add(new Weapon("Falchion", 3, Dice.Get(2, 4), "", WeaponGroup.HeavyBlade, WeaponProperties.HighCrit, WeaponCategory.MilitaryMelee, 25, true));
            weapons.Add(new Weapon("Glaive", 3, Dice.Get(2, 4), "", WeaponGroup.HeavyBlade | WeaponGroup.Polearm, WeaponProperties.Reach, WeaponCategory.MilitaryMelee, 25, true));
            weapons.Add(new Weapon("Greataxe", 2, Dice.Get(12), "", WeaponGroup.Axe, WeaponProperties.HighCrit, WeaponCategory.MilitaryMelee, 30, true));
            weapons.Add(new Weapon("Greatsword", 3, Dice.Get(10), "", WeaponGroup.HeavyBlade, WeaponProperties.None, WeaponCategory.MilitaryMelee, 30, true));
            weapons.Add(new Weapon("Halberd", 2, Dice.Get(10), "", WeaponGroup.Axe | WeaponGroup.Polearm, WeaponProperties.Reach, WeaponCategory.MilitaryMelee, 25, true));
            weapons.Add(new Weapon("Heavy flail", 2, Dice.Get(2, 6), "", WeaponGroup.Flail, WeaponProperties.None, WeaponCategory.MilitaryMelee, 25, true));
            weapons.Add(new Weapon("Longspear", 2, Dice.Get(10), "", WeaponGroup.Polearm | WeaponGroup.Spear, WeaponProperties.Reach, WeaponCategory.MilitaryMelee, 10, true));
            weapons.Add(new Weapon("Maul", 2, Dice.Get(2, 6), "", WeaponGroup.Hammer, WeaponProperties.None, WeaponCategory.MilitaryMelee, 30, true));

            // Superior Melee
            weapons.Add(new Weapon("Bastard sword", 3, Dice.Get(10), "", WeaponGroup.HeavyBlade, WeaponProperties.Versatile, WeaponCategory.SuperiorMelee, 30));
            weapons.Add(new Weapon("Katar", 3, Dice.Get(6), "", WeaponGroup.LightBlade, WeaponProperties.OffHand | WeaponProperties.HighCrit, WeaponCategory.SuperiorMelee, 3));
            weapons.Add(new Weapon("Rapier", 3, Dice.Get(8), "", WeaponGroup.LightBlade, WeaponProperties.None, WeaponCategory.SuperiorMelee, 25));
            weapons.Add(new Weapon("Spiked chain", 3, Dice.Get(2, 4), "", WeaponGroup.Flail, WeaponProperties.Reach, WeaponCategory.SuperiorMelee, 30, true));

            // Simple Ranged
            weapons.Add(new Weapon("Hand crossbow", 2, Dice.Get(6), "10/20", WeaponGroup.Crossbow, WeaponProperties.LoadFree, WeaponCategory.SimpleRanged, 25));
            weapons.Add(new Weapon("Sling", 2, Dice.Get(6), "10/20", WeaponGroup.Sling, WeaponProperties.LoadFree, WeaponCategory.SimpleRanged, 1));
            weapons.Add(new Weapon("Crossbow", 2, Dice.Get(8), "15/30", WeaponGroup.Crossbow, WeaponProperties.LoadMinor, WeaponCategory.SimpleRanged, 25, true));

            // Military Ranged
            weapons.Add(new Weapon("Longbow", 2, Dice.Get(10), "20/40", WeaponGroup.Bow, WeaponProperties.LoadFree, WeaponCategory.MilitaryRanged, 30, true));
            weapons.Add(new Weapon("Shortbow", 2, Dice.Get(8), "15/30", WeaponGroup.Bow, WeaponProperties.LoadFree | WeaponProperties.Small, WeaponCategory.MilitaryRanged, 25, true));

            // Superior Ranged
            weapons.Add(new Weapon("Shuriken", 3, Dice.Get(4), "6/12", WeaponGroup.LightBlade, WeaponProperties.LightThrown, WeaponCategory.SuperiorRanged, 1));

            // AV_1
            // Simple Melee
            weapons.Add(new Weapon("Spiked gauntlet", 2, Dice.Get(6), "", WeaponGroup.Unarmed, WeaponProperties.OffHand, WeaponCategory.SimpleMelee, 5));

            // Military Melee
            weapons.Add(new Weapon("Broadsword", 2, Dice.Get(10), "", WeaponGroup.HeavyBlade, WeaponProperties.Versatile, WeaponCategory.MilitaryMelee, 20));
            weapons.Add(new Weapon("Khopesh", 2, Dice.Get(8), "", WeaponGroup.Axe | WeaponGroup.HeavyBlade, WeaponProperties.Brutal_1 | WeaponProperties.Versatile, WeaponCategory.MilitaryMelee, 20));
            weapons.Add(new Weapon("Light war pick", 2, Dice.Get(6), "", WeaponGroup.Pick, WeaponProperties.HighCrit | WeaponProperties.OffHand, WeaponCategory.MilitaryMelee, 10));
            weapons.Add(new Weapon("Scourge", 2, Dice.Get(8), "", WeaponGroup.Flail, WeaponProperties.OffHand, WeaponCategory.MilitaryMelee, 3));
            weapons.Add(new Weapon("Trident", 2, Dice.Get(8), "3/6", WeaponGroup.Spear, WeaponProperties.HeavyThrown | WeaponProperties.Versatile, WeaponCategory.MilitaryMelee, 10));
            weapons.Add(new Weapon("Heavy war pick", 2, Dice.Get(12), "", WeaponGroup.Pick, WeaponProperties.HighCrit, WeaponCategory.MilitaryMelee, 20, true));

            // Superior Melee
            weapons.Add(new Weapon("Craghammer", 2, Dice.Get(10), "", WeaponGroup.Hammer, WeaponProperties.Brutal_2 | WeaponProperties.Versatile, WeaponCategory.SuperiorMelee, 20));
            weapons.Add(new Weapon("Kukri", 2, Dice.Get(6), "", WeaponGroup.LightBlade, WeaponProperties.Brutal_1 | WeaponProperties.OffHand, WeaponCategory.SuperiorMelee, 10));
            weapons.Add(new Weapon("Parrying dagger", 2, Dice.Get(4), "", WeaponGroup.LightBlade, WeaponProperties.Defensive | WeaponProperties.OffHand, WeaponCategory.SuperiorMelee, 5));
            weapons.Add(new Weapon("Spiked shield", 2, Dice.Get(6), "", WeaponGroup.LightBlade, WeaponProperties.OffHand, WeaponCategory.SuperiorMelee, 10));
            weapons.Add(new Weapon("Tratnyr", 2, Dice.Get(8), "10/20", WeaponGroup.Spear, WeaponProperties.HeavyThrown | WeaponProperties.Versatile, WeaponCategory.SuperiorMelee, 10));
            weapons.Add(new Weapon("Triple-headed flail", 3, Dice.Get(10), "", WeaponGroup.Flail, WeaponProperties.Versatile, WeaponCategory.SuperiorMelee, 15));
            weapons.Add(new Weapon("Waraxe", 2, Dice.Get(12), "", WeaponGroup.Axe, WeaponProperties.Versatile, WeaponCategory.SuperiorMelee, 30));
            weapons.Add(new Weapon("Execution axe", 2, Dice.Get(12), "", WeaponGroup.Axe, WeaponProperties.Brutal_2 | WeaponProperties.HighCrit, WeaponCategory.SuperiorMelee, 30, true));
            weapons.Add(new Weapon("Fullblade", 3, Dice.Get(10), "", WeaponGroup.HeavyBlade, WeaponProperties.HighCrit, WeaponCategory.SuperiorMelee, 30, true));
            weapons.Add(new Weapon("Greatspear", 3, Dice.Get(10), "", WeaponGroup.Polearm | WeaponGroup.Spear, WeaponProperties.Reach, WeaponCategory.SuperiorMelee, 25, true));
            weapons.Add(new Weapon("Mordenkrad", 2, Dice.Get(2, 6), "", WeaponGroup.Hammer, WeaponProperties.Brutal_1, WeaponCategory.SuperiorMelee, 30, true));
            weapons.Add(new Weapon("Double axe", 2, Dice.Get(10), "", WeaponGroup.Axe, WeaponProperties.Defensive | WeaponProperties.OffHand, WeaponCategory.SuperiorMelee, 40));
            weapons.Add(new Weapon("Double flail", 2, Dice.Get(10), "", WeaponGroup.Flail, WeaponProperties.Defensive | WeaponProperties.OffHand, WeaponCategory.SuperiorMelee, 30));
            weapons.Add(new Weapon("Double sword", 3, Dice.Get(8), "", WeaponGroup.HeavyBlade, WeaponProperties.Defensive | WeaponProperties.OffHand, WeaponCategory.SuperiorMelee, 40));

            // Simple Ranged
            weapons.Add(new Weapon("Repeating crossbow", 2, Dice.Get(8), "10/20", WeaponGroup.Crossbow, WeaponProperties.LoadFree, WeaponCategory.SimpleRanged, 35, true));
            
            // Superior Ranged
            weapons.Add(new Weapon("Greatbow", 2, Dice.Get(12), "25/50", WeaponGroup.Bow, WeaponProperties.LoadFree, WeaponCategory.SuperiorRanged, 30, true));
            weapons.Add(new Weapon("Superior crossbow", 3, Dice.Get(10), "20/40", WeaponGroup.Crossbow, WeaponProperties.LoadMinor, WeaponCategory.SuperiorRanged, 30, true));
        }
    }
}
