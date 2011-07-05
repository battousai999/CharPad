using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public interface IInventoryItem : INotifyPropertyChanged
    {
        string Name { get; }

        IInventoryItem Clone();
    }
}
