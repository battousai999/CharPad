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
using System.ComponentModel;
using CharPad.Framework;
using System.IO;

namespace CharPad
{
    /// <summary>
    /// Interaction logic for EditShieldWindow.xaml
    /// </summary>
    public partial class EditShieldWindow : Window, INotifyPropertyChanged
    {
        private Shield shield;
        private BitmapSource shieldImage;

        public EditShieldWindow(Shield shield)
        {
            InitializeComponent();

            txtName.Text = shield.Name;
            chkIsHeavy.IsChecked = (shield.ArmorType == ArmorType.HeavyShield);
            txtArmorBonus.Text = shield.ArmorBonus.ToString();
            txtEnhancementBonus.Text = shield.EnhancementBonus.ToString();
            txtSkillModifier.Text = shield.SkillModifier.ToString();
            txtPrice.Text = shield.BasePrice.ToString();
            txtNotes.Text = shield.Notes;
            ShieldImage = Common.BuildBitmapImage(shield.Picture);
        }

        public BitmapSource ShieldImage
        {
            get { return shieldImage; }
            set { shieldImage = value; Notify("ShieldImage"); }
        }

        public Shield Shield
        {
            get { return shield; }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a shield name.", "Invalid name", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtName.Focus();
                return;
            }

            int armorBonus;
            int enhancementBonus;
            int skillModifier;
            int price;

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

            if (String.IsNullOrWhiteSpace(txtPrice.Text))
                price = 0;
            else if (!Int32.TryParse(txtPrice.Text, out price))
            {
                MessageBox.Show("Please enter either a valid price or a blank price.", "Invalid price", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPrice.Focus();
                return;
            }

            shield = new Shield(txtName.Text,
                (chkIsHeavy.IsChecked.Value ? ArmorType.HeavyShield : ArmorType.LightShield),
                armorBonus,
                skillModifier,
                price,
                enhancementBonus);

            shield.Picture = Common.ConvertToBitmap(shieldImage);
            shield.Notes = txtNotes.Text;

            DialogResult = true;
        }

        #region INotifyPropertyChanged Members

        private void Notify(string propertyChanged)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyChanged));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
