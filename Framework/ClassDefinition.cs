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
        public List<AttributeType> KeyAttributes { get; set; }

        public ClassDefinition()
        {
            this.ArmorProficiencies = new List<ArmorType>();
            this.WeaponProficiencies = new List<WeaponCategory>();
            this.SpecificWeaponProficiencies = new List<WeaponType>();
            this.DefenseBonuses = new List<DefenseBonus>();
            this.AutomaticSkills = new List<Skill>();
            this.TrainableSkills = new List<Skill>();
            KeyAttributes = new List<AttributeType>();
        }

        #region Initialization

        static ClassDefinition()
        {
            // PHB 1
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
            def.KeyAttributes = new List<AttributeType> { AttributeType.Wisdom, AttributeType.Strength, AttributeType.Charisma };
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
            def.KeyAttributes = new List<AttributeType> { AttributeType.Strength, AttributeType.Dexterity, AttributeType.Wisdom, AttributeType.Constitution };
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
            def.KeyAttributes = new List<AttributeType> { AttributeType.Strength, AttributeType.Charisma, AttributeType.Wisdom };
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
            def.KeyAttributes = new List<AttributeType> { AttributeType.Strength, AttributeType.Dexterity, AttributeType.Wisdom };
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
            def.KeyAttributes = new List<AttributeType> { AttributeType.Dexterity, AttributeType.Strength, AttributeType.Charisma };
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
            def.KeyAttributes = new List<AttributeType> { AttributeType.Charisma, AttributeType.Constitution, AttributeType.Intelligence };
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
            def.KeyAttributes = new List<AttributeType> { AttributeType.Strength, AttributeType.Intelligence, AttributeType.Charisma };
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
            def.KeyAttributes = new List<AttributeType> { AttributeType.Intelligence, AttributeType.Wisdom, AttributeType.Dexterity };
            AddDefinition(def);

            // PHB 2
            def = new ClassDefinition();
            def.Name = "Avenger";
            def.BaseHealth = 14;
            def.HealthPerLevel = 6;
            def.BaseHealingSurges = 7;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth };
            def.WeaponProficiencies = new List<WeaponCategory> { WeaponCategory.SimpleMelee, WeaponCategory.MilitaryMelee, WeaponCategory.SimpleRanged };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(1, DefenseType.Fortitude), new DefenseBonus(1, DefenseType.Reflex), new DefenseBonus(1, DefenseType.Will) };
            def.AutomaticSkills = new List<Skill> { Skill.Religion };
            def.StartingSkills = 3;
            def.TrainableSkills = new List<Skill> { Skill.Acrobatics, Skill.Athletics, Skill.Endurance, Skill.Heal, Skill.Intimidate, Skill.Perception, Skill.Stealth, Skill.Streetwise };
            def.KeyAttributes = new List<AttributeType> { AttributeType.Wisdom, AttributeType.Dexterity, AttributeType.Intelligence };
            AddDefinition(def);

            def = new ClassDefinition();
            def.Name = "Barbarian";
            def.BaseHealth = 15;
            def.HealthPerLevel = 6;
            def.BaseHealingSurges = 8;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth, ArmorType.Leather, ArmorType.Hide };
            def.WeaponProficiencies = new List<WeaponCategory> { WeaponCategory.SimpleMelee, WeaponCategory.MilitaryMelee };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(2, DefenseType.Fortitude) };
            def.StartingSkills = 3;
            def.TrainableSkills = new List<Skill> { Skill.Acrobatics, Skill.Athletics, Skill.Endurance, Skill.Heal, Skill.Intimidate, Skill.Nature, Skill.Perception };
            def.KeyAttributes = new List<AttributeType> { AttributeType.Strength, AttributeType.Constitution, AttributeType.Charisma };
            AddDefinition(def);

            def = new ClassDefinition();
            def.Name = "Bard";
            def.BaseHealth = 12;
            def.HealthPerLevel = 5;
            def.BaseHealingSurges = 7;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth, ArmorType.Leather, ArmorType.Hide, ArmorType.Chainmail, ArmorType.LightShield };
            def.WeaponProficiencies = new List<WeaponCategory> { WeaponCategory.SimpleMelee, WeaponCategory.SimpleRanged, WeaponCategory.MilitaryRanged };
            def.SpecificWeaponProficiencies = new List<WeaponType> { WeaponType.Longsword, WeaponType.Scimitar, WeaponType.ShortSword };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(1, DefenseType.Reflex), new DefenseBonus(1, DefenseType.Will) };
            def.AutomaticSkills = new List<Skill> { Skill.Arcana };
            def.StartingSkills = 4;
            def.TrainableSkills = new List<Skill> { Skill.Acrobatics, Skill.Athletics, Skill.Bluff, Skill.Diplomacy, Skill.Dungeoneering, Skill.Heal, Skill.History, Skill.Insight, Skill.Intimidate, Skill.Nature, Skill.Perception, Skill.Religion, Skill.Streetwise };
            def.KeyAttributes = new List<AttributeType> { AttributeType.Charisma, AttributeType.Intelligence, AttributeType.Constitution };
            AddDefinition(def);

            def = new ClassDefinition();
            def.Name = "Druid";
            def.BaseHealth = 12;
            def.HealthPerLevel = 5;
            def.BaseHealingSurges = 7;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth, ArmorType.Leather, ArmorType.Hide };
            def.WeaponProficiencies = new List<WeaponCategory> { WeaponCategory.SimpleMelee, WeaponCategory.SimpleRanged };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(1, DefenseType.Reflex), new DefenseBonus(1, DefenseType.Will) };
            def.AutomaticSkills = new List<Skill> { Skill.Nature };
            def.StartingSkills = 3;
            def.TrainableSkills = new List<Skill> { Skill.Arcana, Skill.Athletics, Skill.Diplomacy, Skill.Endurance, Skill.Heal, Skill.History, Skill.Insight, Skill.Perception };
            def.KeyAttributes = new List<AttributeType> { AttributeType.Wisdom, AttributeType.Dexterity, AttributeType.Constitution };
            AddDefinition(def);

            def = new ClassDefinition();
            def.Name = "Invoker";
            def.BaseHealth = 10;
            def.HealthPerLevel = 4;
            def.BaseHealingSurges = 6;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth, ArmorType.Leather, ArmorType.Hide, ArmorType.Chainmail };
            def.WeaponProficiencies = new List<WeaponCategory> { WeaponCategory.SimpleMelee, WeaponCategory.SimpleRanged };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(1, DefenseType.Fortitude), new DefenseBonus(1, DefenseType.Reflex), new DefenseBonus(1, DefenseType.Will) };
            def.AutomaticSkills = new List<Skill> { Skill.Religion };
            def.StartingSkills = 3;
            def.TrainableSkills = new List<Skill> { Skill.Arcana, Skill.Diplomacy, Skill.Endurance, Skill.History, Skill.Insight, Skill.Intimidate };
            def.KeyAttributes = new List<AttributeType> { AttributeType.Wisdom, AttributeType.Constitution, AttributeType.Intelligence };
            AddDefinition(def);

            def = new ClassDefinition();
            def.Name = "Shaman";
            def.BaseHealth = 12;
            def.HealthPerLevel = 5;
            def.BaseHealingSurges = 7;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth, ArmorType.Leather };
            def.WeaponProficiencies = new List<WeaponCategory> { WeaponCategory.SimpleMelee };
            def.SpecificWeaponProficiencies = new List<WeaponType> { WeaponType.Longspear };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(1, DefenseType.Fortitude), new DefenseBonus(1, DefenseType.Will) };
            def.AutomaticSkills = new List<Skill> { Skill.Nature };
            def.StartingSkills = 3;
            def.TrainableSkills = new List<Skill> { Skill.Arcana, Skill.Athletics, Skill.Endurance, Skill.Heal, Skill.History, Skill.Insight, Skill.Perception, Skill.Religion };
            def.KeyAttributes = new List<AttributeType> { AttributeType.Wisdom, AttributeType.Constitution, AttributeType.Intelligence };
            AddDefinition(def);

            def = new ClassDefinition();
            def.Name = "Sorcerer";
            def.BaseHealth = 12;
            def.HealthPerLevel = 5;
            def.BaseHealingSurges = 6;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth };
            def.WeaponProficiencies = new List<WeaponCategory> { WeaponCategory.SimpleMelee, WeaponCategory.SimpleRanged };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(2, DefenseType.Will) };
            def.AutomaticSkills = new List<Skill> { Skill.Arcana };
            def.StartingSkills = 3;
            def.TrainableSkills = new List<Skill> { Skill.Athletics, Skill.Bluff, Skill.Diplomacy, Skill.Dungeoneering, Skill.Endurance, Skill.History, Skill.Insight, Skill.Intimidate, Skill.Nature };
            def.KeyAttributes = new List<AttributeType> { AttributeType.Charisma, AttributeType.Dexterity, AttributeType.Strength };
            AddDefinition(def);

            def = new ClassDefinition();
            def.Name = "Warden";
            def.BaseHealth = 17;
            def.HealthPerLevel = 7;
            def.BaseHealingSurges = 9;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth, ArmorType.Leather, ArmorType.Hide, ArmorType.LightShield, ArmorType.HeavyShield };
            def.WeaponProficiencies = new List<WeaponCategory> { WeaponCategory.SimpleMelee, WeaponCategory.MilitaryMelee, WeaponCategory.SimpleRanged };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(1, DefenseType.Fortitude), new DefenseBonus(1, DefenseType.Will) };
            def.AutomaticSkills = new List<Skill> { Skill.Nature };
            def.StartingSkills = 3;
            def.TrainableSkills = new List<Skill> { Skill.Athletics, Skill.Dungeoneering, Skill.Endurance, Skill.Heal, Skill.Intimidate, Skill.Perception };
            def.KeyAttributes = new List<AttributeType> { AttributeType.Strength, AttributeType.Constitution, AttributeType.Wisdom };
            AddDefinition(def);

            // FRPG
            def = new ClassDefinition();
            def.Name = "Swordmage";
            def.BaseHealth = 15;
            def.HealthPerLevel = 6;
            def.BaseHealingSurges = 8;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth, ArmorType.Leather };
            def.WeaponProficiencies = new List<WeaponCategory> { WeaponCategory.SimpleMelee, WeaponCategory.SimpleRanged };
            def.SpecificWeaponProficiencies = new List<WeaponType> { WeaponType.Longsword, WeaponType.Scimitar, WeaponType.ShortSword, WeaponType.Falchion, WeaponType.Glaive, WeaponType.Greatsword, WeaponType.Broadsword, WeaponType.Khopesh };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(2, DefenseType.Will) };
            def.AutomaticSkills = new List<Skill> { Skill.Arcana };
            def.StartingSkills = 3;
            def.TrainableSkills = new List<Skill> { Skill.Athletics, Skill.Diplomacy, Skill.Endurance, Skill.History, Skill.Insight, Skill.Intimidate };
            def.KeyAttributes = new List<AttributeType> { AttributeType.Intelligence, AttributeType.Strength, AttributeType.Constitution };
            AddDefinition(def);

            // PHB 3
            def = new ClassDefinition();
            def.Name = "Ardent";
            def.BaseHealth = 12;
            def.HealthPerLevel = 5;
            def.BaseHealingSurges = 7;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth, ArmorType.Leather, ArmorType.Hide, ArmorType.Chainmail };
            def.WeaponProficiencies = new List<WeaponCategory> { WeaponCategory.SimpleMelee, WeaponCategory.MilitaryMelee, WeaponCategory.SimpleRanged };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(1, DefenseType.Fortitude), new DefenseBonus(1, DefenseType.Will) };
            def.StartingSkills = 4;
            def.TrainableSkills = new List<Skill> { Skill.Arcana, Skill.Athletics, Skill.Bluff, Skill.Diplomacy, Skill.Endurance, Skill.Heal, Skill.Insight, Skill.Intimidate, Skill.Streetwise };
            def.KeyAttributes = new List<AttributeType> { AttributeType.Charisma, AttributeType.Constitution, AttributeType.Wisdom };
            AddDefinition(def);

            def = new ClassDefinition();
            def.Name = "Battlemind";
            def.BaseHealth = 15;
            def.HealthPerLevel = 6;
            def.BaseHealingSurges = 9;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth, ArmorType.Leather, ArmorType.Hide, ArmorType.Chainmail, ArmorType.Scale, ArmorType.LightShield, ArmorType.HeavyShield };
            def.WeaponProficiencies = new List<WeaponCategory> { WeaponCategory.SimpleMelee, WeaponCategory.MilitaryMelee, WeaponCategory.SimpleRanged };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(2, DefenseType.Will) };
            def.StartingSkills = 3;
            def.TrainableSkills = new List<Skill> { Skill.Arcana, Skill.Athletics, Skill.Bluff, Skill.Diplomacy, Skill.Endurance, Skill.Heal, Skill.Insight, Skill.Intimidate };
            def.KeyAttributes = new List<AttributeType> { AttributeType.Constitution, AttributeType.Wisdom, AttributeType.Charisma };
            AddDefinition(def);

            def = new ClassDefinition();
            def.Name = "Monk";
            def.BaseHealth = 12;
            def.HealthPerLevel = 5;
            def.BaseHealingSurges = 7;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth };
            def.SpecificWeaponProficiencies = new List<WeaponType> { WeaponType.Club, WeaponType.Dagger, WeaponType.Quarterstaff, WeaponType.Shuriken, WeaponType.Sling, WeaponType.Spear };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(1, DefenseType.Fortitude), new DefenseBonus(1, DefenseType.Reflex), new DefenseBonus(1, DefenseType.Will) };
            def.StartingSkills = 4;
            def.TrainableSkills = new List<Skill> { Skill.Acrobatics, Skill.Athletics, Skill.Diplomacy, Skill.Endurance, Skill.Heal, Skill.Insight, Skill.Perception, Skill.Religion, Skill.Stealth, Skill.Thievery };
            def.KeyAttributes = new List<AttributeType> { AttributeType.Dexterity, AttributeType.Strength, AttributeType.Wisdom };
            AddDefinition(def);

            def = new ClassDefinition();
            def.Name = "Psion";
            def.BaseHealth = 12;
            def.HealthPerLevel = 4;
            def.BaseHealingSurges = 6;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth };
            def.WeaponProficiencies = new List<WeaponCategory> { WeaponCategory.SimpleMelee, WeaponCategory.SimpleRanged };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(2, DefenseType.Will) };
            def.StartingSkills = 4;
            def.TrainableSkills = new List<Skill> { Skill.Arcana, Skill.Bluff, Skill.Diplomacy, Skill.Dungeoneering, Skill.History, Skill.Insight, Skill.Intimidate, Skill.Perception };
            def.KeyAttributes = new List<AttributeType> { AttributeType.Intelligence, AttributeType.Charisma, AttributeType.Wisdom };
            AddDefinition(def);

            def = new ClassDefinition();
            def.Name = "Runepriest";
            def.BaseHealth = 12;
            def.HealthPerLevel = 5;
            def.BaseHealingSurges = 7;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth, ArmorType.Leather, ArmorType.Hide, ArmorType.Chainmail, ArmorType.Scale, ArmorType.LightShield };
            def.WeaponProficiencies = new List<WeaponCategory> { WeaponCategory.SimpleMelee, WeaponCategory.SimpleRanged };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(2, DefenseType.Will) };
            def.AutomaticSkills = new List<Skill> { Skill.Religion };
            def.StartingSkills = 3;
            def.TrainableSkills = new List<Skill> { Skill.Arcana, Skill.Athletics, Skill.Endurance, Skill.Heal, Skill.History, Skill.Insight, Skill.Thievery };
            def.KeyAttributes = new List<AttributeType> { AttributeType.Strength, AttributeType.Constitution, AttributeType.Wisdom };
            AddDefinition(def);

            def = new ClassDefinition();
            def.Name = "Seeker";
            def.BaseHealth = 12;
            def.HealthPerLevel = 5;
            def.BaseHealingSurges = 7;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth, ArmorType.Leather };
            def.WeaponProficiencies = new List<WeaponCategory> { WeaponCategory.SimpleMelee, WeaponCategory.SimpleRanged, WeaponCategory.MilitaryRanged };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(1, DefenseType.Reflex), new DefenseBonus(1, DefenseType.Will) };
            def.AutomaticSkills = new List<Skill> { Skill.Nature };
            def.StartingSkills = 3;
            def.TrainableSkills = new List<Skill> { Skill.Acrobatics, Skill.Athletics, Skill.Endurance, Skill.Heal, Skill.Insight, Skill.Intimidate, Skill.Perception, Skill.Stealth };
            def.KeyAttributes = new List<AttributeType> { AttributeType.Wisdom, AttributeType.Strength, AttributeType.Dexterity };
            AddDefinition(def);

            // EPG
            def = new ClassDefinition();
            def.Name = "Artificer";
            def.BaseHealth = 12;
            def.HealthPerLevel = 5;
            def.BaseHealingSurges = 6;
            def.ArmorProficiencies = new List<ArmorType> { ArmorType.Cloth, ArmorType.Leather };
            def.WeaponProficiencies = new List<WeaponCategory> { WeaponCategory.SimpleMelee, WeaponCategory.SimpleRanged };
            def.DefenseBonuses = new List<DefenseBonus> { new DefenseBonus(1, DefenseType.Fortitude), new DefenseBonus(1, DefenseType.Will) };
            def.AutomaticSkills = new List<Skill> { Skill.Arcana };
            def.StartingSkills = 4;
            def.TrainableSkills = new List<Skill> { Skill.Diplomacy, Skill.Dungeoneering, Skill.Heal, Skill.History, Skill.Perception, Skill.Thievery };
            def.KeyAttributes = new List<AttributeType> { AttributeType.Intelligence, AttributeType.Constitution, AttributeType.Wisdom };
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
