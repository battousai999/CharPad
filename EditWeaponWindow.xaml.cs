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
using System.Text.RegularExpressions;
using System.IO;

namespace CharPad
{
    /// <summary>
    /// Interaction logic for EditWeaponWindow.xaml
    /// </summary>
    public partial class EditWeaponWindow : Window, INotifyPropertyChanged
    {
        #region Classes

        public class CategoryItem
        {
            public string Name { get; set; }
            public WeaponCategory Category { get; set; }

            public CategoryItem(WeaponCategory category)
            {
                this.Category = category;
                this.Name = Utility.GetWeaponCategoryName(category);
            }
        }

        #endregion

        private static Regex damageRegex = new Regex(@"^\s*(\d{0,2})\s*d\s*(\d{1,3})\s*$", RegexOptions.IgnoreCase);

        private Weapon originalWeapon;
        private Weapon weapon;
        private WeaponGroup weaponGroup;
        private WeaponProperties properties;
        private List<CategoryItem> weaponCategories;
        private BitmapSource weaponImage;
        private BasicAdjustmentList toHitAdjustments;
        private BasicAdjustmentList damageAdjustments;

        public EditWeaponWindow(Player player, Weapon weapon, bool allowImplementSwitch)
        {
            this.originalWeapon = weapon;

            weaponCategories = new List<CategoryItem>();

            if ((weapon.Category == WeaponCategory.PureImplement) || allowImplementSwitch)
                weaponCategories.Add(new CategoryItem(WeaponCategory.PureImplement));

            if ((weapon.Category != WeaponCategory.PureImplement) || allowImplementSwitch)
            {
                weaponCategories.Add(new CategoryItem(WeaponCategory.SimpleMelee));
                weaponCategories.Add(new CategoryItem(WeaponCategory.SimpleRanged));
                weaponCategories.Add(new CategoryItem(WeaponCategory.MilitaryMelee));
                weaponCategories.Add(new CategoryItem(WeaponCategory.MilitaryRanged));
                weaponCategories.Add(new CategoryItem(WeaponCategory.SuperiorMelee));
                weaponCategories.Add(new CategoryItem(WeaponCategory.SuperiorRanged));
            }

            InitializeComponent();

            txtName.Text = weapon.Name;
            txtProficiencyBonus.Text = weapon.ProficiencyBonus.ToString();
            txtEnhancementBonus.Text = weapon.EnhancementBonus.ToString();
            txtDamage.Text = (weapon.Damage == null ? "" : weapon.Damage.DisplayString);
            txtRange.Text = weapon.Range;
            WeaponGroup = weapon.Group;
            Properties = weapon.Properties;
            txtPrice.Text = weapon.BasePrice.ToString();
            cboCategory.SelectedItem = weaponCategories.Find(x => x.Category == weapon.Category);
            chkIsTwoHanded.IsChecked = weapon.IsTwoHanded;
            txtNotes.Text = (weapon.Notes == null ? "" : weapon.Notes);
            chkIsImplement.IsChecked = weapon.IsImplement;
            WeaponImage = Common.BuildBitmapImage(weapon.Picture);

            WeaponBonusValue weaponBonus = player.WeaponBonuses[weapon];

            if (weaponBonus == null)
            {
                weaponBonus = new WeaponBonusValue();
                player.WeaponBonuses.Add(weapon, weaponBonus);
            }

            ToHitAdjustments = new BasicAdjustmentList();

            foreach (BasicAdjustment adjustment in weaponBonus.ToHitAdjustments)
            {
                ToHitAdjustments.Add(new BasicAdjustment(adjustment.Modifier, adjustment.Note));
            }

            DamageAdjustments = new BasicAdjustmentList();

            foreach (BasicAdjustment adjustment in weaponBonus.DamageAdjustments)
            {
                DamageAdjustments.Add(new BasicAdjustment(adjustment.Modifier, adjustment.Note));
            }
        }

