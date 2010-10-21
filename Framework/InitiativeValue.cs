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
        private List<BasicAdjustment> miscAdjustments;

        public InitiativeValue(Player player)
        {
            this.player = player;

            player.PropertyChanged += new PropertyChangedEventHandler(player_PropertyChanged);
        }

        private void player_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Level") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "DexModifier") == 0))
            {
                Notify("Value");
            }
        }

        public Player Player { get { return player; } }
        public List<BasicAdjustment> MiscAdjustments { get { return miscAdjustments; } }

        public int Value
        {
            get
            {
                return player.LevelBonus + player.DexModifier + miscAdjustments.Sum(x => x.Modifier);
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
