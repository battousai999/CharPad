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
        private List<BasicAdjustment> miscAdjustments;

        public SkillValue(Player player, Skill skill, bool isTrained)
        {
            this.player = player;
            this.skill = skill;
            this.isTrained = isTrained;
            this.miscAdjustments = new List<BasicAdjustment>();
        }

        public Skill Skill { get { return skill; } }
        public Player Player { get { return player; } }
        public bool IsTrained { get { return isTrained; } set { isTrained = value; Notify("Value"); Notify("IsTrained"); } }
        public List<BasicAdjustment> MiscAdjustments { get { return miscAdjustments; } }

        public void AddMiscAdjustment(BasicAdjustment adjustment)
        {
            miscAdjustments.Add(adjustment);
            Notify("Value");
        }

        public int Value
        {
            get
            {
                return player.LevelBonus + GetAttributeBonus() + (isTrained ? 5 : 0) + GetArmorAdjustment() +
                    miscAdjustments.Sum(x => x.Modifier);
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
                    return 0; // TODO: Determine armor penalty...
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