        public Weapon Weapon
        {
            get { return weapon; }
        }

        public WeaponGroup WeaponGroup
        {
            get { return weaponGroup; }
            set { weaponGroup = value; Notify("WeaponGroup"); }
        }

        public WeaponProperties Properties
        {
            get { return properties; }
            set { properties = value; Notify("Properties"); }
        }

        public List<CategoryItem> WeaponCategories
        {
            get { return weaponCategories; }
        }

        public BitmapSource WeaponImage
        {
            get { return weaponImage; }
            set { weaponImage = value; Notify("WeaponImage"); }
        }

        public BasicAdjustmentList ToHitAdjustments
        {
            get { return toHitAdjustments; }
            set { toHitAdjustments = value; Notify("ToHitAdjustments"); }
        }

        public BasicAdjustmentList DamageAdjustments
        {
            get { return damageAdjustments; }
            set { damageAdjustments = value; Notify("DamageAdjustments"); }
        }

        #region INotifyPropertyChanged Members

        private void Notify(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a weapon name.", "Invalid name", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtName.Focus();
                return;
            }

            int price;
            int enhancementBonus;

            if (String.IsNullOrWhiteSpace(txtPrice.Text))
                price = 0;
            else if (!Int32.TryParse(txtPrice.Text, out price))
            {
                MessageBox.Show("Please enter either a valid price or a blank price.", "Invalid price", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPrice.Focus();
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

            WeaponCategory weaponCategory = ((CategoryItem)cboCategory.SelectedItem).Category;

            if (weaponCategory == WeaponCategory.PureImplement)
            {
                weapon = Weapon.CreateImplement(txtName.Text, price);
            }
            else
            {
                int proficiencyBonus;

                if (String.IsNullOrWhiteSpace(txtProficiencyBonus.Text))
                    proficiencyBonus = 0;
                else if (!Int32.TryParse(txtProficiencyBonus.Text, out proficiencyBonus))
                {
                    MessageBox.Show("Please enter either a valid proficiency bonus or a blank proficiency bonus.", "Invalid proficiency bonus", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtProficiencyBonus.Focus();
                    return;
                }

                Dice damage = GetDamageValue(txtDamage.Text);

                if (damage == null)
                {
                    MessageBox.Show("Please enter a valid damage value.", "Invalid damage", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtDamage.Focus();
                    return;
                }

                weapon = new Weapon(txtName.Text,
                    proficiencyBonus,
                    damage,
                    txtRange.Text,
                    weaponGroup,
                    properties,
                    weaponCategory,
                    price,
                    chkIsTwoHanded.IsChecked.Value);

                weapon.IsImplement = chkIsImplement.IsChecked.Value;
            }

            weapon.EnhancementBonus = enhancementBonus;
            weapon.Picture = Common.ConvertToBitmap(weaponImage);
            weapon.Notes = txtNotes.Text;

            DialogResult = true;
        }

        private Dice GetDamageValue(string damageString)
        {
            Match match = damageRegex.Match(damageString);

            if (!match.Success)
                return null;

            int number;

            if (String.IsNullOrWhiteSpace(match.Groups[1].Value))
                number = 1;
            else
                number = Convert.ToInt32(match.Groups[1].Value);

            return Dice.Get(number, Convert.ToInt32(match.Groups[2].Value));
        }

        private void btnWeaponGroup_Click(object sender, RoutedEventArgs e)
        {
            EditWeaponGroupWindow window = new EditWeaponGroupWindow(WeaponGroup);

            if (window.ShowDialog(this))
            {
                WeaponGroup = window.WeaponGroup;
            }
        }

        private void btnProperties_Click(object sender, RoutedEventArgs e)
        {
            EditWeaponPropertiesWindow window = new EditWeaponPropertiesWindow(Properties);

            if (window.ShowDialog(this))
            {
                Properties = window.Properties;
            }
        }
    }
}
