using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class Party : INotifyPropertyChanged
    {
        private ObservableCollectionEx<Player> members;
        private bool isDirty = false;

        public Party()
        {
            members = new ObservableCollectionEx<Player>();

            members.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(members_CollectionChanged);
            members.ContainedElementChanged += new PropertyChangedEventHandler(members_ContainedElementChanged);
        }

        void members_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            isDirty = true;
            Notify("IsDirty");
            Notify("Members");
        }

        void members_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            isDirty = true;
            Notify("IsDirty");
            Notify("Members");
        }

        public ObservableCollectionEx<Player> Members
        {
            get { return members; }
        }

        public bool IsDirty
        {
            get { return isDirty; }
        }

        private void Notify(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
