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
            miscAdjustments.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(miscAdjustments_CollectionChanged);
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

        public int BaseSpeed
        {
            get { return player.Race.BaseSpeed; }
        }

        public int ArmorAdjustment
        {
            get { return GetArmorAdjustment(); }
        }

        public int TotalMiscAdjustment
        {
            get { return miscAdjustments.TotalAdjustment; }
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
                    playerRace.PropertyChanged -= new PropertyChangedEventHandler(Race_PropertyChanged);

                if (player.Race != null)
                    player.Race.PropertyChanged += new PropertyChangedEventHandler(Race_PropertyChanged);

                playerRace = player.Race;
                Notify("BaseSpeed");
                Notify("Value");
            }

            if (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Armor") == 0)
            {
                Notify("ArmorAdjustment");
                Notify("Value");
            }
        }

        private void Race_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "BaseSpeed") == 0)
            {
                Notify("BaseSpeed");
                Notify("Value");
            }
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
