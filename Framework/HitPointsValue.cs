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
        private List<BasicAdjustment> miscAdjustments;

        public HitPointsValue(Player player)
        {
            this.player = player;

            player.PropertyChanged += new PropertyChangedEventHandler(player_PropertyChanged);
        }

        public Player Player { get { return player; } }
        public List<BasicAdjustment> MiscAdjustments { get { return miscAdjustments; } }

        public int Value
        {
            get
            {
                return player.Class.BaseHealth + player.Con + miscAdjustments.Sum(x => x.Modifier);
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
