using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CharPad.Framework;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;

namespace CharPad
{
    /// <summary>
    /// Interaction logic for CharacterWindow.xaml
    /// </summary>
    public partial class CharacterWindow : Window, INotifyPropertyChanged
    {
        #region Classes

        public class InvItem : INotifyPropertyChanged
        {
            public static InvItem NullItem = new InvItem(null);

            private string displayName;
            private IInventoryItem item;

            public string DisplayName { get { return displayName; } }
            public IInventoryItem Item { get { return item; } }

            public InvItem(IInventoryItem item)
            {
                this.displayName = (item == null ? "(none)" : item.Name);
                this.item = item;

                if (item != null)
                    item.PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
            }

            void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                if (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Name") == 0)
                {
                    displayName = item.Name;
                    Notify("DisplayName");
                }
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

        #endregion

        private Player player;
        private bool isNew;
        private ObservableCollectionEx<IInventoryItem> inventory;
        private ObservableCollection<InvItem> mainWeapons;
        private ObservableCollection<InvItem> offhandWeapons;
        private ObservableCollection<InvItem> rangedWeapons;
        private ObservableCollection<InvItem> armors;
        private ObservableCollection<InvItem> shields;
        private ObservableCollection<InvItem> implements;
        private InvItem currentArmor;
        private InvItem currentShield;
        private InvItem currentMainWeapon;
        private InvItem currentOffhandWeapon;
        private InvItem currentRangedWeapon;
        private InvItem currentImplement;
        private bool ignorePlayerWeaponUpdating = false;
        private bool ignorePlayerArmorUpdating = false;
        private BitmapSource characterImage;

        public CharacterWindow(Player player, bool isNew)
        {
            this.player = player;
            this.isNew = isNew;

            characterImage = Common.BuildBitmapImage(player.Picture);

            List<IInventoryItem> wieldedItems = player.WieldedItems;

            inventory = new ObservableCollectionEx<IInventoryItem>(player.Inventory.Where(x => !wieldedItems.Contains(x)));
            player.Inventory.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Inventory_CollectionChanged);
            player.Inventory.ContainedElementChanged += new PropertyChangedEventHandler(Inventory_ContainedElementChanged);
            player.PropertyChanged += new PropertyChangedEventHandler(player_PropertyChanged);

            mainWeapons = new ObservableCollection<InvItem>();
            offhandWeapons = new ObservableCollection<InvItem>();
            rangedWeapons = new ObservableCollection<InvItem>();
            armors = new ObservableCollection<InvItem>();
            shields = new ObservableCollection<InvItem>();
            implements = new ObservableCollection<InvItem>();

            UpdateItemCollections();

            currentMainWeapon = mainWeapons.FirstOrDefault(x => x.Item == player.Weapon);
            currentOffhandWeapon = offhandWeapons.FirstOrDefault(x => x.Item == player.WeaponOffhand);
            currentRangedWeapon = rangedWeapons.FirstOrDefault(x => x.Item == player.RangedWeapon);
            currentArmor = armors.FirstOrDefault(x => x.Item == player.Armor);
            currentShield = shields.FirstOrDefault(x => x.Item == player.Shield);
            currentImplement = implements.FirstOrDefault(x => x.Item == player.Implement);

            InitializeComponent();

            mainGrid.DataContext = player;

            if (isNew)
                this.Title = "New Character";
            else
                this.SetBinding(Window.TitleProperty, new Binding { Source = player, Path = new PropertyPath("CharacterName"), StringFormat = "Edit {0}" });
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            player.Inventory.CollectionChanged -= new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Inventory_CollectionChanged);
            player.Inventory.ContainedElementChanged -= new PropertyChangedEventHandler(Inventory_ContainedElementChanged);
            player.PropertyChanged -= new PropertyChangedEventHandler(player_PropertyChanged);
        }

        void player_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ((StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Weapon") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "WeaponOffhand") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "RangedWeapon") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Implement") == 0))
            {
                UpdateWeaponCollections();
            }

            if ((StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Weapon") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "WeaponOffhand") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "RangedWeapon") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Implement") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Armor") == 0) ||
                (StringComparer.CurrentCultureIgnoreCase.Compare(e.PropertyName, "Shield") == 0))
            {
                UpdateInventoryItems();
            }

