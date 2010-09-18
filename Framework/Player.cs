using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class Player : INotifyPropertyChanged
    {
        private string name;
        private PlayerClass _class;

        public string Name { get { return name; } set { name = value; Notify("Name"); } }
        public PlayerClass Class { get { return _class; } set { _class = value; Notify("Class"); } }

        public Player()
        {
            this.name = "";
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
