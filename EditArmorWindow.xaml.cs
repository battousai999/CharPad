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
using System.IO;

namespace CharPad
{
    /// <summary>
    /// Interaction logic for EditArmorWindow.xaml
    /// </summary>
    public partial class EditArmorWindow : Window, INotifyPropertyChanged
    {
        #region Classes

        public class ArmorTypeItem
        {
            public string Name { get; set; }
            public ArmorType ArmorType { get; set; }

            public ArmorTypeItem(ArmorType armorType)
            {
                this.ArmorType = armorType;
                this.Name = Utility.GetArmorTypeName(armorType);
            }
        }

        #endregion

        private Armor armor;
        private List<ArmorTypeItem> armorTypes;
        private BitmapSource armorImage;

        public EditArmorWindow(Armor armor)
        {
            armorTypes = new List<ArmorTypeItem>();
            armorTypes.Add(new ArmorTypeItem(ArmorType.Cloth));
            armorTypes.Add(new ArmorTypeItem(ArmorType.Leather));
            armorTypes.Add(new ArmorTypeItem(ArmorType.Hide));
            armorTypes.Add(new ArmorTypeItem(ArmorType.Chainmail));
            armorTypes.Add(new ArmorTypeItem(ArmorType.Scale));
            armorTypes.Add(new ArmorTypeItem(ArmorType.Plate));

            InitializeComponent();

            txtName.Text = armor.Name;
            cboArmorType.SelectedItem = armorTypes.Find(x => x.ArmorType == armor.ArmorType);
            txtArmorBonus.Text = armor.ArmorBonus.ToString();
            txtEnhancementBonus.Text = armor.EnhancementBonus.ToString();
            txtSkillModifier.Text = armor.SkillModifier.ToString();
            txtSpeedModifier.Text = armor.SpeedModifier.ToString();
            txtPrice.Text = armor.BasePrice.ToString();
            chkIsHeavy.IsChecked = armor.IsHeavy;
            txtSpecialProperty.Text = armor.SpecialProperty;
            txtMinEnhanceBonus.Text = armor.MinEnhancementBonus.ToString();
            txtNotes.Text = armor.Notes;
            ArmorImage = Common.BuildBitmapImage(armor.Picture);
        }

        public BitmapSource ArmorImage
        {
            get { return armorImage; }
            set { armorImage = value; Notify("ArmorImage"); }
        }

        public Armor Armor
        {
            get { return armor; }
        }

        public List<ArmorTypeItem> ArmorTypes
        {
            get { return armorTypes; }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter an armor name.", "Invalid name", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtName.Focus();
                return;
            }

            int armorBonus;
            int enhancementBonus;
            int skillModifier;
            int speedModifier;
            int price;
            int minEnhanceBonus;

            if (String.IsNullOrWhiteSpace(txtArmorBonus.Text))
                armorBonus = 0;
            else if (!Int32.TryParse(txtArmorBonus.Text, out armorBonus))
            {
                MessageBox.Show("Please enter either a valid armor bonus or a blank armor bonus.", "Invalid armor bonus", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtArmorBonus.Focus();
                return;
            }

            if (String.IsNullOrWhiteSpace(txtEnhancementBonus.Text))
                enhancementBonus = 0;
            else if (!Int32.TryParse(txtEnhancementBonus.Text, out enhancementBonus))
            {
                MessageBox.Show("Please enter either a valid enhancement bonus or a blank enhancement bonus.", "Invalid enhancement bonus", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEnhancementBonus.Focus();
                return;
            }

            if (String.IsNullOrWhiteSpace(txtSkillModifier.Text))
                skillModifier = 0;
            else if (!Int32.TryParse(txtSkillModifier.Text, out skillModifier))
            {
                MessageBox.Show("Please enter either a valid skill modifier or a blank skill modifier.", "Invalid skill modifier", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtSkillModifier.Focus();
                return;
            }

            if (String.IsNullOrWhiteSpace(txtSpeedModifier.Text))
                speedModifier = 0;
            else if (!Int32.TryParse(txtSpeedModifier.Text, out speedModifier))
            {
                MessageBox.Show("Please enter either a valid speed modifier or a blank speed modifier.", "Invalid speed modifier", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtSpeedModifier.Focus();
                return;
            }

            if (String.IsNullOrWhiteSpace(txtPrice.Text))
                price = 0;
            else if (!Int32.TryParse(txtPrice.Text, out price))
            {
                MessageBox.Show("Please enter either a valid price or a blank price.", "Invalid price", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPrice.Focus();
                return;
            }

            if (String.IsNullOrWhiteSpace(txtMinEnhanceBonus.Text))
                minEnhanceBonus = 0;
            else if (!Int32.TryParse(txtMinEnhanceBonus.Text, out minEnhanceBonus))
            {
                MessageBox.Show("Please enter either a valid minimum enhancement bonus or a blank minimum enhancement bonus.", "Invalid minimum enhancement bonus", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtMinEnhanceBonus.Focus();
                return;
            }

            armor = new Armor(txtName.Text,
                ((ArmorTypeItem)cboArmorType.SelectedItem).ArmorType,
                armorBonus,
                skillModifier,
                speedModifier,
                price,
                chkIsHeavy.IsChecked.Value,
                txtSpecialProperty.Text,
                minEnhanceBonus,
                enhancementBonus);

            armor.Picture = Common.ConvertToBitmap(armorImage);
            armor.Notes = txtNotes.Text;

            DialogResult = true;
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
