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

        public Weapon(string name, int proficiencyBonus, Dice damage, string range, WeaponGroup group, WeaponProperties properties)
            : this(name, proficiencyBonus, damage, range, group, properties, 0)
        {
        }

        public Weapon(string name, int proficiencyBonus, Dice damage, string range, WeaponGroup group, WeaponProperties properties, int enhancementBonus)
        {
            this.name = name;
            this.proficiencyBonus = proficiencyBonus;
            this.damage = damage;
            this.range = range;
            this.group = group;
            this.properties = properties;
            this.enhancementBonus = enhancementBonus;
        }

        public string Name { get { return name; } set { name = value; Notify("Name"); } }
        public int ProficiencyBonus { get { return proficiencyBonus; } set { proficiencyBonus = value; Notify("ProficiencyBonus"); Notify("TotalBonus"); } }
        public int EnhancementBonus { get { return enhancementBonus; } set { enhancementBonus = value; Notify("EnhancementBonus"); Notify("TotalBonus"); } }
        public Dice Damage { get { return damage; } set { damage = value; Notify("Damage"); } }
        public string Range { get { return range; } set { range = value; Notify("Range"); } }
        public WeaponGroup Group { get { return group; } set { group = value; Notify("Group"); } }
        public WeaponProperties Properties { get { return properties; } set { properties = value; Notify("Properties"); } }

        public int TotalBonus { get { return proficiencyBonus + enhancementBonus; } }

        public bool IsRanged { get { return !String.IsNullOrWhiteSpace(Range); } }

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
