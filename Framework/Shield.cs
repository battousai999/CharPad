using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class Shield : IInventoryItem, INotifyPropertyChanged
    {
        private string name;
        private ArmorType armorType;
        private int enhancementBonus;
        private int armorBonus;
        private int skillModifier;
        private int basePrice;

        public Shield(string name, ArmorType armorType, int armorBonus, int skillModifier, int basePrice)
            : this(name, armorType, armorBonus, skillModifier, basePrice, 0)
        {
        }

        public Shield(string name, ArmorType armorType, int armorBonus, int skillModifier, int basePrice, int enhancementBonus)
        {
            if ((armorType != ArmorType.LightShield) && (armorType != ArmorType.HeavyShield))
                throw new InvalidOperationException("Unexpected armor type: " + Enum.Format(typeof(ArmorType), armorType, "G"));

            this.name = name;
            this.armorType = armorType;
            this.armorBonus = armorBonus;
            this.skillModifier = skillModifier;
            this.basePrice = basePrice;
            this.enhancementBonus = enhancementBonus;
        }

        public string Name { get { return name; } set { name = value; Notify("Name"); } }
        public ArmorType ArmorType { get { return armorType; } set { armorType = value; Notify("ArmorType"); } }
        public int ArmorBonus { get { return armorBonus; } set { armorBonus = value; Notify("ArmorBonus"); Notify("TotalBonus"); } }
        public int EnhancementBonus { get { return enhancementBonus; } set { enhancementBonus = value; Notify("EnhancementBonus"); } }
        public int SkillModifier { get { return skillModifier; } set { skillModifier = value; Notify("SkillModifier"); } }
        public int BasePrice { get { return basePrice; } set { basePrice = value; Notify("BasePrice"); } }

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
