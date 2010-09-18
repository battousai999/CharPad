using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharPad.Framework
{
    public class ClassDefinition
    {
        private static Dictionary<string, ClassDefinition> definitionCache = new Dictionary<string, ClassDefinition>(StringComparer.CurrentCultureIgnoreCase);

        public string Name { get; set; }
        public int BaseHealth { get; set; }
        public int HealthPerLevel { get; set; }
        public int BaseHealingSurges { get; set; }
        public List<ArmorType> ArmorProficiencies { get; set; }
        public List<WeaponCategory> WeaponProficiencies { get; set; }
        public List<WeaponType> SpecificWeaponProficiencies { get; set; }
        public List<DefenseBonus> DefenseBonuses { get; set; }
        public List<Skill> AutomaticSkills { get; set; }
        public int StartingSkills { get; set; }
        public List<Skill> TrainableSkills { get; set; }

        public ClassDefinition()
        {
            this.ArmorProficiencies = new List<ArmorType>();
            this.WeaponProficiencies = new List<WeaponCategory>();
            this.SpecificWeaponProficiencies = new List<WeaponType>();
            this.DefenseBonuses = new List<DefenseBonus>();
            this.AutomaticSkills = new List<Skill>();
            this.TrainableSkills = new List<Skill>();
        }

        #region Initialization

        static ClassDefinition()
        {
            ClassDefinition def = new ClassDefinition();
            def.Name = "Cleric";
            def.BaseHealth = 12;
            def.HealthPerLevel = 5;
            def.BaseHealingSurges = 7;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth, ArmorType.Leather, ArmorType.Hide, ArmorType.Chainmail };
            def.WeaponProficiencies = new List<WeaponCategory> { WeaponCategory.SimpleMelee, WeaponCategory.SimpleRanged };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(2, DefenseType.Will) };
            def.AutomaticSkills = new List<Skill> { Skill.Religion };
            def.StartingSkills = 3;
            def.TrainableSkills = new List<Skill> { Skill.Arcana, Skill.Diplomacy, Skill.Heal, Skill.History, Skill.Insight, Skill.Religion };
            AddDefinition(def);

            def = new ClassDefinition();
            def.Name = "Fighter";
            def.BaseHealth = 15;
            def.HealthPerLevel = 6;
            def.BaseHealingSurges = 9;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth, ArmorType.Leather, ArmorType.Hide, ArmorType.Chainmail, ArmorType.Scale, ArmorType.LightShield, ArmorType.HeavyShield };
            def.WeaponProficiencies = new List<WeaponCategory> { WeaponCategory.SimpleMelee, WeaponCategory.MilitaryMelee, WeaponCategory.SimpleRanged, WeaponCategory.MilitaryRanged };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(2, DefenseType.Fortitude) };
            def.StartingSkills = 3;
            def.TrainableSkills = new List<Skill> { Skill.Athletics, Skill.Endurance, Skill.History, Skill.Intimidate, Skill.Streetwise };
            AddDefinition(def);

            def = new ClassDefinition();
            def.Name = "Paladin";
            def.BaseHealth = 15;
            def.HealthPerLevel = 6;
            def.BaseHealingSurges = 10;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth, ArmorType.Leather, ArmorType.Hide, ArmorType.Chainmail, ArmorType.Scale, ArmorType.Plate, ArmorType.LightShield, ArmorType.HeavyShield };
            def.WeaponProficiencies = new List<WeaponCategory> { WeaponCategory.SimpleMelee, WeaponCategory.MilitaryMelee, WeaponCategory.SimpleRanged };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(1, DefenseType.Fortitude), new DefenseBonus(1, DefenseType.Reflex), new DefenseBonus(1, DefenseType.Will) };
            def.AutomaticSkills = new List<Skill> { Skill.Religion };
            def.StartingSkills = 3;
            def.TrainableSkills = new List<Skill> { Skill.Diplomacy, Skill.Endurance, Skill.Heal, Skill.History, Skill.Insight, Skill.Intimidate, Skill.Religion };
            AddDefinition(def);

            def = new ClassDefinition();
            def.Name = "Ranger";
            def.BaseHealth = 12;
            def.HealthPerLevel = 5;
            def.BaseHealingSurges = 6;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth, ArmorType.Leather, ArmorType.Hide };
            def.WeaponProficiencies = new List<WeaponCategory> { WeaponCategory.SimpleMelee, WeaponCategory.MilitaryMelee, WeaponCategory.SimpleRanged, WeaponCategory.MilitaryRanged };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(1, DefenseType.Fortitude), new DefenseBonus(1, DefenseType.Reflex) };
            def.StartingSkills = 5;
            def.TrainableSkills = new List<Skill> { Skill.Acrobatics, Skill.Athletics, Skill.Dungeoneering, Skill.Endurance, Skill.Heal, Skill.Nature, Skill.Perception, Skill.Stealth };
            AddDefinition(def);

            def = new ClassDefinition();
            def.Name = "Rogue";
            def.BaseHealth = 12;
            def.HealthPerLevel = 5;
            def.BaseHealingSurges = 6;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth, ArmorType.Leather };
            def.SpecificWeaponProficiencies = new List<WeaponType> { WeaponType.Dagger, WeaponType.HandCrossbow, WeaponType.Shuriken, WeaponType.Sling, WeaponType.ShortSword };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(2, DefenseType.Reflex) };
            def.AutomaticSkills = new List<Skill> { Skill.Stealth, Skill.Thievery };
            def.StartingSkills = 4;
            def.TrainableSkills = new List<Skill> { Skill.Acrobatics, Skill.Athletics, Skill.Bluff, Skill.Dungeoneering, Skill.Insight, Skill.Intimidate, Skill.Perception, Skill.Stealth, Skill.Streetwise, Skill.Thievery };
            AddDefinition(def);

            def = new ClassDefinition();
            def.Name = "Warlock";
            def.BaseHealth = 12;
            def.HealthPerLevel = 5;
            def.BaseHealingSurges = 6;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth, ArmorType.Leather };
            def.WeaponProficiencies = new List<WeaponCategory> { WeaponCategory.SimpleMelee, WeaponCategory.SimpleRanged };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(1, DefenseType.Reflex), new DefenseBonus(1, DefenseType.Will) };
            def.StartingSkills = 4;
            def.TrainableSkills = new List<Skill> { Skill.Arcana, Skill.Bluff, Skill.History, Skill.Insight, Skill.Intimidate, Skill.Religion, Skill.Streetwise, Skill.Thievery };
            AddDefinition(def);

            def = new ClassDefinition();
            def.Name = "Warlord";
            def.BaseHealth = 12;
            def.HealthPerLevel = 5;
            def.BaseHealingSurges = 7;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth, ArmorType.Leather, ArmorType.Hide, ArmorType.Chainmail, ArmorType.LightShield };
            def.WeaponProficiencies = new List<WeaponCategory> { WeaponCategory.SimpleMelee, WeaponCategory.MilitaryMelee, WeaponCategory.SimpleRanged };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(1, DefenseType.Fortitude), new DefenseBonus(1, DefenseType.Will) };
            def.StartingSkills = 4;
            def.TrainableSkills = new List<Skill> { Skill.Athletics, Skill.Diplomacy, Skill.Endurance, Skill.Heal, Skill.History, Skill.Intimidate };
            AddDefinition(def);

            def = new ClassDefinition();
            def.Name = "Wizard";
            def.BaseHealth = 10;
            def.HealthPerLevel = 4;
            def.BaseHealingSurges = 6;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth };
            def.SpecificWeaponProficiencies = new List<WeaponType> { WeaponType.Dagger, WeaponType.Quarterstaff };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(2, DefenseType.Will) };
            def.AutomaticSkills = new List<Skill> { Skill.Arcana };
            def.StartingSkills = 3;
            def.TrainableSkills = new List<Skill> { Skill.Arcana, Skill.Diplomacy, Skill.Dungeoneering, Skill.History, Skill.Insight, Skill.Nature, Skill.Religion };
            AddDefinition(def);

        }

        #endregion

        private static void AddDefinition(ClassDefinition def)
        {
            definitionCache.Add(def.Name, def);
        }

        public static ClassDefinition GetClass(string name)
        {
            if (!definitionCache.ContainsKey(name))
                return null;

            return definitionCache[name];
        }

        public static List<ClassDefinition> GetClasses()
        {
            List<ClassDefinition> list = new List<ClassDefinition>(definitionCache.Values);

            list.Sort((x, y) => StringComparer.CurrentCultureIgnoreCase.Compare(x.Name, y.Name));

            return list;
        }
    }
}
