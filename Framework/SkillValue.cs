using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class SkillValue : INotifyPropertyChanged
    {
        private Skill skill;
        private Player player;
        private Armor playerArmor;
        private Shield playerShield;
        private bool isTrained;
        private BasicAdjustmentList miscAdjustments;

        public SkillValue(Player player, Skill skill, bool isTrained)
        {
            this.player = player;
            this.skill = skill;
            this.isTrained = isTrained;
            this.miscAdjustments = new BasicAdjustmentList();

            player.PropertyChanged += new PropertyChangedEventHandler(player_PropertyChanged);

            if (player.Armor != null)
                player.Armor.PropertyChanged += new PropertyChangedEventHandler(Armor_PropertyChanged);

            playerArmor = player.Armor;

            if (player.Shield != null)
                player.Shield.PropertyChanged += new PropertyChangedEventHandler(Shield_PropertyChanged);

            playerShield = player.Shield;

            miscAdjustments.ContainedElementChanged += new PropertyChangedEventHandler(miscAdjustments_ContainedElementChanged);
        }

        void Shield_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("ArmorAdjustment");
            Notify("Value");
        }

        void Armor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("ArmorAdjustment");
            Notify("Value");
        }

        private void player_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttributeProperty(e.PropertyName))
            {
                Notify("AttributeBonus");
                Notify("Value");
            }

            if (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Armor") == 0)
            {
                if (playerArmor != null)
                    playerArmor.PropertyChanged -= new PropertyChangedEventHandler(Armor_PropertyChanged);

                if (player.Armor != null)
                    player.Armor.PropertyChanged += new PropertyChangedEventHandler(Armor_PropertyChanged);

                playerArmor = player.Armor;

                Notify("ArmorAdjustment");
                Notify("Value");
            }

            if (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Shield") == 0)
            {
                if (playerShield != null)
                    playerShield.PropertyChanged += new PropertyChangedEventHandler(Shield_PropertyChanged);

                if (player.Shield != null)
                    player.Shield.PropertyChanged += new PropertyChangedEventHandler(Shield_PropertyChanged);

                playerShield = player.Shield;

                Notify("ArmorAdjustment");
                Notify("Value");
            }

            if (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "LevelBonus") == 0)
            {
                Notify("LevelBonus");
                Notify("Value");
            }
        }

        private bool IsAttributeProperty(string propertyName)
        {
            return ((StringComparer.CurrentCultureIgnoreCase.Compare(propertyName, "StrModifier") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(propertyName, "ConModifier") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(propertyName, "DexModifier") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(propertyName, "IntModifier") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(propertyName, "WisModifier") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(propertyName, "ChaModifier") == 0));
        }

        public Skill Skill { get { return skill; } }
        public Player Player { get { return player; } }
        public bool IsTrained { get { return isTrained; } set { isTrained = value; Notify("Value"); Notify("IsTrained"); } }
        public BasicAdjustmentList MiscAdjustments { get { return miscAdjustments; } }

        private void miscAdjustments_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("Value");
        }

        public int Value
        {
            get
            {
                return player.LevelBonus + GetAttributeBonus() + (isTrained ? 5 : 0) + GetArmorAdjustment() +
                    miscAdjustments.TotalAdjustment;
            }
        }

        public int LevelBonus
        {
            get { return player.LevelBonus; }
        }

        public int AttributeBonus
        {
            get { return GetAttributeBonus(); }
        }

        public int TrainedBonus
        {
            get { return (isTrained ? 5 : 0); }
        }

        public int ArmorAdjustment
        {
            get { return GetArmorAdjustment(); }
        }

        public int TotalMiscAdjustment
        {
            get { return miscAdjustments.TotalAdjustment; }
        }

        public AttributeType AttributeBonusType
        {
            get { return GetAttributeBonusType(); }
        }

        private int GetAttributeBonus()
        {
            switch (skill)
            {
                case Skill.Athletics:
                    return player.StrModifier;
                case Skill.Endurance:
                    return player.ConModifier;
                case Skill.Acrobatics:
                case Skill.Stealth:
                case Skill.Thievery:
                    return player.DexModifier;
                case Skill.Arcana:
                case Skill.History:
                case Skill.Religion:
                    return player.IntModifier;
                case Skill.Dungeoneering:
                case Skill.Heal:
                case Skill.Insight:
                case Skill.Nature:
                case Skill.Perception:
                    return player.WisModifier;
                case Skill.Bluff:
                case Skill.Diplomacy:
                case Skill.Intimidate:
                case Skill.Streetwise:
                    return player.ChaModifier;
                default:
                    throw new InvalidOperationException("Unexpected skill: " + Enum.Format(typeof(Skill), skill, "G"));
            }
        }

        private AttributeType GetAttributeBonusType()
        {
            switch (skill)
            {
                case Skill.Athletics:
                    return AttributeType.Strength;
                case Skill.Endurance:
                    return AttributeType.Constitution;
                case Skill.Acrobatics:
                case Skill.Stealth:
                case Skill.Thievery:
                    return AttributeType.Dexterity;
                case Skill.Arcana:
                case Skill.History:
                case Skill.Religion:
                    return AttributeType.Intelligence;
                case Skill.Dungeoneering:
                case Skill.Heal:
                case Skill.Insight:
                case Skill.Nature:
                case Skill.Perception:
                    return AttributeType.Wisdom;
                case Skill.Bluff:
                case Skill.Diplomacy:
                case Skill.Intimidate:
                case Skill.Streetwise:
                    return AttributeType.Charisma;
                default:
                    throw new InvalidOperationException("Unexpected skill: " + Enum.Format(typeof(Skill), skill, "G"));
            }
        }

        public int GetArmorAdjustment()
        {
            switch (skill)
            {
                case Skill.Acrobatics:
                case Skill.Athletics:
                case Skill.Endurance:
                case Skill.Stealth:
                case Skill.Thievery:
                    return (player.Armor == null ? 0 : player.Armor.SkillModifier) + (player.Shield == null ? 0 : player.Shield.SkillModifier);
                default:
                    return 0;
            }
        }

        private void Notify(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
