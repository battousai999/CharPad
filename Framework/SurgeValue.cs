using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class SurgeValue : INotifyPropertyChanged
    {
        private Player player;
        private List<BasicAdjustment> miscAdjustments;

        public SurgeValue(Player player)
        {
            this.player = player;

            player.HitPoints.PropertyChanged += new PropertyChangedEventHandler(HitPoints_PropertyChanged);
        }

        private void HitPoints_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Value") == 0)
                Notify("Value");
        }

        public Player Player { get { return player; } }
        public List<BasicAdjustment> MiscAdjustments { get { return miscAdjustments; } }

        public int Value
        {
            get
            {
                return (player.HitPoints.Value / 4) + miscAdjustments.Sum(x => x.Modifier);
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
