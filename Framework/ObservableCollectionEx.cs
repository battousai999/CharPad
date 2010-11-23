using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Generic;

namespace CharPad.Framework
{
    public class ObservableCollectionEx<T> : ObservableCollection<T>, INotifyPropertyChanged
        where T : INotifyPropertyChanged
    {
        public virtual event PropertyChangedEventHandler ContainedElementChanged;

        public ObservableCollectionEx()
            : base()
        {
        }

        public ObservableCollectionEx(IEnumerable<T> list)
            : base()
        {
            foreach (T element in list)
            {
                Add(element);
            }
        }

        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Unsubscribe(e.OldItems);
            Subscribe(e.NewItems);
            base.OnCollectionChanged(e);
        }

        private void Subscribe(System.Collections.IList iList)
        {
            if (iList != null)
            {
                foreach (T element in iList)
                    element.PropertyChanged += (x, y) => OnContainedElementChanged(y);
            }
        }

        private void Unsubscribe(System.Collections.IList iList)
        {
            if (iList != null)
            {
                foreach (T element in iList)
                    element.PropertyChanged -= (x, y) => OnContainedElementChanged(y);
            }
        }

        protected virtual void OnContainedElementChanged(PropertyChangedEventArgs e)
        {
            if (ContainedElementChanged != null)
                ContainedElementChanged(this, e);
        }
    }
}
