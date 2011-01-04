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
    /// Interaction logic for EditWeaponPropertiesWindow.xaml
    /// </summary>
    public partial class EditWeaponPropertiesWindow : Window
    {
        private WeaponProperties properties;

        public EditWeaponPropertiesWindow(WeaponProperties properties)
        {
            InitializeComponent();

            WeaponProperties[] props = (WeaponProperties[])Enum.GetValues(typeof(WeaponProperties));
            props = props.Where(x => x != WeaponProperties.None).ToArray();

            for (int i = 0; i < props.Length; i++)
            {
                gridProperties.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            gridProperties.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            gridProperties.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            for (int i = 0; i < props.Length; i++)
            {
                WeaponProperties property = props[i];

                CheckBox checkbox = new CheckBox { Content = Utility.GetWeaponPropertyName(property), Tag = property, Margin = new Thickness(0, 5, 0, 0), MinWidth = 150 };
                Grid.SetRow(checkbox, i / 2);
                Grid.SetColumn(checkbox, i % 2);

                if ((properties & property) == property)
                    checkbox.IsChecked = true;

                gridProperties.Children.Add(checkbox);
            }
        }

        public WeaponProperties Properties
        {
            get { return properties; }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            properties = 0;

            foreach (UIElement element in gridProperties.Children)
            {
                if (!(element is CheckBox))
                    continue;

                CheckBox checkbox = (CheckBox)element;

                if (checkbox.IsChecked == true)
                    properties |= (WeaponProperties)checkbox.Tag;
            }

            DialogResult = true;
        }
    }
}
