using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;

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
        private string notes;
        private Image picture;

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

        public void CopyValues(Shield shield)
        {
            Name = shield.Name;
            ArmorType = shield.ArmorType;
            ArmorBonus = shield.ArmorBonus;
            EnhancementBonus = shield.EnhancementBonus;
            SkillModifier = shield.SkillModifier;
            BasePrice = shield.BasePrice;
            Notes = shield.Notes;
            Picture = shield.Picture;
        }

        public string Name { get { return name; } set { name = value; Notify("Name"); } }
        public ArmorType ArmorType { get { return armorType; } set { armorType = value; Notify("ArmorType"); } }
        public int ArmorBonus { get { return armorBonus; } set { armorBonus = value; Notify("ArmorBonus"); Notify("TotalBonus"); } }
        public int EnhancementBonus { get { return enhancementBonus; } set { enhancementBonus = value; Notify("EnhancementBonus"); } }
        public int SkillModifier { get { return skillModifier; } set { skillModifier = value; Notify("SkillModifier"); } }
        public int BasePrice { get { return basePrice; } set { basePrice = value; Notify("BasePrice"); } }
        public string Notes { get { return notes; } set { notes = value; Notify("Notes"); } }
        public Image Picture { get { return picture; } set { picture = value; Notify("Picture"); } }

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
