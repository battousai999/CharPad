using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class WeaponSpecValue : INotifyPropertyChanged
    {
        private Player player;
        private Weapon playerWeapon;
        private WeaponBonusValue weaponBonus;
        private BasicAdjustmentList toHitAdjustments;
        private BasicAdjustmentList damageAdjustments;
        private WeaponSlot slot;

        private bool isMainWeapon { get { return (slot == WeaponSlot.MainWeapon); } }
        private bool asRanged { get { return (slot == WeaponSlot.RangedWeapon); } }
        private bool asImplementSlot { get { return (slot == WeaponSlot.Implement); } }

        public WeaponSpecValue(Player player, WeaponSlot slot)
        {
            this.player = player;
            this.slot = slot;
            this.toHitAdjustments = new BasicAdjustmentList();
            this.damageAdjustments = new BasicAdjustmentList();

            player.PropertyChanged += new PropertyChangedEventHandler(player_PropertyChanged);

            playerWeapon = Weapon;

            if (Weapon != null)
                Weapon.PropertyChanged += new PropertyChangedEventHandler(Weapon_PropertyChanged);

            if (Weapon != null)
            {
                weaponBonus = player.WeaponBonuses[Weapon];

                if (weaponBonus != null)
                    weaponBonus.PropertyChanged += new PropertyChangedEventHandler(bonus_PropertyChanged);
            }
            
            toHitAdjustments.ContainedElementChanged += new PropertyChangedEventHandler(toHitAdjustments_ContainedElementChanged);
            toHitAdjustments.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(toHitAdjustments_CollectionChanged);
            damageAdjustments.ContainedElementChanged += new PropertyChangedEventHandler(damageAdjustments_ContainedElementChanged);
            damageAdjustments.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(damageAdjustments_CollectionChanged);
        }

        public Player Player { get { return player; } }
        public WeaponSlot Slot { get { return slot; } }
        //public bool IsMainWeapon { get { return isMainWeapon; } }
        //public bool AsRanged { get { return asRanged; } }
        public BasicAdjustmentList ToHitAdjustments { get { return toHitAdjustments; } }
        public BasicAdjustmentList DamageAdjustments { get { return damageAdjustments; } }

        public Weapon Weapon
        {
            get
            {
                switch (slot)
                {
                    case WeaponSlot.MainWeapon:
                        return player.Weapon;
                    case WeaponSlot.OffhandWeapon:
                        return player.WeaponOffhand;
                    case WeaponSlot.RangedWeapon:
                        return player.RangedWeapon;
                    case WeaponSlot.Implement:
                        return player.Implement;
                    default:
                        throw new InvalidOperationException("Unexpected slot value: " + Enum.Format(typeof(WeaponSlot), slot, "G"));
                }
            }
        }

        public int TotalToHitBonus
        {
            get
            {
                if (Weapon == null)
                    return 0;

                return AttributeBonus + EnhancementBonus + ProficiencyBonus + LevelBonus + TotalToHitAdjustment + WeaponSpecificToHitAdjustment;
            }
        }

        public int TotalThrownToHitBonus
        {
            get
            {
                if ((Weapon == null) || Weapon.IsRanged || !Weapon.IsThrown)
                    return 0;

                return (Weapon.HasWeaponProperty(WeaponProperties.HeavyThrown) ? TotalToHitBonus : (TotalToHitBonus - AttributeBonus + player.DexModifier));
            }
        }

        public int TotalDamageBonus
        {
            get
            {
                if (Weapon == null)
                    return 0;

                return AttributeBonus + EnhancementBonus + TotalDamageAdjustment + WeaponSpecificDamageAdjustment;
            }
        }

        public int TotalThrownDamageBonus
        {
            get
            {
                if ((Weapon == null) || Weapon.IsRanged || !Weapon.IsThrown)
                    return 0;

                return (Weapon.HasWeaponProperty(WeaponProperties.HeavyThrown) ? TotalDamageBonus : (TotalDamageBonus - AttributeBonus + player.DexModifier));
            }
        }

        public string ToHitSpec
        {
            get
            {
                if (Weapon == null)
                    return "";

                return (TotalToHitBonus < 0 ? TotalToHitBonus.ToString() : "+" + TotalToHitBonus.ToString());
            }
        }

        public string ThrownToHitSpec
        {
            get
            {
                if ((Weapon == null) || Weapon.IsRanged || !Weapon.IsThrown)
                    return "";

                int bonus = (Weapon.HasWeaponProperty(WeaponProperties.HeavyThrown) ? TotalToHitBonus : (TotalToHitBonus - AttributeBonus + player.DexModifier));

                return (bonus < 0 ? bonus.ToString() : "+" + bonus.ToString());
            }
        }

        public string DamageSpec
        {
            get
            {
                if (Weapon == null)
                    return "";

                return String.Format("{0} {1} {2}",
                    Weapon.Damage.DisplayString,
                    (TotalDamageBonus < 0 ? "-" : "+"),
                    Math.Abs(TotalDamageBonus).ToString());
            }
        }

        public string ThrownDamageSpec
        {
            get
            {
                if ((Weapon == null) || Weapon.IsRanged || !Weapon.IsThrown)
                    return "";

                int bonus = (Weapon.HasWeaponProperty(WeaponProperties.HeavyThrown) ? TotalDamageBonus : (TotalDamageBonus - AttributeBonus + player.DexModifier));

                return (bonus < 0 ? bonus.ToString() : "+" + bonus.ToString());
            }
        }

        public int AttributeBonus
        {
            get
            {
                if (Weapon == null)
                    return 0;

                if (slot == WeaponSlot.Implement)
                    return 0;
                else if (Weapon.IsRanged)
                    return player.DexModifier;
                else
                    return player.StrModifier;
            }
        }

        public int ProficiencyBonus
        {
            get { return ((Weapon == null) || (slot == WeaponSlot.Implement) ? 0 : Weapon.ProficiencyBonus); }
        }

        public int EnhancementBonus
        {
            get { return (Weapon == null ? 0 : Weapon.EnhancementBonus); }
        }

        public int TotalToHitAdjustment
        {
            get { return toHitAdjustments.TotalAdjustment; }
        }

        public int TotalDamageAdjustment
        {
            get { return damageAdjustments.TotalAdjustment; }
        }

        public int WeaponSpecificToHitAdjustment
        {
            get
            {
                if (Weapon == null)
                    return 0;

                WeaponBonusValue weaponBonus = player.WeaponBonuses[Weapon];

                return (weaponBonus == null ? 0 : weaponBonus.TotalToHitAdjustment);
            }
        }

        public int WeaponSpecificDamageAdjustment
        {
            get
            {
                if (Weapon == null)
                    return 0;

                WeaponBonusValue weaponBonus = player.WeaponBonuses[Weapon];

                return (weaponBonus == null ? 0 : weaponBonus.TotalDamageAdjustment);
            }
        }

        public BasicAdjustmentList WeaponSpecificToHitAdjustments
        {
            get 
            {
                if (Weapon == null)
                    return null;

                WeaponBonusValue bonus = player.WeaponBonuses[Weapon];
 
                return (bonus == null ? null : bonus.ToHitAdjustments); 
            }
        }

        public BasicAdjustmentList WeaponSpecificDamageAdjustments
        {
            get 
            {
                if (Weapon == null)
                    return null;

                WeaponBonusValue bonus = player.WeaponBonuses[Weapon];

                return (bonus == null ? null : bonus.DamageAdjustments); 
            }
        }

        public int LevelBonus
        {
            get { return player.LevelBonus; }
        }

        void bonus_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("WeaponSpecificToHitAdjustments");
            Notify("WeaponSpecificDamageAdjustments");
            Notify("WeaponSpecificToHitAdjustment");
            Notify("WeaponSpecificDamageAdjustment");
            Notify("TotalToHitAdjustment");
            Notify("TotalDamageAdjustment");
            Notify("TotalToHitBonus");
            Notify("TotalDamageBonus");
        }

        void damageAdjustments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Notify("TotalDamageAdjustment");
            Notify("TotalDamageBonus");
        }

        void damageAdjustments_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("TotalDamageAdjustment");
            Notify("TotalDamageBonus");
        }

        void toHitAdjustments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Notify("TotalToHitAdjustment");
            Notify("TotalToHitBonus");
        }

        void toHitAdjustments_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("TotalToHitAdjustment");
            Notify("TotalToHitBonus");
        }

        void player_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Weapon") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "WeaponOffhand") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "RangedWeapon") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Implement") == 0))
            {
                if (playerWeapon != Weapon)
                {
                    if (playerWeapon != null)
                        playerWeapon.PropertyChanged -= new PropertyChangedEventHandler(Weapon_PropertyChanged);

                    if (Weapon != null)
                        Weapon.PropertyChanged += new PropertyChangedEventHandler(Weapon_PropertyChanged);

                    playerWeapon = Weapon;

                    if (weaponBonus != null)
                        weaponBonus.PropertyChanged -= new PropertyChangedEventHandler(bonus_PropertyChanged);

                    weaponBonus = (Weapon == null ? null : player.WeaponBonuses[Weapon]);

                    if (weaponBonus != null)
                        weaponBonus.PropertyChanged += new PropertyChangedEventHandler(bonus_PropertyChanged);

                    Notify("ToHitSpec");
                    Notify("DamageSpec");
                    Notify("AttributeBonus");
                    Notify("ProficiencyBonus");
                    Notify("EnhancementBonus");
                    Notify("WeaponSpecificToHitAdjustments");
                    Notify("WeaponSpecificDamageAdjustments");
                    Notify("TotalToHitBonus");
                    Notify("TotalDamageBonus");
                }
            }
            else if ((StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "DexModifier") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "StrModifier") == 0))
            {
                Notify("AttributeBonus");
                Notify("TotalDamageBonus");
                Notify("TotalToHitBonus");
            }
            else if (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "LevelBonus") == 0)
            {
                Notify("LevelBonus");
                Notify("TotalToHitBonus");
            }
        }

        void Weapon_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("ToHitSpec");
            Notify("DamageSpec");
            Notify("AttributeBonus");
            Notify("ProficiencyBonus");
            Notify("EnhancementBonus");
            Notify("WeaponSpecificToHitAdjustments");
            Notify("WeaponSpecificDamageAdjustments");
            Notify("TotalToHitBonus");
            Notify("TotalDamageBonus");
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
