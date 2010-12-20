using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class Weapon : IInventoryItem, INotifyPropertyChanged
    {
        private string name;
        private int proficiencyBonus;
        private int enhancementBonus;
        private Dice damage;
        private string range;
        private WeaponGroup group;
        private WeaponProperties properties;
        private int basePrice;
        private WeaponCategory category;
        private bool isTwoHanded;

        public Weapon(string name, int proficiencyBonus, Dice damage, string range, WeaponGroup group, WeaponProperties properties, WeaponCategory category)
            : this(name, proficiencyBonus, damage, range, group, properties, 0, category, false, 0)
        {
        }

        public Weapon(string name, int proficiencyBonus, Dice damage, string range, WeaponGroup group, WeaponProperties properties, WeaponCategory category, int basePrice)
            : this(name, proficiencyBonus, damage, range, group, properties, basePrice, category, false, 0)
        {
        }

        public Weapon(string name, int proficiencyBonus, Dice damage, string range, WeaponGroup group, WeaponProperties properties, WeaponCategory category, int basePrice, bool isTwoHanded)
            : this(name, proficiencyBonus, damage, range, group, properties, basePrice, category, isTwoHanded, 0)
        {
        }

        public Weapon(string name, int proficiencyBonus, Dice damage, string range, WeaponGroup group, WeaponProperties properties, int basePrice, WeaponCategory category, bool isTwoHanded, int enhancementBonus)
        {
            this.name = name;
            this.proficiencyBonus = proficiencyBonus;
            this.damage = damage;
            this.range = range;
            this.group = group;
            this.properties = properties;
            this.enhancementBonus = enhancementBonus;
            this.basePrice = basePrice;
            this.category = category;
            this.isTwoHanded = isTwoHanded;
        }

        public string Name { get { return name; } set { name = value; Notify("Name"); } }
        public int ProficiencyBonus { get { return proficiencyBonus; } set { proficiencyBonus = value; Notify("ProficiencyBonus"); Notify("TotalBonus"); } }
        public int EnhancementBonus { get { return enhancementBonus; } set { enhancementBonus = value; Notify("EnhancementBonus"); Notify("TotalBonus"); } }
        public Dice Damage { get { return damage; } set { damage = value; Notify("Damage"); } }
        public string Range { get { return range; } set { range = value; Notify("Range"); } }
        public WeaponGroup Group { get { return group; } set { group = value; Notify("Group"); } }
        public WeaponProperties Properties { get { return properties; } set { properties = value; Notify("Properties"); } }
        public int BasePrice { get { return basePrice; } set { basePrice = value; Notify("BasePrice"); } }
        public WeaponCategory Category { get { return category; } set { category = value; Notify("Category"); } }
        public bool IsTwoHanded { get { return isTwoHanded; } set { isTwoHanded = value; Notify("IsTwoHanded"); } }

        public int TotalBonus { get { return proficiencyBonus + enhancementBonus; } }

        public bool IsRanged { get { return ((category == WeaponCategory.SimpleRanged) || (category == WeaponCategory.MilitaryRanged) || (category == WeaponCategory.SuperiorRanged)); } }
        public bool IsThrown { get { return (HasWeaponProperty(WeaponProperties.LightThrown) || HasWeaponProperty(WeaponProperties.HeavyThrown)); } }

        public bool HasWeaponProperty(WeaponProperties property)
        {
            return ((Properties & property) == property);
        }

        #region INotifyPropertyChanged Members

        private void Notify(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
