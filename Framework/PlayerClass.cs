using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class PlayerClass : INotifyPropertyChanged
    {
        private string name;
        private int baseHealth;
        private int healthPerLevel;
        private int baseHealingSurges;
        private int fortitudeBonus;
        private int reflexBonus;
        private int willBonus;

        public string Name { get { return name; } set { name = value; Notify("Name"); } }
        public int BaseHealth { get { return baseHealth; } set { baseHealth = value; Notify("BaseHealth"); } }
        public int HealthPerLevel { get { return healthPerLevel; } set { healthPerLevel = value; Notify("HealthPerLevel"); } }
        public int BaseHealingSurges { get { return baseHealingSurges; } set { baseHealingSurges = value; Notify("BaseHealingSurges"); } }
        public int FortitudeBonus { get { return fortitudeBonus; } set { fortitudeBonus = value; Notify("FortitudeBonus"); } }
        public int ReflexBonus { get { return reflexBonus; } set { reflexBonus = value; Notify("ReflexBonus"); } }
        public int WillBonus { get { return willBonus; } set { willBonus = value; Notify("WillBonus"); } }

        public PlayerClass(string name, int baseHealth, int healthPerLevel, int baseHealingSurges)
        {
            this.name = name;
            this.baseHealth = baseHealth;
            this.healthPerLevel = healthPerLevel;
            this.baseHealingSurges = baseHealingSurges;
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
