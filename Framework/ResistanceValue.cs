using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class ResistanceValue : INotifyPropertyChanged
    {
        private int modifier;
        private string damageType;
        private string description;

        public ResistanceValue(int modifier, string damageType)
            : this(modifier, damageType, "")
        {
        }

        public ResistanceValue(int modifier, string damageType, string description)
        {
            this.modifier = modifier;
            this.damageType = damageType;
            this.description = description;
        }

        public int Modifier { get { return modifier; } set { modifier = value; Notify("Modifier"); } }
        public string DamageType { get { return damageType; } set { damageType = value; Notify("DamageType"); } }
        public string Description { get { return description; } set { description = value; Notify("Description"); } }

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
