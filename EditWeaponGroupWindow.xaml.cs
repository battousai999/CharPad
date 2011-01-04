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
    /// Interaction logic for EditWeaponGroupWindow.xaml
    /// </summary>
    public partial class EditWeaponGroupWindow : Window
    {
        private WeaponGroup weaponGroup;

        public EditWeaponGroupWindow(WeaponGroup weaponGroup)
        {
            InitializeComponent();

            WeaponGroup[] groups = (WeaponGroup[])Enum.GetValues(typeof(WeaponGroup));

            for (int i = 0; i < groups.Length; i++)
            {
                gridGroups.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            gridGroups.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            gridGroups.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            for (int i = 0; i < groups.Length; i++)
            {
                WeaponGroup group = groups[i];

                CheckBox checkbox = new CheckBox { Content = Utility.GetWeaponGroupName(group), Tag = group, Margin = new Thickness(0, 5, 0, 0), MinWidth = 150 };
                Grid.SetRow(checkbox, i / 2);
                Grid.SetColumn(checkbox, i % 2);

                if ((weaponGroup & group) == group)
                    checkbox.IsChecked = true;

                gridGroups.Children.Add(checkbox);
            }
        }

        public WeaponGroup WeaponGroup
        {
            get { return weaponGroup; }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            weaponGroup = 0;

            foreach (UIElement element in gridGroups.Children)
            {
                if (!(element is CheckBox))
                    continue;

                CheckBox checkbox = (CheckBox)element;

                if (checkbox.IsChecked == true)
                    weaponGroup |= (WeaponGroup)checkbox.Tag;
            }

            DialogResult = true;
        }
    }
}
