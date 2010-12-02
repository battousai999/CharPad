using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class WeaponBonusValue : INotifyPropertyChanged
    {
        private BasicAdjustmentList toHitAdjustments;
        private BasicAdjustmentList damageAdjustments;

        public WeaponBonusValue()
        {
            this.toHitAdjustments = new BasicAdjustmentList();
            this.damageAdjustments = new BasicAdjustmentList();

            toHitAdjustments.ContainedElementChanged += new PropertyChangedEventHandler(toHitAdjustments_ContainedElementChanged);
            toHitAdjustments.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(toHitAdjustments_CollectionChanged);

            damageAdjustments.ContainedElementChanged += new PropertyChangedEventHandler(damageAdjustments_ContainedElementChanged);
            damageAdjustments.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(damageAdjustments_CollectionChanged);
        }

        public BasicAdjustmentList ToHitAdjustments { get { return toHitAdjustments; } }
        public BasicAdjustmentList DamageAdjustments { get { return damageAdjustments; } }

        public int TotalToHitAdjustment
        {
            get { return toHitAdjustments.TotalAdjustment; }
        }

        public int TotalDamageAdjustment
        {
            get { return damageAdjustments.TotalAdjustment; }
        }

        void damageAdjustments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Notify("TotalDamageAdjustment");
        }

        void damageAdjustments_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("TotalDamageAdjustment");
        }

        void toHitAdjustments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Notify("TotalToHitAdjustment");
        }

        void toHitAdjustments_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("TotalToHitAdjustment");
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
