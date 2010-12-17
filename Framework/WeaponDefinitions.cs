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
            weapons.Add(new Weapon("Club", 2, Dice.Get(6), "", WeaponGroup.Mace, WeaponProperties.None));
            weapons.Add(new Weapon("Dagger", 3, Dice.Get(4), "5/10", WeaponGroup.LightBlade, WeaponProperties.OffHand | WeaponProperties.LightThrown));
            weapons.Add(new Weapon("Javalin", 2, Dice.Get(6), "10/20", WeaponGroup.Spear, WeaponProperties.HeavyThrown));
            weapons.Add(new Weapon("Mace", 2, Dice.Get(8), "", WeaponGroup.Mace, WeaponProperties.Versatile));
            weapons.Add(new Weapon("Sickle", 2, Dice.Get(6), "", WeaponGroup.LightBlade, WeaponProperties.OffHand));
            weapons.Add(new Weapon("Spear", 2, Dice.Get(8), "", WeaponGroup.Spear, WeaponProperties.Versatile));
            weapons.Add(new Weapon("Greatclub", 2, Dice.Get(2, 4), "", WeaponGroup.Mace, WeaponProperties.None));
            weapons.Add(new Weapon("Morningstar", 2, Dice.Get(10), "", WeaponGroup.Mace, WeaponProperties.None));
            weapons.Add(new Weapon("Quarterstaff", 2, Dice.Get(8), "", WeaponGroup.Staff, WeaponProperties.None));
            weapons.Add(new Weapon("Scythe", 2, Dice.Get(2, 4), "", WeaponGroup.HeavyBlade, WeaponProperties.None));

            // Military Melee
            weapons.Add(new Weapon("Battleaxe", 2, Dice.Get(10), "", WeaponGroup.Axe, WeaponProperties.Versatile));
            weapons.Add(new Weapon("Flail", 2, Dice.Get(10), "", WeaponGroup.Flail, WeaponProperties.Versatile));
            weapons.Add(new Weapon("Handaxe", 2, Dice.Get(6), "5/10", WeaponGroup.Axe, WeaponProperties.OffHand | WeaponProperties.HeavyThrown));
            weapons.Add(new Weapon("Longsword", 3, Dice.Get(8), "", WeaponGroup.HeavyBlade, WeaponProperties.Versatile));
            weapons.Add(new Weapon("Scimitar", 2, Dice.Get(8), "", WeaponGroup.HeavyBlade, WeaponProperties.HighCrit));
            weapons.Add(new Weapon("Short sword", 3, Dice.Get(6), "", WeaponGroup.LightBlade, WeaponProperties.OffHand));
            weapons.Add(new Weapon("Throwing hammer", 2, Dice.Get(6), "5/10", WeaponGroup.Hammer, WeaponProperties.OffHand | WeaponProperties.HeavyThrown));
            weapons.Add(new Weapon("Warhammer", 2, Dice.Get(10), "", WeaponGroup.Hammer, WeaponProperties.Versatile));
            weapons.Add(new Weapon("War pick", 2, Dice.Get(8), "", WeaponGroup.Pick, WeaponProperties.HighCrit | WeaponProperties.Versatile));
            weapons.Add(new Weapon("Falchion", 3, Dice.Get(2, 4), "", WeaponGroup.HeavyBlade, WeaponProperties.HighCrit));
            weapons.Add(new Weapon("Glaive", 3, Dice.Get(2, 4), "", WeaponGroup.HeavyBlade | WeaponGroup.Polearm, WeaponProperties.Reach));
            weapons.Add(new Weapon("Greataxe", 2, Dice.Get(12), "", WeaponGroup.Axe, WeaponProperties.HighCrit));
            weapons.Add(new Weapon("Greatsword", 3, Dice.Get(10), "", WeaponGroup.HeavyBlade, WeaponProperties.None));
            weapons.Add(new Weapon("Halberd", 2, Dice.Get(10), "", WeaponGroup.Axe | WeaponGroup.Polearm, WeaponProperties.Reach));
            weapons.Add(new Weapon("Heavy flail", 2, Dice.Get(2, 6), "", WeaponGroup.Flail, WeaponProperties.None));
            weapons.Add(new Weapon("Longspear", 2, Dice.Get(10), "", WeaponGroup.Polearm | WeaponGroup.Spear, WeaponProperties.Reach));
            weapons.Add(new Weapon("Maul", 2, Dice.Get(2, 6), "", WeaponGroup.Hammer, WeaponProperties.None));

            // Superior Melee
            weapons.Add(new Weapon("
        }
    }
}
