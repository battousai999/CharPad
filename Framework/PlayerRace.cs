using System;
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
        private int baseSpeed;

        public string Name { get { return name; } set { name = value; Notify("Name"); } }
        public CreatureSize Size { get { return size; } set { size = value; Notify("Size"); } }
        public int BaseSpeed { get { return baseSpeed; } set { baseSpeed = value; Notify("BaseSpeed"); } }

        public PlayerRace()
        {
        }

        public PlayerRace(string name, CreatureSize size, int baseSpeed)
        {
            this.name = name;
            this.size = size;
            this.baseSpeed = baseSpeed;
        }

        #region INotifyPropertyChanged Members

        private void Notify(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void CopyValues(PlayerRace playerRace)
        {
            this.Name = playerRace.Name;
            this.Size = playerRace.Size;
            this.BaseSpeed = playerRace.BaseSpeed;
        }
    }
}
