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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CharPad.Framework;

namespace CharPad
{
    /// <summary>
    /// Interaction logic for PowerGrid.xaml
    /// </summary>
    public partial class PowerGrid : UserControl
    {
        public PowerGrid()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ListProperty =
            DependencyProperty.Register("List", typeof(List<Power>), typeof(PowerGrid), new UIPropertyMetadata(null));

        public static readonly DependencyProperty PlayerProperty =
            DependencyProperty.Register("Player", typeof(Player), typeof(PowerGrid), new UIPropertyMetadata(null));

        public static readonly DependencyProperty PowerTypeProperty =
            DependencyProperty.Register("PowerType", typeof(PowerType), typeof(PowerGrid), new UIPropertyMetadata(null));

        public List<Power> List
        {
            get { return (List<Power>)GetValue(ListProperty); }
            set { SetValue(ListProperty, value); }
        }

        public Player Player
        {
            get { return (Player)GetValue(PlayerProperty); }
            set { SetValue(PlayerProperty, value); }
        }

        public PowerType PowerType
        {
            get { return (PowerType)GetValue(PowerTypeProperty); }
            set { SetValue(PowerTypeProperty, value); }
        }

        private void lvMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditSelectedItem();
        }

        private void EditSelectedItem()
        {
            Power power = (lvMain.SelectedItem as Power);

            if (power == null)
                return;

            EditPowerWindow window = new EditPowerWindow(power);

            if (window.ShowDialog(Application.Current.MainWindow))
            {
                window.UpdatePower(power);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            EditPowerWindow window = new EditPowerWindow(null);

            window.SetPowerType(PowerType);

            if (window.ShowDialog(Application.Current.MainWindow))
            {
                Power power = new Power(Player);

                window.UpdatePower(power);

                Player.Powers.Add(power);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            EditSelectedItem();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Power power = (lvMain.SelectedItem as Power);

            if (power == null)
                return;

            if (MessageBox.Show("Are you sure you want to delete this power?", "Delete power?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                Player.Powers.Remove(power);
            }
        }
    }
}
