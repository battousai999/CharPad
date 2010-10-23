using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class BasicAdjustment : INotifyPropertyChanged
    {
        private int modifier;
        private string note;

        public BasicAdjustment(int modifier, string note)
        {
            this.modifier = modifier;
            this.note = note;
        }

        public int Modifier { get { return modifier; } set { modifier = value; Notify("Modifier"); } }
        public string Note { get { return note; } set { note = value; Notify("Note"); } }

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
