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
        private PlayerClass playerClass;
        private BasicAdjustmentList miscAdjustments;

        public HitPointsValue(Player player)
        {
            this.player = player;
            this.miscAdjustments = new BasicAdjustmentList();

            this.playerClass = player.Class;

            if (player.Class != null)
                player.Class.PropertyChanged += new PropertyChangedEventHandler(Class_PropertyChanged);

            player.PropertyChanged += new PropertyChangedEventHandler(player_PropertyChanged);
            miscAdjustments.ContainedElementChanged += new PropertyChangedEventHandler(miscAdjustments_ContainedElementChanged);
        }

        public Player Player { get { return player; } }
        public BasicAdjustmentList MiscAdjustments { get { return miscAdjustments; } }

        public int Value
        {
            get
            {
                return player.Class.BaseHealth + player.Con + miscAdjustments.TotalAdjustment + (player.Level * player.Class.HealthPerLevel);
            }
        }

        private void player_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Class") == 0)
            {
                if (playerClass != null)
                    playerClass.PropertyChanged -= new PropertyChangedEventHandler(Class_PropertyChanged);

                if (player.Class != null)
                    player.Class.PropertyChanged += new PropertyChangedEventHandler(Class_PropertyChanged);

                playerClass = player.Class;
                Notify("Value");
            }

            if ((StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "ConModifier") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Level") == 0))
            {
                Notify("Value");
            }
        }

        private void Class_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "BaseHealth") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "HealthPerLevel") == 0))
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
