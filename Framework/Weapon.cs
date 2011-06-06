using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;

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
        private Image picture;
        private string notes;
        private bool isImplement;

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

        public static Weapon CreateImplement(string name, int price)
        {
            Weapon weapon = new Weapon(name, 0, null, "", 0, WeaponProperties.None, WeaponCategory.PureImplement, price);
            weapon.IsImplement = true;

            return weapon;
        }

        public void CopyValues(Weapon weapon)
        {
            Name = weapon.Name;
            ProficiencyBonus = weapon.ProficiencyBonus;
            EnhancementBonus = weapon.EnhancementBonus;
            Damage = weapon.Damage;
            Range = weapon.Range;
            Group = weapon.Group;
            Properties = weapon.Properties;
            BasePrice = weapon.BasePrice;
            Category = weapon.Category;
            IsTwoHanded = weapon.IsTwoHanded;
            Picture = weapon.Picture;
            Notes = weapon.Notes;
            IsImplement = weapon.IsImplement;
        }

        public string Name { get { return name; } set { name = value; Notify("Name"); } }
        public int ProficiencyBonus { get { return proficiencyBonus; } set { proficiencyBonus = value; Notify("ProficiencyBonus"); Notify("TotalBonus"); } }
        public int EnhancementBonus { get { return enhancementBonus; } set { enhancementBonus = value; Notify("EnhancementBonus"); Notify("TotalBonus"); } }
        public Dice Damage { get { return damage; } set { damage = value; Notify("Damage"); } }
        public string Range { get { return range; } set { range = value; Notify("Range"); } }
        public WeaponGroup Group { get { return group; } set { group = value; Notify("Group"); } }
        public WeaponProperties Properties { get { return properties; } set { properties = value; Notify("Properties"); } }
        public int BasePrice { get { return basePrice; } set { basePrice = value; Notify("BasePrice"); } }
        public WeaponCategory Category { get { return category; } set { category = value; Notify("Category"); Notify("IsImplement"); } }
        public bool IsTwoHanded { get { return isTwoHanded; } set { isTwoHanded = value; Notify("IsTwoHanded"); } }
        public Image Picture { get { return picture; } set { picture = value; Notify("Picture"); } }
        public string Notes { get { return notes; } set { notes = value; Notify("Notes"); } }
        public bool IsImplement { get { return (isImplement || (category == WeaponCategory.PureImplement)); } set { isImplement = value; Notify("IsImplement"); } }

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
