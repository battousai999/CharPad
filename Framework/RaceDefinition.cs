﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharPad.Framework
{
    public class RaceDefinition
    {
        private static Dictionary<string, RaceDefinition> definitionCache = new Dictionary<string, RaceDefinition>(StringComparer.CurrentCultureIgnoreCase);

        private string Name { get; set; }
        private int Speed { get; set; }
        private List<SkillBonus> SkillBonues { get; set; }
        private List<AttributeBonus> AttributeBonuses { get; set; }

        public RaceDefinition()
        {
            SkillBonues = new List<SkillBonus>();
            AttributeBonuses = new List<AttributeBonus>();
        }

        #region

        static RaceDefinition()
        {
            // PHB #1
            RaceDefinition def = new RaceDefinition();
            def.Name = "Dragonborn";
            def.Speed = 6;
            def.SkillBonues = new List<SkillBonus> { new SkillBonus(2, Skill.History), new SkillBonus(2, Skill.Intimidate) };
            def.AttributeBonuses = new List<AttributeBonus> { new AttributeBonus(2, AttributeType.Strength), new AttributeBonus(2, AttributeType.Charisma) };
            AddDefinition(def);

            def = new RaceDefinition();
            def.Name = "Dwarf";
            def.Speed = 5;
            def.SkillBonues = new List<SkillBonus> { new SkillBonus(2, Skill.Dungeoneering), new SkillBonus(2, Skill.Endurance) };
            def.AttributeBonuses = new List<AttributeBonus> { new AttributeBonus(2, AttributeType.Constitution), new AttributeBonus(2, AttributeType.Wisdom) };
            AddDefinition(def);

            def = new RaceDefinition();
            def.Name = "Eladrin";
            def.Speed = 6;
            def.SkillBonues = new List<SkillBonus> { new SkillBonus(2, Skill.Arcana), new SkillBonus(2, Skill.History) };
            def.AttributeBonuses = new List<AttributeBonus> { new AttributeBonus(2, AttributeType.Dexterity), new AttributeBonus(2, AttributeType.Intelligence) };
            AddDefinition(def);

            def = new RaceDefinition();
            def.Name = "Elf";
            def.Speed = 7;
            def.SkillBonues = new List<SkillBonus> { new SkillBonus(2, Skill.Nature), new SkillBonus(2, Skill.Perception) };
            def.AttributeBonuses = new List<AttributeBonus> { new AttributeBonus(2, AttributeType.Dexterity), new AttributeBonus(2, AttributeType.Wisdom) };
            AddDefinition(def);

            def = new RaceDefinition();
            def.Name = "Half-Elf";
            def.Speed = 6;
            def.SkillBonues = new List<SkillBonus> { new SkillBonus(2, Skill.Diplomacy), new SkillBonus(2, Skill.Insight) };
            def.AttributeBonuses = new List<AttributeBonus> { new AttributeBonus(2, AttributeType.Constitution), new AttributeBonus(2, AttributeType.Charisma) };
            AddDefinition(def);

            def = new RaceDefinition();
            def.Name = "Halfling";
            def.Speed = 6;
            def.SkillBonues = new List<SkillBonus> { new SkillBonus(2, Skill.Acrobatics), new SkillBonus(2, Skill.Thievery) };
            def.AttributeBonuses = new List<AttributeBonus> { new AttributeBonus(2, AttributeType.Dexterity), new AttributeBonus(2, AttributeType.Charisma) };
            AddDefinition(def);

            def = new RaceDefinition();
            def.Name = "Human";
            def.Speed = 6;
            def.AttributeBonuses = new List<AttributeBonus> { new AttributeBonus(2, AttributeType.Wildcard) };
            AddDefinition(def);

            def = new RaceDefinition();
            def.Name = "Tiefling";
            def.Speed = 6;
            def.SkillBonues = new List<SkillBonus> { new SkillBonus(2, Skill.Bluff), new SkillBonus(2, Skill.Stealth) };
            def.AttributeBonuses = new List<AttributeBonus> { new AttributeBonus(2, AttributeType.Intelligence), new AttributeBonus(2, AttributeType.Charisma) };
            AddDefinition(def);

            // PHB #2
            def = new RaceDefinition();
            def.Name = "Deva";
            def.Speed = 6;
            def.SkillBonues = new List<SkillBonus> { new SkillBonus(2, Skill.History), new SkillBonus(2, Skill.Religion) };
            def.AttributeBonuses = new List<AttributeBonus> { new AttributeBonus(2, AttributeType.Intelligence), new AttributeBonus(2, AttributeType.Wisdom) };
            AddDefinition(def);

            def = new RaceDefinition();
            def.Name = "Gnome";
            def.Speed = 5;
            def.SkillBonues = new List<SkillBonus> { new SkillBonus(2, Skill.Arcana), new SkillBonus(2, Skill.Stealth) };
            def.AttributeBonuses = new List<AttributeBonus> { new AttributeBonus(2, AttributeType.Intelligence), new AttributeBonus(2, AttributeType.Charisma) };
            AddDefinition(def);

            def = new RaceDefinition();
            def.Name = "Goliath";
            def.Speed = 6;
            def.SkillBonues = new List<SkillBonus> { new SkillBonus(2, Skill.Athletics), new SkillBonus(2, Skill.Nature) };
            def.AttributeBonuses = new List<AttributeBonus> { new AttributeBonus(2, AttributeType.Strength), new AttributeBonus(2, AttributeType.Constitution) };
            AddDefinition(def);

            def = new RaceDefinition();
            def.Name = "Half-Orc";
            def.Speed = 6;
            def.SkillBonues = new List<SkillBonus> { new SkillBonus(2, Skill.Endurance), new SkillBonus(2, Skill.Intimidate) };
            def.AttributeBonuses = new List<AttributeBonus> { new AttributeBonus(2, AttributeType.Strength), new AttributeBonus(2, AttributeType.Dexterity) };
            AddDefinition(def);

            def = new RaceDefinition();
            def.Name = "Longtooth Shifter";
            def.Speed = 6;
            def.SkillBonues = new List<SkillBonus> { new SkillBonus(2, Skill.Athletics), new SkillBonus(2, Skill.Endurance) };
            def.AttributeBonuses = new List<AttributeBonus> { new AttributeBonus(2, AttributeType.Strength), new AttributeBonus(2, AttributeType.Wisdom) };
            AddDefinition(def);

            def = new RaceDefinition();
            def.Name = "Razorclaw Shifter";
            def.Speed = 6;
            def.SkillBonues = new List<SkillBonus> { new SkillBonus(2, Skill.Acrobatics), new SkillBonus(2, Skill.Stealth) };
            def.AttributeBonuses = new List<AttributeBonus> { new AttributeBonus(2, AttributeType.Dexterity), new AttributeBonus(2, AttributeType.Wisdom) };
            AddDefinition(def);

            // PHB #3
            def = new RaceDefinition();
            def.Name = "Githzerai";
            def.Speed = 6;
            def.SkillBonues = new List<SkillBonus> { new SkillBonus(2, Skill.Acrobatics), new SkillBonus(2, Skill.Athletics) };
            def.AttributeBonuses = new List<AttributeBonus> { new AttributeBonus(2, AttributeType.Wisdom), new AttributeBonus(2, AttributeType.Dexterity, AttributeType.Intelligence) };
            AddDefinition(def);

            def = new RaceDefinition();
            def.Name = "Minotaur";
            def.Speed = 6;
            def.SkillBonues = new List<SkillBonus> { new SkillBonus(2, Skill.Nature), new SkillBonus(2, Skill.Perception) };
            def.AttributeBonuses = new List<AttributeBonus> { new AttributeBonus(2, AttributeType.Strength), new AttributeBonus(2, AttributeType.Constitution, AttributeType.Wisdom) };
            AddDefinition(def);

            def = new RaceDefinition();
            def.Name = "Shardmind";
            def.Speed = 6;
            def.SkillBonues = new List<SkillBonus> { new SkillBonus(2, Skill.Arcana), new SkillBonus(2, Skill.Endurance), new SkillBonus(2, Skill.Wildcard) };
            def.AttributeBonuses = new List<AttributeBonus> { new AttributeBonus(2, AttributeType.Intelligence), new AttributeBonus(2, AttributeType.Wisdom, AttributeType.Charisma) };
            AddDefinition(def);

            def = new RaceDefinition();
            def.Name = "Wilden";
            def.Speed = 6;
            def.SkillBonues = new List<SkillBonus> { new SkillBonus(2, Skill.Nature), new SkillBonus(2, Skill.Stealth) };
            def.AttributeBonuses = new List<AttributeBonus> { new AttributeBonus(2, AttributeType.Wisdom), new AttributeBonus(2, AttributeType.Constitution, AttributeType.Dexterity) };
            AddDefinition(def);
        }

        #endregion

        private static void AddDefinition(RaceDefinition def)
        {
            definitionCache.Add(def.Name, def);
        }

        public static RaceDefinition GetClass(string name)
        {
            if (!definitionCache.ContainsKey(name))
                return null;

            return definitionCache[name];
        }

        public static List<RaceDefinition> GetClasses()
        {
            List<RaceDefinition> list = new List<RaceDefinition>(definitionCache.Values);

            list.Sort((x, y) => StringComparer.CurrentCultureIgnoreCase.Compare(x.Name, y.Name));

            return list;
        }
    }
}
