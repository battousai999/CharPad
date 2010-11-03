using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class SpeedValue : INotifyPropertyChanged
    {
        private Player player;
        private PlayerRace playerRace;
        private BasicAdjustmentList miscAdjustments;

        public SpeedValue(Player player)
        {
            this.player = player;
            this.miscAdjustments = new BasicAdjustmentList();

            player.PropertyChanged += new PropertyChangedEventHandler(player_PropertyChanged);

            this.playerRace = player.Race;

            if (player.Race != null)
                player.Race.PropertyChanged += new PropertyChangedEventHandler(Race_PropertyChanged);

            miscAdjustments.ContainedElementChanged += new PropertyChangedEventHandler(miscAdjustments_ContainedElementChanged);
        }

        public Player Player { get { return player; } }
        public BasicAdjustmentList MiscAdjustments { get { return miscAdjustments; } }

        public int Value
        {
            get
            {
                return player.Race.BaseSpeed + GetArmorAdjustment() + miscAdjustments.TotalAdjustment;
            }
        }

        private int GetArmorAdjustment()
        {
            return (player.Armor == null ? 0 : player.Armor.SpeedModifier);
        }

        private void player_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Race") == 0)
            {
                if (playerRace != null)
                    playerRace.PropertyChanged -= new PropertyChangedEventHandler(player_PropertyChanged);

                if (player.Race != null)
                    player.PropertyChanged += new PropertyChangedEventHandler(player_PropertyChanged);

                playerRace = player.Race;
            }

            if (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Armor") == 0)
                Notify("Value");
        }

        private void Race_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "BaseSpeed") == 0)
                Notify("Value");
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
