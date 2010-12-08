using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class SurgesPerDayValue : INotifyPropertyChanged
    {
        private Player player;
        private PlayerClass playerClass;
        private BasicAdjustmentList miscAdjustments;

        public SurgesPerDayValue(Player player)
        {
            this.player = player;
            this.miscAdjustments = new BasicAdjustmentList();

            player.PropertyChanged += new PropertyChangedEventHandler(player_PropertyChanged);

            if (player.Class != null)
                player.Class.PropertyChanged += new PropertyChangedEventHandler(Class_PropertyChanged);

            playerClass = player.Class;

            miscAdjustments.ContainedElementChanged += new PropertyChangedEventHandler(miscAdjustments_ContainedElementChanged);
            miscAdjustments.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(miscAdjustments_CollectionChanged);
        }

        void Class_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("BaseHealingSurges");
            Notify("Value");
        }

        void miscAdjustments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Notify("TotalMiscAdjustment");
            Notify("Value");
        }

        void miscAdjustments_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("TotalMiscAdjustment");
            Notify("Value");
        }

        void player_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Class") == 0)
            {
                if (playerClass != null)
                    playerClass.PropertyChanged -= new PropertyChangedEventHandler(Class_PropertyChanged);

                if (player.Class != null)
                    player.Class.PropertyChanged += new PropertyChangedEventHandler(Class_PropertyChanged);

                playerClass = player.Class;

                Notify("BaseHealingSurges");
                Notify("Value");
            }

            if (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "ConModifier") == 0)
            {
                Notify("ConModifier");
                Notify("Value");
            }
        }

        public int Value
        {
            get
            {
                return player.Class.BaseHealingSurges + player.ConModifier + miscAdjustments.TotalAdjustment;
            }
        }

        public int BaseHealingSurges
        {
            get { return player.Class.BaseHealingSurges; }
        }

        public int ConModifier
        {
            get { return player.ConModifier; }
        }

        public int TotalMiscAdjustment
        {
            get { return miscAdjustments.TotalAdjustment; }
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
