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
        private int? skillModifier;

        public Shield(string name, ArmorType armorType)
            : this(name, armorType, 0)
        {
        }

        public Shield(string name, ArmorType armorType, int enhancementBonus)
            : this(name, armorType, GetBasicArmorBonus(armorType), enhancementBonus)
        {
        }

        public Shield(string name, ArmorType armorType, int armorBonus, int enhancementBonus)
        {
            if ((armorType != ArmorType.LightShield) && (armorType != ArmorType.HeavyShield))
                throw new InvalidOperationException("Unexpected armor type: " + Enum.Format(typeof(ArmorType), armorType, "G"));

            this.name = name;
            this.armorType = armorType;
            this.armorBonus = armorBonus;
            this.enhancementBonus = enhancementBonus;
        }

        public string Name { get { return name; } set { name = value; Notify("Name"); } }
        public ArmorType ArmorType { get { return armorType; } set { armorType = value; NotifyAll(); } }
        public int ArmorBonus { get { return armorBonus; } set { armorBonus = value; Notify("ArmorBonus"); Notify("TotalBonus"); } }
        public int EnhancementBonus { get { return enhancementBonus; } set { enhancementBonus = value; Notify("EnhancementBonus"); } }

        public int SkillModifier { get { return (skillModifier == null ? GetArmorTypeSkillModifier() : skillModifier.Value); } }

        public int TotalBonus { get { return ArmorBonus + EnhancementBonus; } }

        public void SetSkillModifier(int? value)
        {
            skillModifier = value;
            Notify("SkillModifier");
        }

        private void NotifyAll()
        {
            Notify("Name");
            Notify("ArmorType");
            Notify("EnhancementBonus");
            Notify("SkillModifier");
            Notify("Category");
        }

        private static int GetBasicArmorBonus(ArmorType armorType)
        {
            switch (armorType)
            {
                case ArmorType.LightShield:
                    return 1;
                case ArmorType.HeavyShield:
                    return 2;
                default:
                    throw new InvalidOperationException("Unexpected armor type: " + Enum.Format(typeof(ArmorType), armorType, "G"));
            }
        }

        private int GetArmorTypeSkillModifier()
        {
            switch (armorType)
            {
                case ArmorType.LightShield:
                    return 0;
                case ArmorType.HeavyShield:
                    return -1;
                default:
                    throw new InvalidOperationException("Unexpected armor type: " + Enum.Format(typeof(ArmorType), armorType, "G"));
            }
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
