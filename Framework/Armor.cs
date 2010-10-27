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
        private int? skillModifier;
        private int? speedModifier;

        public Armor(string name, ArmorType armorType)
            : this(name, armorType, 0)
        {
        }

        public Armor(string name, ArmorType armorType, int enhancementBonus)
            : this(name, armorType, GetBasicArmorBonus(armorType), enhancementBonus)
        {
        }

        public Armor(string name, ArmorType armorType, int armorBonus, int enhancementBonus)
        {
            if ((armorType != ArmorType.Cloth) &&
                (armorType != ArmorType.Leather) &&
                (armorType != ArmorType.Hide) &&
                (armorType != ArmorType.Chainmail) &&
                (armorType != ArmorType.Scale) &&
                (armorType != ArmorType.Plate))
            {
                throw new InvalidOperationException("Unexpected armor type: " + Enum.Format(typeof(ArmorType), armorType, "G"));
            }

            this.name = name;
            this.armorType = armorType;
            this.armorBonus = armorBonus;
            this.enhancementBonus = enhancementBonus;
        }

        public string Name { get { return name; } set { name = value; Notify("Name"); } }
        public ArmorType ArmorType { get { return armorType; } set { armorType = value; NotifyAll(); } }
        public int ArmorBonus { get { return armorBonus; } set { armorBonus = value; Notify("ArmorBonus"); Notify("TotalBonus"); } }
        public int EnhancementBonus { get { return enhancementBonus; } set { enhancementBonus = value; Notify("EnhancementBonus"); Notify("TotalBonus"); } }

        public int SkillModifier { get { return (skillModifier == null ? GetArmorTypeSkillModifier() : skillModifier.Value); } }
        public int SpeedModifier { get { return (speedModifier == null ? GetArmorTypeSpeedModifier() : speedModifier.Value); } }

        public int TotalBonus { get { return ArmorBonus + EnhancementBonus; } }

        public bool IsHeavy
        {
            get
            {
                switch (armorType)
                {
                    case ArmorType.Cloth:
                    case ArmorType.Leather:
                    case ArmorType.Hide:
                        return false;
                    default:
                        return true;
                }
            }
        }

        public void SetSkillModifier(int? value)
        {
            skillModifier = value;
            Notify("SkillModifier");
        }

        public void SetSpeedModifier(int? value)
        {
            speedModifier = value;
            Notify("SpeedModifier");
        }

        private void NotifyAll()
        {
            Notify("Name");
            Notify("ArmorType");
            Notify("EnhancementBonus");
            Notify("SkillModifier");
            Notify("SpeedModifier");
            Notify("Category");
        }

        private static int GetBasicArmorBonus(ArmorType armorType)
        {
            switch (armorType)
            {
                case ArmorType.Cloth:
                    return 0;
                case ArmorType.Leather:
                    return 2;
                case ArmorType.Hide:
                    return 3;
                case ArmorType.Chainmail:
                    return 6;
                case ArmorType.Scale:
                    return 7;
                case ArmorType.Plate:
                    return 8;
                default:
                    throw new InvalidOperationException("Unexpected armor type: " + Enum.Format(typeof(ArmorType), armorType, "G"));
            }
        }

        private int GetArmorTypeSkillModifier()
        {
            switch (armorType)
            {
                case ArmorType.Cloth:
                case ArmorType.Leather:
                case ArmorType.Scale:
                    return 0;
                case ArmorType.Hide:
                case ArmorType.Chainmail:
                    return -1;
                case ArmorType.Plate:
                    return -2;
                default:
                    throw new InvalidOperationException("Unexpected armor type: " + Enum.Format(typeof(ArmorType), armorType, "G"));
            }
        }

        private int GetArmorTypeSpeedModifier()
        {
            return (IsHeavy ? -1 : 0);
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
