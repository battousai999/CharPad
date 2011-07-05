using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class InventoryItem : IInventoryItem, INotifyPropertyChanged
    {
        private string name;
        private bool isStackable;
        private int count;

        public InventoryItem()
        {
        }

        public InventoryItem(string name)
        {
            this.name = name;
            this.isStackable = false;
            this.count = 1;
        }

        public InventoryItem(string name, int count)
        {
            this.name = name;
            this.isStackable = true;
            this.count = count;
        }

        public string Name { get { return name; } set { name = value; Notify("Name"); } }
        public bool IsStackable { get { return isStackable; } set { isStackable = value; Notify("IsStackable"); } }
        public int Count { get { return count; } set { count = value; Notify("Count"); } }

        public IInventoryItem Clone()
        {
            InventoryItem item = new InventoryItem();

            item.name = this.name;
            item.isStackable = this.isStackable;
            item.count = this.count;

            return item;
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
