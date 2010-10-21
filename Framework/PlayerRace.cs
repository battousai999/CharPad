﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class PlayerRace : INotifyPropertyChanged
    {
        private string name;
        private CreatureSize size;

        public string Name { get { return name; } set { name = value; Notify("Name"); } }
        public CreatureSize Size { get { return size; } set { size = value; Notify("Size"); } }

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
