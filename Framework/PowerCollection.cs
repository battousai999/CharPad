using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CharPad.Framework
{
    public class PowerCollection : ObservableCollectionEx<Power>, INotifyPropertyChanged
    {
        private List<Power> atWillPowers = null;
        private List<Power> encounterPowers = null;
        private List<Power> dailyPowers = null;

        public PowerCollection()
            : base()
        {
        }

        public PowerCollection(IEnumerable<Power> list)
            : base(list)
        {
        }

        private void Notify(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected override void OnCollectionChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

            ListAdapter<Power> newItems = new ListAdapter<Power>(e.NewItems);
            ListAdapter<Power> oldItems = new ListAdapter<Power>(e.OldItems);

            if (newItems.Any(x => x.PowerType == PowerType.AtWill) || oldItems.Any(x => x.PowerType == PowerType.AtWill))
            {
                atWillPowers = null;
                Notify("AtWillPowers");
            }

            if (newItems.Any(x => x.PowerType == PowerType.Encounter) || oldItems.Any(x => x.PowerType == PowerType.Encounter))
            {
                encounterPowers = null;
                Notify("EncounterPowers");
            }

            if (newItems.Any(x => x.PowerType == PowerType.Daily) || oldItems.Any(x => x.PowerType == PowerType.Daily))
            {
                dailyPowers = null;
                Notify("DailyPowers");
            }
        }

        public List<Power> AtWillPowers
        {
            get
            {
                if (atWillPowers == null)
                    atWillPowers = (new ListAdapter<Power>(this)).Where(x => x.PowerType == PowerType.AtWill).ToList();

                return atWillPowers;
            }
        }

        public List<Power> EncounterPowers
        {
            get
            {
                if (encounterPowers == null)
                    encounterPowers = (new ListAdapter<Power>(this)).Where(x => x.PowerType == PowerType.Encounter).ToList();

                return encounterPowers;
            }
        }

        public List<Power> DailyPowers
        {
            get
            {
                if (dailyPowers == null)
                    dailyPowers = (new ListAdapter<Power>(this)).Where(x => x.PowerType == PowerType.Daily).ToList();

                return dailyPowers;
            }
        }
    }
}
