using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class InitiativeValue : INotifyPropertyChanged
    {
        private Player player;
        private BasicAdjustmentList miscAdjustments;

        public InitiativeValue(Player player)
        {
            this.player = player;
            this.miscAdjustments = new BasicAdjustmentList();

            player.PropertyChanged += new PropertyChangedEventHandler(player_PropertyChanged);
            miscAdjustments.ContainedElementChanged += new PropertyChangedEventHandler(miscAdjustments_ContainedElementChanged);
            miscAdjustments.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(miscAdjustments_CollectionChanged);
        }

        private void player_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "LevelBonus") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "DexModifier") == 0))
            {
                Notify("Value");
            }
        }

        public Player Player { get { return player; } }
        public BasicAdjustmentList MiscAdjustments { get { return miscAdjustments; } }

        public int Value
        {
            get
            {
                return player.LevelBonus + player.DexModifier + miscAdjustments.TotalAdjustment;
            }
        }

        public int TotalMiscAdjustment
        {
            get { return miscAdjustments.TotalAdjustment; }
        }

        private void miscAdjustments_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("TotalMiscAdjustment");
            Notify("Value");
        }

        private void miscAdjustments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Notify("TotalMiscAdjustment");
            Notify("Value");
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
