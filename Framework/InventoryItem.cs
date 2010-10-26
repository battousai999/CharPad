using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class InventoryItem : IInventoryItem, INotifyPropertyChanged
    {
        private bool isStackable;
        private int count;

        public string Name { get; set; }

        public InventoryItem(string name)
        {
            this.Name = name;
            this.isStackable = false;
            this.count = 1;
        }

        public InventoryItem(string name, int count)
        {
            this.Name = name;
            this.isStackable = true;
            this.count = count;
        }

        public bool IsStackable { get { return isStackable; } }
        public int Count { get { return count; } set { count = value; Notify("Count"); } }

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
