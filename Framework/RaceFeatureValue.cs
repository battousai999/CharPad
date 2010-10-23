using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class RaceFeatureValue : INotifyPropertyChanged
    {
        private string name;
        private string shortDescription;
        private string longDescription;

        public RaceFeatureValue(string name, string description)
            : this(name, description, null)
        {
        }

        public RaceFeatureValue(string name, string shortDescription, string longDescription)
        {
            this.name = name;
            this.shortDescription = shortDescription;
            this.longDescription = longDescription;
        }

        public string Name { get { return name; } set { name = value; Notify("Name"); } }
        public string ShortDescription { get { return shortDescription; } set { shortDescription = value; Notify("ShortDescription"); Notify("LongDescription"); } }

        public string LongDescription 
        { 
            get { return (String.IsNullOrWhiteSpace(longDescription) ? shortDescription : longDescription); } 
            set { longDescription = value; Notify("ShortDescription"); Notify("LongDescription"); } }

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
