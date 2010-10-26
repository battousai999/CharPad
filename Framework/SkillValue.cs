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
        private bool isTrained;
        private BasicAdjustmentList miscAdjustments;

        public SkillValue(Player player, Skill skill, bool isTrained)
        {
            this.player = player;
            this.skill = skill;
            this.isTrained = isTrained;
            this.miscAdjustments = new BasicAdjustmentList();

            player.PropertyChanged += new PropertyChangedEventHandler(player_PropertyChanged);
            miscAdjustments.ContainedElementChanged += new PropertyChangedEventHandler(miscAdjustments_ContainedElementChanged);
        }

        private void player_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsAttributeProperty(e.PropertyName) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "LevelBonus") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Armor") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Shield") == 0))
            {
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

        public void AddMiscAdjustment(BasicAdjustment adjustment)
        {
            miscAdjustments.Add(adjustment);
            Notify("Value");
        }

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
