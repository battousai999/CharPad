using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;

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
        private Image picture;
        private string notes;

        public Armor()
        {
        }

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

        public void CopyValues(Armor armor)
        {
            Name = armor.Name;
            ArmorType = armor.ArmorType;
            ArmorBonus = armor.ArmorBonus;
            EnhancementBonus = armor.EnhancementBonus;
            SkillModifier = armor.SkillModifier;
            SpeedModifier = armor.SpeedModifier;
            BasePrice = armor.BasePrice;
            IsHeavy = armor.IsHeavy;
            SpecialProperty = armor.SpecialProperty;
            MinEnhancementBonus = armor.MinEnhancementBonus;
            Picture = armor.Picture;
            Notes = armor.Notes;
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
        public Image Picture { get { return picture; } set { picture = value; Notify("Picture"); } }
        public string Notes { get { return notes; } set { notes = value; Notify("Notes"); } }

        public int TotalBonus { get { return ArmorBonus + EnhancementBonus; } }

        public IInventoryItem Clone()
        {
            Armor armor = new Armor();

            armor.name = this.name;
            armor.armorType = this.armorType;
            armor.enhancementBonus = this.enhancementBonus;
            armor.armorBonus = this.armorBonus;
            armor.skillModifier = this.skillModifier;
            armor.speedModifier = this.speedModifier;
            armor.basePrice = this.basePrice;
            armor.isHeavy = this.isHeavy;
            armor.specialProperty = this.specialProperty;
            armor.minEnhancementBonus = this.minEnhancementBonus;
            armor.picture = this.picture;
            armor.notes = this.notes;

            return armor;
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
