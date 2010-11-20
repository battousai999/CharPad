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
        private PlayerClass playerClass;
        private DefenseType defenseType;
        private BasicAdjustmentList miscAdjustments;

        public DefenseValue(Player player, DefenseType defenseType)
        {
            this.player = player;
            this.defenseType = defenseType;
            this.miscAdjustments = new BasicAdjustmentList();

            player.PropertyChanged += new PropertyChangedEventHandler(player_PropertyChanged);

            this.playerClass = player.Class;

            if (player.Class != null)
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
                return 10 + player.LevelBonus + GetAttributeBonus() + GetArmorBonus() + GetClassBonus() + miscAdjustments.TotalAdjustment;
            }
        }

        private int GetAttributeBonus()
        {
            switch (defenseType)
            {
                case DefenseType.AC:
                    return ((player.Armor == null) || !player.Armor.IsHeavy ? Math.Max(player.DexModifier, player.IntModifier) : 0);
                case DefenseType.Fortitude:
                    return Math.Max(player.StrModifier, player.ConModifier);
                case DefenseType.Reflex:
                    return Math.Max(player.DexModifier, player.IntModifier);
                case DefenseType.Will:
                    return Math.Max(player.WisModifier, player.ChaModifier);
                default:
                    throw new InvalidOperationException("Unexpected defense type: " + Enum.Format(typeof(DefenseType), defenseType, "G"));
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
            if (defenseType == DefenseType.AC)
                return (player.Armor == null ? 0 : player.Armor.TotalBonus) + (player.Shield == null ? 0 : player.Shield.TotalBonus);
            else if (defenseType == DefenseType.Reflex)
                return (player.Shield == null ? 0 : player.Shield.TotalBonus);
            else
                return 0;
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

            if ((StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "LevelBonus") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Armor") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Shield") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "StrModifier") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "ConModifier") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "DexModifier") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "IntModifier") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "WisModifier") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "ChaModifier") == 0))
            {
                Notify("Value");
            }
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
