using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class DefenseValue : INotifyPropertyChanged
    {
        private Player player;
        private DefenseType defenseType;
        private BasicAdjustmentList miscAdjustments;

        public DefenseValue(Player player, DefenseType defenseType)
        {
            this.player = player;
            this.defenseType = defenseType;
            this.miscAdjustments = new BasicAdjustmentList();

            player.PropertyChanged += new PropertyChangedEventHandler(player_PropertyChanged);
            player.Class.PropertyChanged += new PropertyChangedEventHandler(Class_PropertyChanged);
            miscAdjustments.ContainedElementChanged += new PropertyChangedEventHandler(miscAdjustments_ContainedElementChanged);
        }

        public Player Player { get { return player; } }
        public DefenseType DefenseType { get { return defenseType; } }
        public BasicAdjustmentList MiscAdjustments { get { return miscAdjustments; } }

        public int Value
        {
            get
            {
                return 10 + player.LevelBonus + GetArmorBonus() + GetClassBonus() + miscAdjustments.TotalAdjustment;
            }
        }

        private int GetClassBonus()
        {
            switch (defenseType)
            {
                case DefenseType.Fortitude:
                    return player.Class.FortitudeBonus;
                case DefenseType.Reflex:
                    return player.Class.ReflexBonus;
                case DefenseType.Will:
                    return player.Class.WillBonus;
                default:
                    return 0;
            }
        }

        private int GetArmorBonus()
        {
            // TODO: Determine armor bonus...
            return 0;
        }

        private void player_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // TODO: Check for armor change...

            if (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "LevelBonus") == 0)
                Notify("Value");
        }

        private void Class_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "FortitudeBonus") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "ReflexBonus") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "WillBonus") == 0))
            {
                Notify("Value"); 
            }
        }

        private void miscAdjustments_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("Value");
        }

        private void Notify(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
