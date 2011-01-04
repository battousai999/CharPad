using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class WeaponBonusList : ObservableCollectionEx<WeaponBonus>
    {
        public WeaponBonusList()
            : base()
        {
        }

        public WeaponBonusValue this[Weapon weapon]
        {
            get
            {
                WeaponBonus bonus = this.FirstOrDefault(x => x.Weapon == weapon);

                return (bonus == null ? null : bonus.Bonus);
            }

            set
            {
                WeaponBonus bonus = this.FirstOrDefault(x => x.Weapon == weapon);

                if (bonus == null)
                    this.Add(new WeaponBonus(weapon, value));
                else
                    bonus.Bonus = value;
            }
        }

        public void Add(Weapon weapon, WeaponBonusValue bonus)
        {
            this.Add(new WeaponBonus(weapon, bonus));
        }

        public void Remove(Weapon weapon)
        {
            WeaponBonus bonus = this.FirstOrDefault(x => x.Weapon == weapon);

            if (bonus == null)
                throw new ArgumentException("No matching item found.");

            this.Remove(bonus);
        }
    }

    public class WeaponBonus : INotifyPropertyChanged
    {
        private Weapon weapon;
        private WeaponBonusValue bonus;

        public WeaponBonus()
        {
        }

        public WeaponBonus(Weapon weapon, WeaponBonusValue bonus)
        {
            this.weapon = weapon;
            this.bonus = bonus;
        }

        public Weapon Weapon
        {
            get { return weapon; }
            set { weapon = value; Notify("Weapon"); }
        }

        public WeaponBonusValue Bonus
        {
            get { return bonus; }
            set 
            { 
                if (bonus != null)
                    bonus.PropertyChanged -= new PropertyChangedEventHandler(bonus_PropertyChanged);

                bonus = value; 

                if (bonus != null)
                    bonus.PropertyChanged += new PropertyChangedEventHandler(bonus_PropertyChanged);
                
                Notify("Bonus"); 
            }
        }

        public int TotalToHitBonus
        {
            get { return bonus.TotalToHitAdjustment; }
        }

        public int TotalDamageBonus
        {
            get { return bonus.TotalDamageAdjustment; }
        }

        void bonus_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "TotalToHitAdjustment") == 0)
                Notify("TotalToHitBonus");

            if (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "TotalDamageAdjustment") == 0)
                Notify("TotalDamageBonus");

            Notify("Bonus");
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
