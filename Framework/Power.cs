using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace CharPad.Framework
{
    public class Power : INotifyPropertyChanged
    {
        private Player player;
        private Weapon playerWeapon;
        private string name;
        private PowerType powerType;
        private PowerActionType actionType;
        private int level;
        private string description; // displays next to name
        private string notes;       // displays in tooltip w/ image
        private PowerAttackType attackType;
        private WeaponSlot attackWeapon;
        private AttributeType attackAttribute;
        private DefenseType defenseType;
        private BasicAdjustmentList attackModifiers;
        private Dice damage;
        private int weaponDamageMultiplier;
        private string damageType;
        private AttributeType? bonusDamageAttribute;
        private BasicAdjustmentList damageModifiers;
        private Image picture;

        public Power(Player player)
        {
            this.player = player;
            this.name = "";
            this.powerType = PowerType.AtWill;
            this.actionType = PowerActionType.Standard;
            this.level = 1;
            this.description = "";
            this.notes = "";
            this.attackType = PowerAttackType.Weapon;
            this.attackWeapon = WeaponSlot.MainWeapon;
            this.attackAttribute = AttributeType.Strength;
            this.defenseType = DefenseType.AC;
            this.weaponDamageMultiplier = 1;
            this.damageType = "";
            this.bonusDamageAttribute = null;
            this.attackModifiers = new BasicAdjustmentList();
            this.damageModifiers = new BasicAdjustmentList();

            player.PropertyChanged += new PropertyChangedEventHandler(player_PropertyChanged);

            playerWeapon = Weapon;

            if (playerWeapon != null)
                playerWeapon.PropertyChanged += new PropertyChangedEventHandler(playerWeapon_PropertyChanged);

            attackModifiers.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(attackModifiers_CollectionChanged);
            attackModifiers.ContainedElementChanged += new PropertyChangedEventHandler(attackModifiers_ContainedElementChanged);
            damageModifiers.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(damageModifiers_CollectionChanged);
            damageModifiers.ContainedElementChanged += new PropertyChangedEventHandler(damageModifiers_ContainedElementChanged);
        }

        void damageModifiers_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("DamageModifiers", "TotalDamageBonus");
        }

        void damageModifiers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Notify("DamageModifiers", "TotalDamageBonus");
        }

        void attackModifiers_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("AttackModifiers", "TotalToHitBonus");
        }

        void attackModifiers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Notify("AttackModifiers", "TotalToHitBonus");
        }

        void playerWeapon_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("Weapon", "WeaponProficiencyBonus", "WeaponEnhancementBonus", "TotalToHitBonus", "DamageText", "FullDamageText", "WeaponToHitBonus", "WeaponDamageBonus", "TotalDamageBonus");
        }

        void player_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "LevelBonus") == 0)
                Notify("LevelBonus", "TotalToHitBonus");
            else if ((new string[] { "StrModifier", "ConModifier", "DexModifier", "IntModifier", "WisModifier", "ChaModifier" }).Contains(e.PropertyName, StringComparer.CurrentCultureIgnoreCase))
                Notify("AttributeBonus", "AttributeDamageBonus", "TotalToHitBonus", "TotalDamageBonus", "FullDamageText");
            else if ((new string[] { "Weapon", "WeaponOffhand", "RangedWeapon", "Implement" }).Contains(e.PropertyName, StringComparer.CurrentCultureIgnoreCase))
            {
                Notify("Weapon", "WeaponProficiencyBonus", "WeaponEnhancementBonus", "TotalToHitBonus", "DamageText", "FullDamageText", "WeaponToHitBonus", "WeaponDamageBonus", "TotalDamageBonus");

                if (playerWeapon != Weapon)
                {
                    if (playerWeapon != null)
                        playerWeapon.PropertyChanged -= new PropertyChangedEventHandler(playerWeapon_PropertyChanged);

                    playerWeapon = Weapon;

                    if (playerWeapon != null)
                        playerWeapon.PropertyChanged += new PropertyChangedEventHandler(playerWeapon_PropertyChanged);
                }
            }
        }

        public Player Player { get { return player; } }
        public string Name { get { return name; } set { name = value; Notify("Name"); } }
        public PowerType PowerType { get { return powerType; } set { powerType = value; Notify("PowerType"); } }
        public PowerActionType ActionType { get { return actionType; } set { actionType = value; Notify("ActionType", "AttributeBonus"); } }
        public int Level { get { return level; } set { level = value; Notify("Level"); } }
        public string Description { get { return description; } set { description = value; Notify("Description"); } }
        public string Notes { get { return notes; } set { notes = value; Notify("Notes"); } }
        public PowerAttackType AttackType { get { return attackType; } set { attackType = value; Notify("AttackType"); } }
        public WeaponSlot AttackWeapon { get { return attackWeapon; } set { attackWeapon = value; Notify("AttackWeapon", "Weapon", "WeaponProficiencyBonus", "WeaponEnhancementBonus" ); } }
        public AttributeType AttackAttribute { get { return attackAttribute; } set { attackAttribute = value; Notify("AttackAttribute"); } }
        public DefenseType DefenseType { get { return defenseType; } set { defenseType = value; Notify("DefenseType"); } }
        public BasicAdjustmentList AttackModifiers { get { return attackModifiers; } }
        public Dice Damage { get { return damage; } set { damage = value; Notify("Damage"); } }
        public int WeaponDamamgeMultiplier { get { return weaponDamageMultiplier; } set { weaponDamageMultiplier = value; Notify("WeaponDamageMultiplier"); } }
        public string DamageType { get { return damageType; } set { damageType = value; Notify("DamageType"); } }
        public AttributeType? BonusDamageAttribute { get { return bonusDamageAttribute; } set { bonusDamageAttribute = value; Notify("BonusDamageAttribute"); } }
        public BasicAdjustmentList DamageModifiers { get { return damageModifiers; } }
        public Image Picture { get { return picture; } set { picture = value; Notify("Picture"); } }
        
        public Weapon Weapon
        {
            get
            {
                switch (attackWeapon)
                {
                    case WeaponSlot.MainWeapon:
                        return player.Weapon;
                    case WeaponSlot.OffhandWeapon:
                        return player.WeaponOffhand;
                    case WeaponSlot.RangedWeapon:
                        return player.RangedWeapon;
                    case WeaponSlot.Implement:
                        return (player.Implement != null ? player.Implement : ((player.Weapon != null) && player.Weapon.IsImplement ? player.Weapon : null));
                    default:
                        throw new InvalidOperationException("Unexpected slot value: " + Enum.Format(typeof(WeaponSlot), attackWeapon, "G"));
                }
            }
        }

        public int LevelBonus { get { return player.LevelBonus; } }
        public int AttributeBonus { get { return player.GetAttributeModifier(attackAttribute); } }
        public int WeaponProficiencyBonus { get { return ((Weapon == null) || (attackWeapon == WeaponSlot.Implement) ? 0 : Weapon.ProficiencyBonus); } }
        public int WeaponEnhancementBonus { get { return (Weapon == null ? 0 : Weapon.EnhancementBonus); } }

        public int WeaponToHitBonus
        {
            get
            {
                WeaponSpecValue spec = (Weapon == null ? null : player.GetWeaponSpec(AttackWeapon));
                return (spec == null ? 0 : (spec.TotalToHitAdjustment + spec.WeaponSpecificToHitAdjustment));
            }
        }

        public int TotalAttackAdjustment { get { return attackModifiers.TotalAdjustment; } }

        public int TotalToHitBonus
        {
            get
            {
                return LevelBonus + AttributeBonus + WeaponProficiencyBonus + WeaponEnhancementBonus + WeaponToHitBonus + TotalAttackAdjustment;
            }
        }

        public int AttributeDamageBonus { get { return (bonusDamageAttribute == null ? 0 : player.GetAttributeModifier(bonusDamageAttribute.Value)); } }
        public int TotalDamageAdjustment { get { return damageModifiers.TotalAdjustment; } }

        public int WeaponDamageBonus
        {
            get
            {
                WeaponSpecValue spec = (Weapon == null ? null : player.GetWeaponSpec(AttackWeapon);
                return (spec == null ? 0 : (spec.TotalDamageAdjustment + spec.WeaponSpecificDamageAdjustment));
            }
        }

        public int TotalDamageBonus
        {
            get { return AttributeDamageBonus + WeaponEnhancementBonus + WeaponDamageBonus + TotalDamageAdjustment; }
        }

        public string DamageText
        {
            get
            {
                if (attackWeapon == WeaponSlot.Implement)
                    return (damage == null ? "(n/a)" : damage.DisplayString);
                else
                    return (Weapon == null ? Dice.Get(2) : Weapon.Damage).GetDice(weaponDamageMultiplier).DisplayString;
            }
        }

        public string FullDamageText
        {
            get
            {
                if (TotalDamageBonus == 0)
                    return DamageText;
                else
                    return DamageText + (TotalDamageBonus < 0 ? TotalDamageBonus.ToString() : ("+" + TotalDamageBonus.ToString()));
            }
        }

        public string FullDescription
        {
            get
            {
                if (AttackType == PowerAttackType.None)
                    return Description;
                else
                {
                    Weapon weapon = Weapon;

                    if ((AttackType == PowerAttackType.Implement) && (AttackWeapon != WeaponSlot.Implement) && !weapon.IsImplement)
                        return "Invalid power settings";

                    if ((AttackType == PowerAttackType.Weapon) && ((AttackWeapon == WeaponSlot.Implement) || (weapon == null)))
                        return "Invalid power settings.";

                    string damageString = (String.IsNullOrWhiteSpace(damageType) ? "damage" : (damageType + " damage"));

                    string tempString = String.Format("{0} vs {1}, {2} {3}",
                        (TotalToHitBonus >= 0 ? "+" + TotalToHitBonus.ToString() : TotalToHitBonus.ToString()),
                        Enum.Format(typeof(DefenseType), defenseType, "G"),
                        FullDamageText,
                        damageString);

                    return (String.IsNullOrWhiteSpace(Description) ? tempString : String.Format("{0} ({1})", tempString, Description));
                }
            }
        }

        #region INotifyPropertyChanged Members

        private void Notify(params string[] propertyNames)
        {
            if (PropertyChanged != null)
            {
                // This is just easier for now...
                PropertyChanged(this, new PropertyChangedEventArgs("FullDescription"));

                foreach (string propertyName in propertyNames)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
