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
    /// Interaction logic for EditWeaponSpecWindow.xaml
    /// </summary>
    public partial class EditWeaponSpecWindow : Window
    {
        WeaponSpecValue weaponSpec;

        public EditWeaponSpecWindow(WeaponSpecValue weaponSpec)
        {
            this.weaponSpec = weaponSpec;

            InitializeComponent();
        }

        public WeaponSpecValue WeaponSpec
        {
            get { return weaponSpec; }
        }

        public Weapon Weapon
        {
            get { return weaponSpec.Weapon; }
        }

        public BasicAdjustmentList GeneralToHitAdjustments
        {
            get { return weaponSpec.ToHitAdjustments; }
        }

        public BasicAdjustmentList SpecificToHitAdjustments
        {
            get { return weaponSpec.WeaponSpecificToHitAdjustments; }
        }

        public BasicAdjustmentList GeneralDamageAdjustments
        {
            get { return weaponSpec.DamageAdjustments; }
        }

        public BasicAdjustmentList SpecificDamageAdjustments
        {
            get { return weaponSpec.WeaponSpecificDamageAdjustments; }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
