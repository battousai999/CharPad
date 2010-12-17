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
        private List<Weapon> weapons;
        private List<IInventoryItem> armors;
        private List<InventoryItem> inventoryItems;

        public AddInventoryItemWindow()
        {
            BuildItemLists();

            InitializeComponent();
        }

        public List<Weapon> Weapons
        {
            get { return weapons; }
        }

        public List<IInventoryItem> Armors
        {
            get { return armors; }
        }

        public List<InventoryItem> InventoryItems
        {
            get { return inventoryItems; }
        }

        private void BuildItemLists()
        {
            weapons = new List<Weapon>();


        }


    }
}
