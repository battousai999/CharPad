﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class Player : INotifyPropertyChanged
    {
        private string name;
        private PlayerClass _class;
        private PlayerRace race;
        private int level;
        private int str;
        private int con;
        private int dex;
        private int _int;
        private int wis;
        private int cha;
        private int hitPoints;
        private int surgeValue;
        private int surgesPerDay;
        private SkillValue acrobatics;
        private SkillValue arcana;
        private SkillValue athletics;
        private SkillValue bluff;
        private SkillValue diplomacy;
        private SkillValue dungeoneering;
        private SkillValue endurance;
        private SkillValue heal;
        private SkillValue history;
        private SkillValue insight;
        private SkillValue intimidate;
        private SkillValue nature;
        private SkillValue perception;
        private SkillValue religion;
        private SkillValue stealth;
        private SkillValue streetwise;
        private SkillValue thievery;

        public string Name { get { return name; } set { name = value; Notify("Name"); } }
        public PlayerClass Class { get { return _class; } set { _class = value; Notify("Class"); } }
        public PlayerRace Race { get { return race; } set { race = value; Notify("Race"); } }
        public int Level { get { return level; } set { level = value; Notify("Level"); } }
        public int Str { get { return str; } set { str = value; Notify("Str"); } }
        public int Con { get { return con; } set { con = value; Notify("Con"); } }
        public int Dex { get { return dex; } set { dex = value; Notify("Dex"); } }
        public int Int { get { return _int; } set { _int = value; Notify("Int"); } }
        public int Wis { get { return wis; } set { wis = value; Notify("Wis"); } }
        public int Cha { get { return cha; } set { cha = value; Notify("Cha"); } }
        public int HitPoints { get { return hitPoints; } set { hitPoints = value; Notify("HitPoints"); } }
        public int SurgeValue { get { return surgeValue; } set { surgeValue = value; Notify("SurgeValue"); } }
        public int SurgesPerDay { get { return surgesPerDay; } set { surgesPerDay = value; Notify("SurgesPerDay"); } }
        public SkillValue Acrobatics { get { return acrobatics; } }
        public SkillValue Arcana { get { return arcana; } }
        public SkillValue Athletics { get { return athletics; } }
        public SkillValue Bluff { get { return bluff; } }
        public SkillValue Diplomacy { get { return diplomacy; } }
        public SkillValue Dungeoneering { get { return dungeoneering; } }
        public SkillValue Endurance { get { return endurance; } }
        public SkillValue Heal { get { return heal; } }
        public SkillValue History { get { return history; } }
        public SkillValue Insight { get { return insight; } }
        public SkillValue Intimidate { get { return intimidate; } }
        public SkillValue Nature { get { return nature; } }
        public SkillValue Perception { get { return perception; } }
        public SkillValue Religion { get { return religion; } }
        public SkillValue Stealth { get { return stealth; } }
        public SkillValue Streetwise { get { return streetwise; } }
        public SkillValue Thievery { get { return thievery; } }

        public Player()
        {
            this.name = "";
            this.acrobatics = new SkillValue(this, Skill.Acrobatics, false);
            this.arcana = new SkillValue(this, Skill.Arcana, false);
            this.athletics = new SkillValue(this, Skill.Athletics, false);
            this.bluff = new SkillValue(this, Skill.Bluff, false);
            this.diplomacy = new SkillValue(this, Skill.Diplomacy, false);
            this.dungeoneering = new SkillValue(this, Skill.Dungeoneering, false);
            this.endurance = new SkillValue(this, Skill.Endurance, false);
            this.heal = new SkillValue(this, Skill.Heal, false);
            this.history = new SkillValue(this, Skill.History, false);
            this.insight = new SkillValue(this, Skill.Insight, false);
            this.intimidate = new SkillValue(this, Skill.Intimidate, false);
            this.nature = new SkillValue(this, Skill.Nature, false);
            this.perception = new SkillValue(this, Skill.Perception, false);
            this.religion = new SkillValue(this, Skill.Religion, false);
            this.stealth = new SkillValue(this, Skill.Stealth, false);
            this.streetwise = new SkillValue(this, Skill.Streetwise, false);
            this.thievery = new SkillValue(this, Skill.Thievery, false);
        }

        public static int GetAttributeModifier(int attributeValue)
        {
            return (attributeValue >= 10 ? (attributeValue - 10) / 2 : (attributeValue - 11) / 2);
        }

        public int LevelBonus
        {
            get { return (Level / 2); }
        }

        public int StrModifier { get { return GetAttributeModifier(str); } }
        public int ConModifier { get { return GetAttributeModifier(con); } }
        public int DexModifier { get { return GetAttributeModifier(dex); } }
        public int IntModifier { get { return GetAttributeModifier(_int); } }
        public int WisModifier { get { return GetAttributeModifier(wis); } }
        public int ChaModifier { get { return GetAttributeModifier(cha); } }

        public int BloodiedValue { get { return (hitPoints / 2); } }
        public int BasicSurgeValue { get { return (hitPoints / 4); } }

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