            BindingOperations.GetMultiBindingExpression(txtWeaponSpec, TextBlock.TextProperty).UpdateTarget();
            BindingOperations.GetMultiBindingExpression(txtOffhandWeaponSpec, TextBlock.TextProperty).UpdateTarget();
            BindingOperations.GetMultiBindingExpression(txtRangedWeaponSpec, TextBlock.TextProperty).UpdateTarget();
        }

        private void UpdateInventoryItems()
        {
            List<IInventoryItem> wieldedItems = player.WieldedItems;
            List<IInventoryItem> itemsToRemove = new List<IInventoryItem>();

            foreach (IInventoryItem item in inventory)
            {
                if (wieldedItems.Contains(item))
                    itemsToRemove.Add(item);
            }

            foreach (IInventoryItem item in itemsToRemove)
            {
                inventory.Remove(item);
            }

            foreach (IInventoryItem item in player.Inventory.Where(x => !wieldedItems.Contains(x)))
            {
                if (!inventory.Contains(item))
                    inventory.Add(item);
            }
        }

        void Inventory_ContainedElementChanged(object sender, PropertyChangedEventArgs e)
        {
            Notify("Inventory");
        }

        void Inventory_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if ((e.NewItems != null) && (e.NewItems.Count > 0))
            {
                foreach (IInventoryItem item in e.NewItems)
                {
                    inventory.Add(item);
                }
            }

            if ((e.OldItems != null) && (e.OldItems.Count > 0))
            {
                foreach (IInventoryItem item in e.OldItems)
                {
                    inventory.Remove(item);
                }
            }

            UpdateItemCollections();

            Notify("Inventory");            
        }

        private void UpdateItemCollections()
        {
            UpdateWeaponCollections();

            ignorePlayerArmorUpdating = true;

            try
            {
                armors.Clear();
                armors.Add(InvItem.NullItem);
                player.Inventory.Where(x => x is Armor).ToList().ForEach(x => armors.Add(new InvItem(x)));
                currentArmor = armors.FirstOrDefault(x => x.Item == player.Armor);
                Notify("CurrentArmor");

                shields.Clear();
                shields.Add(InvItem.NullItem);
                player.Inventory.Where(x => x is Shield).ToList().ForEach(x => shields.Add(new InvItem(x)));
                currentShield = shields.FirstOrDefault(x => x.Item == player.Shield);
                Notify("CurrentShield");
            }
            finally
            {
                ignorePlayerArmorUpdating = false;
            }
        }

        private void UpdateWeaponCollections()
        {
            ignorePlayerWeaponUpdating = true;

            try
            {
                mainWeapons.Clear();
                mainWeapons.Add(InvItem.NullItem);
                player.Inventory.Where(x => (x is Weapon) && (x != player.WeaponOffhand) && (x != player.RangedWeapon) && (x != player.Implement) && !((Weapon)x).IsRanged).ToList().ForEach(x => mainWeapons.Add(new InvItem(x)));
                currentMainWeapon = mainWeapons.FirstOrDefault(x => x.Item == player.Weapon);
                Notify("CurrentMainWeapon");

                offhandWeapons.Clear();
                offhandWeapons.Add(InvItem.NullItem);
                player.Inventory.Where(x => (x is Weapon) && (x != player.Weapon) && (x != player.RangedWeapon) && (x != player.Implement) && !((Weapon)x).IsRanged).ToList().ForEach(x => offhandWeapons.Add(new InvItem(x)));
                currentOffhandWeapon = offhandWeapons.FirstOrDefault(x => x.Item == player.WeaponOffhand);
                Notify("CurrentOffhandWeapon");

                rangedWeapons.Clear();
                rangedWeapons.Add(InvItem.NullItem);
                player.Inventory.Where(x => (x is Weapon) && (x != player.Weapon) && (x != player.WeaponOffhand) && (x != player.Implement) && ((Weapon)x).IsRanged).ToList().ForEach(x => rangedWeapons.Add(new InvItem(x)));
                currentRangedWeapon = rangedWeapons.FirstOrDefault(x => x.Item == player.RangedWeapon);
                Notify("CurrentRangedWeapon");

                implements.Clear();
                implements.Add(InvItem.NullItem);
                player.Inventory.Where(x => (x is Weapon) && (x != player.Weapon) && (x != player.WeaponOffhand) && (x != player.RangedWeapon) && ((Weapon)x).IsImplement).ToList().ForEach(x => implements.Add(new InvItem(x)));
                currentImplement = implements.FirstOrDefault(x => x.Item == player.Implement);
                Notify("CurrentImplement");
            }
            finally
            {
                ignorePlayerWeaponUpdating = false;
            }
        }

        public Player Player
        {
            get { return player; }
        }

        public ObservableCollectionEx<ResistanceValue> Resistances
        {
            get { return player.Resistances; }
        }

        public InvItem CurrentArmor
        {
            get { return currentArmor; }
            set
            {
                currentArmor = value; 

                if (!ignorePlayerArmorUpdating)
                    player.Armor = (Armor)currentArmor.Item; 

                Notify("CurrentArmor");
            }
        }

        public InvItem CurrentShield
        {
            get { return currentShield; }
            set
            {
                currentShield = value; 

                if (!ignorePlayerArmorUpdating)
                    player.Shield = (Shield)currentShield.Item; 

                Notify("CurrentShield");
            }
        }

        public InvItem CurrentMainWeapon
        {
            get { return currentMainWeapon; }
            set
            {
                currentMainWeapon = value; 

                if (!ignorePlayerWeaponUpdating)
                    player.Weapon = (Weapon)currentMainWeapon.Item;

                Notify("CurrentMainWeapon");
            }
        }

        public InvItem CurrentOffhandWeapon
        {
            get { return currentOffhandWeapon; }
            set
            {
                currentOffhandWeapon = value; 

                if (!ignorePlayerWeaponUpdating)
                    player.WeaponOffhand = (Weapon)currentOffhandWeapon.Item;

                Notify("CurrentOffhandWeapon");
            }
        }

        public InvItem CurrentRangedWeapon
        {
            get { return currentRangedWeapon; }
            set
            {
                currentRangedWeapon = value;

                if (!ignorePlayerWeaponUpdating)
                    player.RangedWeapon = (Weapon)currentRangedWeapon.Item;

                Notify("CurrentRangedWeapon");
            }
        }

        public InvItem CurrentImplement
        {
            get { return currentImplement; }
            set
            {
                currentImplement = value;

                if (!ignorePlayerWeaponUpdating)
                    player.Implement = (Weapon)currentImplement.Item;

                Notify("CurrentImplement");
            }
        }

        public ObservableCollectionEx<IInventoryItem> Inventory
        {
            get { return inventory; }
        }

        public ObservableCollection<InvItem> MainWeapons
        {
            get { return mainWeapons; }
        }

        public ObservableCollection<InvItem> OffhandWeapons
        {
            get { return offhandWeapons; }
        }

        public ObservableCollection<InvItem> RangedWeapons
        {
            get { return rangedWeapons; }
        }

        public ObservableCollection<InvItem> Implements
        {
            get { return implements; }
        }

        public ObservableCollection<InvItem> Armors
        {
            get { return armors; }
        }

        public ObservableCollection<InvItem> Shields
        {
            get { return shields; }
        }

        public bool IsNew
        {
            get { return isNew; }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            player.Picture = Common.ConvertToBitmap(characterImage);
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void btnGender_Click(object sender, RoutedEventArgs e)
        {
            player.IsMale = !player.IsMale;
        }

        private void btnRace_Click(object sender, RoutedEventArgs e)
        {
            EditCharacterRaceWindow window = new EditCharacterRaceWindow(player.Race);

            if (window.ShowDialog(this))
            {
                if (player.Race == null)
                    player.Race = window.Race;
                else
                    player.Race.CopyValues(window.Race);
            }
        }

        private void btnClass_Click(object sender, RoutedEventArgs e)
        {
            EditCharacterClassWindow window = new EditCharacterClassWindow(player.Class);

            if (window.ShowDialog(this))
            {
                if (player.Class == null)
                    player.Class = window.Class;
                else
                    player.Class.CopyValues(window.Class);
            }
        }

        private void btnLevel_Click(object sender, RoutedEventArgs e)
        {
            EditCharacterLevelWindow window = new EditCharacterLevelWindow(player.Level);

            if (window.ShowDialog(this))
                player.Level = window.Level;
        }

        private void btnAcDefense_Click(object sender, RoutedEventArgs e)
        {
            EditDefenseWindow window = new EditDefenseWindow(player, DefenseType.AC);

            window.ShowDialog(this);
        }

        private void btnFortDefense_Click(object sender, RoutedEventArgs e)
        {
            EditDefenseWindow window = new EditDefenseWindow(player, DefenseType.Fortitude);

            window.ShowDialog(this);
        }

        private void btnReflexDefense_Click(object sender, RoutedEventArgs e)
        {
            EditDefenseWindow window = new EditDefenseWindow(player, DefenseType.Reflex);

            window.ShowDialog(this);
        }

        private void btnWillDefense_Click(object sender, RoutedEventArgs e)
        {
            EditDefenseWindow window = new EditDefenseWindow(player, DefenseType.Will);

            window.ShowDialog(this);
        }

        private void btnHitPoints_Click(object sender, RoutedEventArgs e)
        {
            EditHitPointsWindow window = new EditHitPointsWindow(player);

            window.ShowDialog(this);
        }

        private void btnSurgeValue_Click(object sender, RoutedEventArgs e)
        {
            EditSurgeValueWindow window = new EditSurgeValueWindow(player);

            window.ShowDialog(this);
        }

        private void btnInitiative_Click(object sender, RoutedEventArgs e)
        {
            EditInitiativeWindow window = new EditInitiativeWindow(player);

            window.ShowDialog(this);
        }

        private void btnSpeed_Click(object sender, RoutedEventArgs e)
        {
            EditSpeedWindow window = new EditSpeedWindow(player);

            window.ShowDialog(this);
        }

        private void btnMainWeaponSpec_Click(object sender, RoutedEventArgs e)
        {
            if (player.Weapon == null)
                return;

            EditWeaponSpecWindow window = new EditWeaponSpecWindow(player, player.WeaponSpec);

            window.ShowDialog(this);

            BindingOperations.GetMultiBindingExpression(txtWeaponSpec, TextBlock.TextProperty).UpdateTarget();
        }

        private void btnOffhandWeaponSpec_Click(object sender, RoutedEventArgs e)
        {
            if (player.WeaponOffhand == null)
                return;

            EditWeaponSpecWindow window = new EditWeaponSpecWindow(player, player.WeaponOffhandSpec);

            window.ShowDialog(this);

            BindingOperations.GetMultiBindingExpression(txtOffhandWeaponSpec, TextBlock.TextProperty).UpdateTarget();
        }

        private void btnRangedWeaponSpec_Click(object sender, RoutedEventArgs e)
        {
            if (player.RangedWeapon == null)
                return;

            EditWeaponSpecWindow window = new EditWeaponSpecWindow(player, player.RangedWeaponSpec);

            window.ShowDialog(this);

            BindingOperations.GetMultiBindingExpression(txtRangedWeaponSpec, TextBlock.TextProperty).UpdateTarget();
        }

        private void btnInvImplement_Click(object sender, RoutedEventArgs e)
        {
            if (player.Implement == null)
                return;

            EditWeaponSpecWindow window = new EditWeaponSpecWindow(player, player.ImplementSpec);

            window.ShowDialog(this);
        }

        private void btnAddResistance_Click(object sender, RoutedEventArgs e)
        {
            AddResistanceValueWindow window = new AddResistanceValueWindow(null);

            if (window.ShowDialog(Application.Current.MainWindow))
            {
                player.Resistances.Add(window.Resistance);
            }
        }

        private void btnEditResistance_Click(object sender, RoutedEventArgs e)
        {
            EditSelectedResistance();
        }

        private void EditSelectedResistance()
        {
            ResistanceValue resistance = (lvResistances.SelectedItem as ResistanceValue);

            if (resistance == null)
                return;

            AddResistanceValueWindow window = new AddResistanceValueWindow(resistance);

            if (window.ShowDialog(Application.Current.MainWindow))
            {
                resistance.Modifier = window.Resistance.Modifier;
                resistance.DamageType = window.Resistance.DamageType;
                resistance.Description = window.Resistance.Description;
            }
        }

        private void btnDeleteResistance_Click(object sender, RoutedEventArgs e)
        {
            ResistanceValue resistance = (lvResistances.SelectedItem as ResistanceValue);

            if (resistance == null)
                return;

            if (MessageBox.Show("Are you sure you want to delete this resistance value?", "Delete resistance?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                player.Resistances.Remove(resistance);
            }
        }

        #region INotifyPropertyChanged Members

        private void Notify(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void lvResistances_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditSelectedResistance();
        }

        private void btnSkill_Click(object sender, RoutedEventArgs e)
        {
            SkillValue skill = ((sender as Button).Tag as SkillValue);

            if (skill == null)
                return;

            EditSkillValueWindow window = new EditSkillValueWindow(skill);

            window.ShowDialog(this);
        }

        private void btnSurgesPerDay_Click(object sender, RoutedEventArgs e)
        {
            EditSurgesPerDayWindow window = new EditSurgesPerDayWindow(player);

            window.ShowDialog(this);
        }

        private void lvInventory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            IInventoryItem item = (lvInventory.SelectedItem as IInventoryItem);

            if (item == null)
                return;

            EditInventoryItem(item);
        }

        private void EditInventoryItem(IInventoryItem item)
        {
            if (item is Weapon)
            {
                Weapon weapon = (Weapon)item;
                EditWeaponWindow window1 = new EditWeaponWindow(player, weapon, true);

                if (window1.ShowDialog(this))
                {
                    weapon.CopyValues(window1.Weapon);
                    player.WeaponBonuses[weapon] = new WeaponBonusValue(window1.ToHitAdjustments, window1.DamageAdjustments);
                }
            }
            else if (item is Armor)
            {
                Armor armor = (Armor)item;
                EditArmorWindow window2 = new EditArmorWindow(armor);

                if (window2.ShowDialog(this))
                {
                    armor.CopyValues(window2.Armor);
                }
            }
            else if (item is Shield)
            {
                Shield shield = (Shield)item;
                EditShieldWindow window3 = new EditShieldWindow(shield);

                if (window3.ShowDialog(this))
                {
                    shield.CopyValues(window3.Shield);
                }
            }
        }

        private void btnAddInventory_Click(object sender, RoutedEventArgs e)
        {
            AddInventoryItemWindow window = new AddInventoryItemWindow();

            if (window.ShowDialog(this))
            {
                if (window.SelectedItem != null)
                    player.Inventory.Add(window.SelectedItem);

                if (window.EditItem)
                    EditInventoryItem(window.SelectedItem);
            }
        }

        private void btnEditInventory_Click(object sender, RoutedEventArgs e)
        {
            if (lvInventory.SelectedItems.Count > 1)
                return;

            IInventoryItem item = (lvInventory.SelectedItem as IInventoryItem);

            if (item == null)
                return;

            EditInventoryItem(item);
        }

        private void btnDeleteInventory_Click(object sender, RoutedEventArgs e)
        {
            if (lvInventory.SelectedItems.Count == 0)
                return;

            string message;

            if (lvInventory.SelectedItems.Count == 1)
                message = String.Format("Are you sure that you want to delete this item ({0})?", (lvInventory.SelectedItem as IInventoryItem).Name);
            else
                message = String.Format("Are you sure that you want to delete the selected {0} items?", lvInventory.SelectedItems.Count.ToString());

            if (MessageBox.Show(message, "Delete inventory items", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                List<IInventoryItem> itemsToDelete = new List<IInventoryItem>(new ListAdapter<IInventoryItem>(lvInventory.SelectedItems));

                foreach (IInventoryItem item in itemsToDelete)
                {
                    player.Inventory.Remove(item);
                }
            }
        }

        private void btnInvWeaponMain_Click(object sender, RoutedEventArgs e)
        {
            if (player.Weapon == null)
                return;

            EditWeaponWindow window = new EditWeaponWindow(player, player.Weapon, false);

            if (window.ShowDialog(this))
            {
                player.Weapon.CopyValues(window.Weapon);
                player.WeaponBonuses[player.Weapon].CopyValues(window.ToHitAdjustments, window.DamageAdjustments);
                BindingOperations.GetMultiBindingExpression(txtWeaponSpec, TextBlock.TextProperty).UpdateTarget();
            }
        }

        private void btnInvWeaponOffhand_Click(object sender, RoutedEventArgs e)
        {
            if (player.WeaponOffhand == null)
                return;

            EditWeaponWindow window = new EditWeaponWindow(player, player.WeaponOffhand, false);

            if (window.ShowDialog(this))
            {
                player.WeaponOffhand.CopyValues(window.Weapon);
                player.WeaponBonuses[player.WeaponOffhand].CopyValues(window.ToHitAdjustments, window.DamageAdjustments);
                BindingOperations.GetMultiBindingExpression(txtOffhandWeaponSpec, TextBlock.TextProperty).UpdateTarget();
            }
        }

        private void btnInvRangedWeapon_Click(object sender, RoutedEventArgs e)
        {
            if (player.RangedWeapon == null)
                return;

            EditWeaponWindow window = new EditWeaponWindow(player, player.RangedWeapon, false);

            if (window.ShowDialog(this))
            {
                player.RangedWeapon.CopyValues(window.Weapon);
                player.WeaponBonuses[player.RangedWeapon].CopyValues(window.ToHitAdjustments, window.DamageAdjustments);
                BindingOperations.GetMultiBindingExpression(txtRangedWeaponSpec, TextBlock.TextProperty).UpdateTarget();
            }
        }

        private void btnInvArmor_Click(object sender, RoutedEventArgs e)
        {
            if (player.Armor == null)
                return;

            EditArmorWindow window = new EditArmorWindow(player.Armor);

            if (window.ShowDialog(this))
            {
                player.Armor.CopyValues(window.Armor);
            }
        }

        private void btnInvShield_Click(object sender, RoutedEventArgs e)
        {
            if (player.Shield == null)
                return;

            EditShieldWindow window = new EditShieldWindow(player.Shield);

            if (window.ShowDialog(this))
            {
                player.Shield.CopyValues(window.Shield);
            }
        }
    }
}
