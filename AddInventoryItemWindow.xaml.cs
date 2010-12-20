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

namespace CharPad
{
    /// <summary>
    /// Interaction logic for AddInventoryItemWindow.xaml
    /// </summary>
    public partial class AddInventoryItemWindow : Window
    {
        #region Classes

        public class ArmorItem
        {
            public IInventoryItem Item { get; set; }
            public string ArmorGroup { get; set; }
        }

        #endregion

        private List<Weapon> weapons;
        private List<ArmorItem> armors;
        private List<InventoryItem> inventoryItems;
        private IInventoryItem selectedItem;

        public AddInventoryItemWindow()
        {
            BuildItemLists();

            InitializeComponent();
        }

        public List<Weapon> Weapons
        {
            get { return weapons; }
        }

        public List<ArmorItem> Armors
        {
            get { return armors; }
        }

        public List<InventoryItem> InventoryItems
        {
            get { return inventoryItems; }
        }

        public IInventoryItem SelectedItem
        {
            get { return selectedItem; }
        }

        public bool EditItem
        {
            get { return chkEditItem.IsChecked.Value; }
        }

        private void BuildItemLists()
        {
            weapons = new List<Weapon>(WeaponDefinitions.Weapons);

            inventoryItems = new List<InventoryItem>();
            inventoryItems.Add(new InventoryItem { Name = "Backpack", IsStackable = false });
            inventoryItems.Add(new InventoryItem { Name = "Torch", IsStackable = true });

            armors = new List<ArmorItem>();

            armors.AddRange(ArmorDefinitions.NormalArmors.ConvertAll(x => new ArmorItem { Item = x, ArmorGroup = "Normal Armors" }));
            armors.AddRange(ArmorDefinitions.Shields.ConvertAll(x => new ArmorItem { Item = x, ArmorGroup = "Shields" }));
            armors.AddRange(ArmorDefinitions.SpecialArmors.ConvertAll(x => new ArmorItem { Item = x, ArmorGroup = "Special Armors" }));
        }

        private void btnSelect_Click(object sender, RoutedEventArgs args)
        {
            HandleSelection();
        }

        private void HandleSelection()
        {
            if (mainTabControl.SelectedItem == null)
                return;

            ListView listview = ((ListView)((TabItem)mainTabControl.SelectedItem).Content);

            if (listview.SelectedItem == null)
                return;

            selectedItem = (listview.SelectedItem is ArmorItem ? ((ArmorItem)listview.SelectedItem).Item : (IInventoryItem)listview.SelectedItem);

            DialogResult = true;
        }

        private void listview_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            HandleSelection();
        }
    }
}
