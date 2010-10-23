using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class HitPointsValue : INotifyPropertyChanged
    {
        private Player player;
        private BasicAdjustmentList miscAdjustments;

        public HitPointsValue(Player player)
        {
            this.player = player;
            this.miscAdjustments = new BasicAdjustmentList();

            player.PropertyChanged += new PropertyChangedEventHandler(player_PropertyChanged);
            miscAdjustments.ContainedElementChanged += new PropertyChangedEventHandler(miscAdjustments_ContainedElementChanged);
        }

        public Player Player { get { return player; } }
        public BasicAdjustmentList MiscAdjustments { get { return miscAdjustments; } }

        public int Value
        {
            get
            {
                return player.Class.BaseHealth + player.Con + miscAdjustments.TotalAdjustment;
            }
        }

        private void player_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Class") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "ConModifier") == 0))
            {
                Notify("Value");
            }
        }

        private void miscAdjustments_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
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
