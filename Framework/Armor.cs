using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class Armor : IInventoryItem, INotifyPropertyChanged
    {
        private string name;
        private ArmorType armorType;
        private int enhancementBonus;
        private int armorBonus;
        private int skillModifier;
        private int speedModifier;
        private int basePrice;
        private bool isHeavy;
        private string specialProperty;
        private int minEnhancementBonus;

        public Armor(string name, ArmorType armorType, int armorBonus, int skillModifier, int speedModifier, int basePrice, bool isHeavy, string specialProperty, int minEnhancementBonus)
            : this(name, armorType, armorBonus, skillModifier, speedModifier, basePrice, isHeavy, specialProperty, minEnhancementBonus, 0)
        {
        }

        public Armor(string name, ArmorType armorType, int armorBonus, int skillModifier, int speedModifier, int basePrice, bool isHeavy, string specialProperty, int minEnhancementBonus, int enhancementBonus)
        {
            this.name = name;
            this.armorType = armorType;
            this.armorBonus = armorBonus;
            this.skillModifier = skillModifier;
            this.speedModifier = speedModifier;
            this.basePrice = basePrice;
            this.isHeavy = isHeavy;
            this.specialProperty = specialProperty;
            this.minEnhancementBonus = minEnhancementBonus;
            this.enhancementBonus = enhancementBonus;
        }

        public string Name { get { return name; } set { name = value; Notify("Name"); } }
        public ArmorType ArmorType { get { return armorType; } set { armorType = value; Notify("ArmorType"); } }
        public int ArmorBonus { get { return armorBonus; } set { armorBonus = value; Notify("ArmorBonus"); Notify("TotalBonus"); } }
        public int EnhancementBonus { get { return enhancementBonus; } set { enhancementBonus = value; Notify("EnhancementBonus"); Notify("TotalBonus"); } }
        public int SkillModifier { get { return skillModifier; } set { skillModifier = value; Notify("SkillModifier"); } }
        public int SpeedModifier { get { return speedModifier; } set { speedModifier = value; Notify("SpeedModifier"); } }
        public int BasePrice { get { return basePrice; } set { basePrice = value; Notify("BasePrice"); } }
        public bool IsHeavy { get { return isHeavy; } set { isHeavy = value; Notify("IsHeavy"); } }
        public string SpecialProperty { get { return specialProperty; } set { specialProperty = value; Notify("SpecialProperty"); } }
        public int MinEnhancementBonus { get { return minEnhancementBonus; } set { minEnhancementBonus = value; Notify("MinEnhancementBonus"); } }

        public int TotalBonus { get { return ArmorBonus + EnhancementBonus; } }

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
