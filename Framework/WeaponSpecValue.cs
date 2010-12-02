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
        private bool isMainWeapon;
        private bool asRanged;
        private BasicAdjustmentList toHitAdjustments;
        private BasicAdjustmentList damageAdjustments;

        public WeaponSpecValue(Player player, bool isMainWeapon)
            : this(player, isMainWeapon, false)
        {
        }

        public WeaponSpecValue(Player player, bool isMainWeapon, bool asRanged)
        {
            this.player = player;
            this.isMainWeapon = isMainWeapon;
            this.asRanged = asRanged;
            this.toHitAdjustments = new BasicAdjustmentList();
            this.damageAdjustments = new BasicAdjustmentList();

            player.PropertyChanged += new PropertyChangedEventHandler(player_PropertyChanged);

            playerWeapon = Weapon;

            if (Weapon != null)
                Weapon.PropertyChanged += new PropertyChangedEventHandler(Weapon_PropertyChanged);
            
            player.WeaponBonuses.ContainedElementChanged += new PropertyChangedEventHandler(WeaponBonuses_ContainedElementChanged);
            player.WeaponBonuses.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(WeaponBonuses_CollectionChanged);
            toHitAdjustments.ContainedElementChanged += new PropertyChangedEventHandler(toHitAdjustments_ContainedElementChanged);
            toHitAdjustments.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(toHitAdjustments_CollectionChanged);
            damageAdjustments.ContainedElementChanged += new PropertyChangedEventHandler(damageAdjustments_ContainedElementChanged);
            damageAdjustments.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(damageAdjustments_CollectionChanged);
        }

        public Player Player { get { return player; } }
        public bool IsMainWeapon { get { return isMainWeapon; } }
        public BasicAdjustmentList ToHitAdjustments { get { return toHitAdjustments; } }
        public BasicAdjustmentList DamageAdjustments { get { return damageAdjustments; } }

        public Weapon Weapon { get { return (isMainWeapon ? player.Weapon : player.WeaponOffhand); } }

        public int TotalToHitBonus
        {
            get
            {
                if (Weapon == null)
                    return 0;

                return AttributeBonus + ProficiencyBonus + LevelBonus + TotalToHitAdjustments + WeaponSpecificToHitAdjustments;
            }
        }

        public int TotalDamageBonus
        {
            get
            {
                if (Weapon == null)
                    return 0;

                return AttributeBonus + TotalDamageAdjustments + WeaponSpecificDamageAdjustments;
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

        public int AttributeBonus
        {
            get
            {
                if (Weapon == null)
                    return 0;

                if (Weapon.IsRanged && !Weapon.HasWeaponProperty(WeaponProperties.LightThrown | WeaponProperties.HeavyThrown))
                    return player.DexModifier;
                else
                    return player.StrModifier;
            }
        }

        public int ProficiencyBonus
        {
            get { return (Weapon == null ? 0 : Weapon.ProficiencyBonus); }
        }

        public int TotalToHitAdjustments
        {
            get { return toHitAdjustments.TotalAdjustment; }
        }

        public int TotalDamageAdjustments
        {
            get { return damageAdjustments.TotalAdjustment; }
        }

        public int WeaponSpecificToHitAdjustments
        {
            get
            {
                if (Weapon == null)
                    return 0;

                WeaponBonusValue weaponBonus = player.WeaponBonuses[Weapon];

                return (weaponBonus == null ? 0 : weaponBonus.TotalToHitAdjustment);
            }
        }

        public int WeaponSpecificDamageAdjustments
        {
            get
            {
                if (Weapon == null)
                    return 0;

                WeaponBonusValue weaponBonus = player.WeaponBonuses[Weapon];

                return (weaponBonus == null ? 0 : weaponBonus.TotalDamageAdjustment);
            }
        }

        public int LevelBonus
        {
            get { return player.LevelBonus; }
        }

        void WeaponBonuses_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Notify("WeaponSpecificToHitAdjustments");
            Notify("TotalToHitBonus");
        }

        void WeaponBonuses_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("WeaponSpecificDamageAdjustments");
            Notify("TotalDamageBonus");
        }

        void damageAdjustments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Notify("TotalDamageAdjustments");
            Notify("TotalDamageBonus");
        }

        void damageAdjustments_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("TotalDamageAdjustments");
            Notify("TotalDamageBonus");
        }

        void toHitAdjustments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Notify("TotalToHitAdjustments");
            Notify("TotalToHitBonus");
        }

        void toHitAdjustments_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("TotalToHitAdjustments");
            Notify("TotalToHitBonus");
        }

        void player_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Weapon") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "WeaponOffhand") == 0))
            {
                if (playerWeapon != Weapon)
                {
                    if (playerWeapon != null)
                        playerWeapon.PropertyChanged -= new PropertyChangedEventHandler(Weapon_PropertyChanged);

                    if (Weapon != null)
                        Weapon.PropertyChanged += new PropertyChangedEventHandler(Weapon_PropertyChanged);

                    playerWeapon = Weapon;

                    Notify("ToHitSpec");
                    Notify("DamageSpec");
                    Notify("AttributeBonus");
                    Notify("ProficiencyBonus");
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
