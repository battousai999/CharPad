using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class BasicAdjustmentList : ObservableCollectionEx<BasicAdjustment>
    {
        public int TotalAdjustment
        {
            get
            {
                int value = 0;

                foreach (BasicAdjustment adjustment in this)
                {
                    value += adjustment.Modifier;
                }

                return value;
            }
        }

        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

            Notify("TotalAdjustment");
        }

        protected override void OnContainedElementChanged(PropertyChangedEventArgs e)
        {
            base.OnContainedElementChanged(e);

            Notify("TotalAdjustment");
        }

        private void Notify(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}
