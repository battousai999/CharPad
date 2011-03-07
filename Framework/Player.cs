using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace CharPad.Framework
{
    public class Player : INotifyPropertyChanged
    {
        private string characterName;
        private string playerName;
        private PlayerClass _class;
        private PlayerRace race;
        private bool isMale;
        private string deity;
        private int level;
        private int str;
        private int con;
        private int dex;
        private int _int;
        private int wis;
        private int cha;
        private HitPointsValue hitPoints;
        private SurgeValue surgeValue;
        private SurgesPerDayValue surgesPerDay;
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
        private InitiativeValue initiative;
        private DefenseValue acDefense;
        private DefenseValue fortDefense;
        private DefenseValue reflexDefense;
        private DefenseValue willDefense;
        private SpeedValue speed;
        private ObservableCollectionEx<FeatureValue> raceFeatures;
        private ObservableCollectionEx<FeatureValue> classFeatures;
        private ObservableCollectionEx<FeatureValue> paragonFeatures;
        private ObservableCollectionEx<FeatureValue> destinyFeatures;
        private ObservableCollectionEx<FeatureValue> feats;
        private ObservableCollectionEx<ResistanceValue> resistances;
        private Armor armor;
        private Shield shield;
        private Weapon weapon;
        private Weapon weaponOffhand;
        private Weapon rangedWeapon;
        private Weapon implement;
        private ObservableCollectionEx<IInventoryItem> inventory;
        private WeaponBonusList weaponBonuses;
        private WeaponSpecValue weaponSpec;
        private WeaponSpecValue weaponOffhandSpec;
        private WeaponSpecValue rangedWeaponSpec;
        private WeaponSpecValue implementSpec;

        public string CharacterName { get { return characterName; } set { characterName = value; Notify("CharacterName"); } }
        public string PlayerName { get { return playerName; } set { playerName = value; Notify("PlayerName"); } }
        public PlayerClass Class { get { return _class; } set { _class = value; Notify("Class"); } }
        public PlayerRace Race { get { return race; } set { race = value; Notify("Race"); } }
        public bool IsMale { get { return isMale; } set { isMale = value; Notify("IsMale"); } }
        public string Deity { get { return deity; } set { deity = value; Notify("Deity"); } }
        public int Level { get { return level; } set { level = value; Notify("Level"); Notify("LevelBonus"); } }
        public int Str { get { return str; } set { str = value; Notify("Str"); Notify("StrModifier"); } }
        public int Con { get { return con; } set { con = value; Notify("Con"); Notify("ConModifier"); } }
        public int Dex { get { return dex; } set { dex = value; Notify("Dex"); Notify("DexModifier"); } }
        public int Int { get { return _int; } set { _int = value; Notify("Int"); Notify("IntModifier"); } }
        public int Wis { get { return wis; } set { wis = value; Notify("Wis"); Notify("WisModifier"); } }
        public int Cha { get { return cha; } set { cha = value; Notify("Cha"); Notify("ChaModifier"); } }
        public HitPointsValue HitPoints { get { return hitPoints; } }
        public SurgeValue SurgeValue { get { return surgeValue; } }
        public SurgesPerDayValue SurgesPerDay { get { return surgesPerDay; } }
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
        public InitiativeValue Initiative { get { return initiative; } }
        public DefenseValue AcDefense { get { return acDefense; } }
        public DefenseValue FortDefense { get { return fortDefense; } }
        public DefenseValue ReflexDefense { get { return reflexDefense; } }
        public DefenseValue WillDefense { get { return willDefense; } }
        public SpeedValue Speed { get { return speed; } }
        public ObservableCollectionEx<FeatureValue> RaceFeatures { get { return raceFeatures; } }
        public ObservableCollectionEx<FeatureValue> ClassFeatures { get { return classFeatures; } }
        public ObservableCollectionEx<FeatureValue> ParagonFeatures { get { return paragonFeatures; } }
        public ObservableCollectionEx<FeatureValue> DestinyFeatures { get { return destinyFeatures; } }
        public ObservableCollectionEx<FeatureValue> Feats { get { return feats; } }
        public ObservableCollectionEx<ResistanceValue> Resistances { get { return resistances; } }
        public ObservableCollectionEx<IInventoryItem> Inventory { get { return inventory; } }
        public WeaponBonusList WeaponBonuses { get { return weaponBonuses; } }
        public WeaponSpecValue WeaponSpec { get { return weaponSpec; } }
        public WeaponSpecValue WeaponOffhandSpec { get { return weaponOffhandSpec; } }
        public WeaponSpecValue RangedWeaponSpec { get { return rangedWeaponSpec; } }
        public WeaponSpecValue ImplementSpec { get { return implementSpec; } }

        public Armor Armor 
        { 
            get { return armor; }             
            set 
            {
                if ((value != null) && !inventory.Contains(value))
                    throw new InvalidOperationException("Cannot equipt armor that is not in your inventory.");

                if (armor != null)
                    armor.PropertyChanged -= new PropertyChangedEventHandler(armor_PropertyChanged);

                armor = value;

                if (armor != null)
                    armor.PropertyChanged += new PropertyChangedEventHandler(armor_PropertyChanged);
                
                Notify("Armor");
                Notify("AcDefense");
                Notify("Speed");
                Notify("Acrobatics");
                Notify("Athletics");
                Notify("Endurance");
                Notify("Stealth");
                Notify("Thievery");
            } 
        }

        private void armor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("Armor");
            Notify("AcDefense");
            Notify("Speed");
            Notify("Acrobatics");
            Notify("Athletics");
            Notify("Endurance");
            Notify("Stealth");
            Notify("Thievery");
        }

        public Shield Shield
        {
            get { return shield; }
            set
            {
                if ((value != null) && !inventory.Contains(value))
                    throw new InvalidOperationException("Cannot equipt a shield that is not in your inventory.");

                if ((value != null) && (WeaponOffhand != null))
                    WeaponOffhand = null;

                if (shield != null)
                    shield.PropertyChanged -= new PropertyChangedEventHandler(shield_PropertyChanged);

                shield = value;

                if (shield != null)
                    shield.PropertyChanged += new PropertyChangedEventHandler(shield_PropertyChanged);

                Notify("Shield");
                Notify("AcDefense");
                Notify("ReflexDefense");
                Notify("Speed");
                Notify("Acrobatics");
                Notify("Athletics");
                Notify("Endurance");
                Notify("Stealth");
                Notify("Thievery");
            }
        }

        private void shield_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("Shield");
            Notify("AcDefense");
            Notify("ReflexDefense");
            Notify("Speed");
            Notify("Acrobatics");
            Notify("Athletics");
            Notify("Endurance");
            Notify("Stealth");
            Notify("Thievery");
        }

        public Weapon Weapon
        {
            get { return weapon; }
            set
            {
                if ((value != null) && !inventory.Contains(value))
                    throw new InvalidOperationException("Cannot equipt a weapon that is not in your inventory.");

                if (weapon != null)
                    weapon.PropertyChanged -= new PropertyChangedEventHandler(weapon_PropertyChanged);

                weapon = value;

                if (weapon != null)
                    weapon.PropertyChanged += new PropertyChangedEventHandler(weapon_PropertyChanged);

                WeaponBonusValue bonus = WeaponBonuses[weapon];

                if (bonus == null)
                    WeaponBonuses.Add(weapon, new WeaponBonusValue());

                Notify("Weapon");
                Notify("WeaponSpec");
            }
        }

        private void weapon_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("Weapon");
            Notify("WeaponSpec");
        }

        public Weapon WeaponOffhand
        {
            get { return weaponOffhand; }
            set
            {
                if ((value != null) && !inventory.Contains(value))
                    throw new InvalidOperationException("Cannot equipt an off-hand weapon that is not in your inventory.");

                if ((value != null) && (Shield != null))
                    Shield = null;

                if (weaponOffhand != null)
                    weaponOffhand.PropertyChanged -= new PropertyChangedEventHandler(weaponOffhand_PropertyChanged);

                weaponOffhand = value;

                if (weaponOffhand != null)
                    weaponOffhand.PropertyChanged += new PropertyChangedEventHandler(weaponOffhand_PropertyChanged);

                if (WeaponBonuses[weaponOffhand] == null)
                    weaponBonuses.Add(weaponOffhand, new WeaponBonusValue());

                Notify("WeaponOffhand");
                Notify("WeaponOffhandSpec");
            }
        }

        private void weaponOffhand_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("WeaponOffhand");
            Notify("WeaponOffhandSpec");
        }

        public Weapon RangedWeapon
        {
            get { return rangedWeapon; }
            set
            {
                if ((value != null) && !inventory.Contains(value))
                    throw new InvalidOperationException("Cannot specify a ranged weapon that is not in your inventory.");

                if (rangedWeapon != null)
                    rangedWeapon.PropertyChanged -= new PropertyChangedEventHandler(rangedWeapon_PropertyChanged);

                rangedWeapon = value;

                if (rangedWeapon != null)
                    rangedWeapon.PropertyChanged += new PropertyChangedEventHandler(rangedWeapon_PropertyChanged);

                if (WeaponBonuses[rangedWeapon] == null)
                    weaponBonuses.Add(rangedWeapon, new WeaponBonusValue());

                Notify("RangedWeapon");
                Notify("RangedWeaponSpec");
            }
        }

        void rangedWeapon_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("RangedWeapon");
            Notify("RangedWeaponSpec");
        }

        public Weapon Implement
        {
            get { return implement; }
            set
            {
                if ((value != null) && !inventory.Contains(value))
                    throw new InvalidOperationException("Cannot equipt an implement that is not in your inventory.");

                if (implement != null)
                    implement.PropertyChanged -= new PropertyChangedEventHandler(implement_PropertyChanged);

                implement = value;

                if (implement != null)
                    implement.PropertyChanged += new PropertyChangedEventHandler(implement_PropertyChanged);

                WeaponBonusValue bonus = WeaponBonuses[implement];

                if (bonus == null)
                    WeaponBonuses.Add(implement, new WeaponBonusValue());

                Notify("Implement");
                Notify("ImplementSpec");
            }
        }

        void implement_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("Implement");
            Notify("ImplementSpec");
        }

        public List<IInventoryItem> WieldedItems
        {
            get
            {
                List<IInventoryItem> list = new List<IInventoryItem>();

                if (weapon != null)
                    list.Add(weapon);

                if (weaponOffhand != null)
                    list.Add(weaponOffhand);

                if (armor != null)
                    list.Add(armor);

                if (shield != null)
                    list.Add(shield);

                return list;
            }
        }

        public Player()
        {
            this.characterName = "";
            this.playerName = "";
            this.deity = "";
            this.isMale = true;
            this.level = 1;
            this.str = 10;
            this.con = 10;
            this.dex = 10;
            this._int = 10;
            this.wis = 10;
            this.cha = 10;
            this.hitPoints = new HitPointsValue(this);
            this.surgeValue = new SurgeValue(this);
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
            this.initiative = new InitiativeValue(this);
            this.acDefense = new DefenseValue(this, DefenseType.AC);
            this.fortDefense = new DefenseValue(this, DefenseType.Fortitude);
            this.reflexDefense = new DefenseValue(this, DefenseType.Reflex);
            this.willDefense = new DefenseValue(this, DefenseType.Will);
            this.speed = new SpeedValue(this);
            this.raceFeatures = new ObservableCollectionEx<FeatureValue>();
            this.classFeatures = new ObservableCollectionEx<FeatureValue>();
            this.paragonFeatures = new ObservableCollectionEx<FeatureValue>();
            this.destinyFeatures = new ObservableCollectionEx<FeatureValue>();
            this.feats = new ObservableCollectionEx<FeatureValue>();
            this.resistances = new ObservableCollectionEx<ResistanceValue>();
            this.inventory = new ObservableCollectionEx<IInventoryItem>();
            this.weaponBonuses = new WeaponBonusList();
            this.weaponSpec = new WeaponSpecValue(this, WeaponSlot.MainWeapon);
            this.weaponOffhandSpec = new WeaponSpecValue(this, WeaponSlot.OffhandWeapon);
            this.rangedWeaponSpec = new WeaponSpecValue(this, WeaponSlot.RangedWeapon);
            this.implementSpec = new WeaponSpecValue(this, WeaponSlot.Implement);
            this.surgesPerDay = new SurgesPerDayValue(this);

            hitPoints.PropertyChanged += new PropertyChangedEventHandler(hitPoints_PropertyChanged);
            insight.PropertyChanged += new PropertyChangedEventHandler(insight_PropertyChanged);
            perception.PropertyChanged += new PropertyChangedEventHandler(perception_PropertyChanged);
            raceFeatures.ContainedElementChanged += new PropertyChangedEventHandler(raceFeatures_ContainedElementChanged);
            raceFeatures.CollectionChanged += new NotifyCollectionChangedEventHandler(raceFeatures_CollectionChanged);
            classFeatures.ContainedElementChanged += new PropertyChangedEventHandler(classFeatures_ContainedElementChanged);
            classFeatures.CollectionChanged += new NotifyCollectionChangedEventHandler(classFeatures_CollectionChanged);
            paragonFeatures.ContainedElementChanged += new PropertyChangedEventHandler(paragonFeatures_ContainedElementChanged);
            paragonFeatures.CollectionChanged += new NotifyCollectionChangedEventHandler(paragonFeatures_CollectionChanged);
            destinyFeatures.ContainedElementChanged += new PropertyChangedEventHandler(destinyFeatures_ContainedElementChanged);
            destinyFeatures.CollectionChanged += new NotifyCollectionChangedEventHandler(destinyFeatures_CollectionChanged);
            feats.ContainedElementChanged += new PropertyChangedEventHandler(feats_ContainedElementChanged);
            feats.CollectionChanged += new NotifyCollectionChangedEventHandler(feats_CollectionChanged);
            resistances.ContainedElementChanged += new PropertyChangedEventHandler(resistances_ContainedElementChanged);
            resistances.CollectionChanged += new NotifyCollectionChangedEventHandler(resistances_CollectionChanged);
            inventory.ContainedElementChanged += new PropertyChangedEventHandler(inventory_ContainedElementChanged);
            inventory.CollectionChanged += new NotifyCollectionChangedEventHandler(inventory_CollectionChanged);
            weaponBonuses.ContainedElementChanged += new PropertyChangedEventHandler(weaponBonuses_ContainedElementChanged);
            weaponBonuses.CollectionChanged += new NotifyCollectionChangedEventHandler(weaponBonuses_CollectionChanged);
            weaponSpec.PropertyChanged += new PropertyChangedEventHandler(weaponSpec_PropertyChanged);
            weaponOffhandSpec.PropertyChanged += new PropertyChangedEventHandler(weaponOffhandSpec_PropertyChanged);
            rangedWeaponSpec.PropertyChanged += new PropertyChangedEventHandler(rangedWeaponSpec_PropertyChanged);
            implementSpec.PropertyChanged += new PropertyChangedEventHandler(implementSpec_PropertyChanged);
            surgesPerDay.PropertyChanged += new PropertyChangedEventHandler(surgesPerDay_PropertyChanged);
        }

        void implementSpec_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("ImplementSpec");
        }

        void rangedWeaponSpec_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("RangedWeaponSpec");
        }

        void surgesPerDay_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("SurgesPerDay");
        }

        void weaponOffhandSpec_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("WeaponOffhandSpec");
        }

        void weaponSpec_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("WeaponSpec");
        }

        void weaponBonuses_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Notify("WeaponBonuses");
        }

        void weaponBonuses_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("WeaponBonuses");
        }

        void inventory_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if ((weapon != null) && !inventory.Contains(weapon))
                Weapon = null;

            if ((weaponOffhand != null) && !inventory.Contains(weaponOffhand))
                WeaponOffhand = null;

            if ((armor != null) && !inventory.Contains(armor))
                Armor = null;

            if ((shield != null) && !inventory.Contains(shield))
                Shield = null;

            Notify("Inventory");
        }

        void resistances_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Notify("Resistances");
        }

        void feats_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Notify("Feats");
        }

        void destinyFeatures_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Notify("DestinyFeatures");
        }

        void paragonFeatures_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Notify("ParagonFeatures");
        }

        void classFeatures_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Notify("ClassFeatures");
        }

        void raceFeatures_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Notify("RaceFeatures");
        }

        void inventory_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("Inventory");
        }

        void resistances_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("Resistances");
        }

        void feats_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("Feats");
        }

        void destinyFeatures_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("DestinyFeatures");
        }

        void paragonFeatures_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("ParagonFeatures");
        }

        void classFeatures_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("ClassFeatures");
        }

        void raceFeatures_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("RaceFeatures");
        }

        void perception_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("PassivePerception");
        }

        void insight_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("PassiveInsight");
        }

        void hitPoints_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("BloodiedValue");
        }

        public static int GetAttributeModifier(int attributeValue)
        {
            return (attributeValue >= 10 ? (attributeValue - 10) / 2 : (attributeValue - 11) / 2);
        }

        public int GetAttributeModifier(AttributeType attribute)
        {
            int attributeValue;

            switch (attribute)
            {
                case AttributeType.Strength:     
                    attributeValue = str; 
                    break;
                case AttributeType.Constitution: 
                    attributeValue = con; 
                    break;
                case AttributeType.Dexterity: 
                    attributeValue = dex; 
                    break;
                case AttributeType.Intelligence:
                    attributeValue = _int;
                    break;
                case AttributeType.Wisdom:
                    attributeValue = wis;
                    break;
                case AttributeType.Charisma:
                    attributeValue = cha;
                    break;
                default:
                    throw new InvalidOperationException("Unexpected attribute value: " + Enum.Format(typeof(AttributeType), attribute, "G"));
            }

            return GetAttributeModifier(attributeValue);
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

        public int BloodiedValue { get { return (hitPoints.Value / 2); } }

        public int PassiveInsight { get { return insight.Value + 10; } }
        public int PassivePerception { get { return perception.Value + 10; } }

        public DefenseValue GetDefenseValue(DefenseType defenseType)
        {
            switch (defenseType)
            {
                case DefenseType.AC:
                    return AcDefense;
                case DefenseType.Fortitude:
                    return FortDefense;
                case DefenseType.Reflex:
                    return ReflexDefense;
                case DefenseType.Will:
                    return WillDefense;
                default:
                    throw new InvalidOperationException("Unexpected defense type value: " + Enum.Format(typeof(DefenseType), defenseType, "G"));
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
