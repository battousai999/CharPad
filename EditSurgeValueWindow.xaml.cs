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
    /// Interaction logic for EditSurgeValueWindow.xaml
    /// </summary>
    public partial class EditSurgeValueWindow : Window
    {
        private Player player;

        public EditSurgeValueWindow(Player player)
        {
            this.player = player;

            InitializeComponent();
        }

        public Player Player
        {
            get { return player; }
        }

        public SurgeValue SurgeValue
        {
            get { return player.SurgeValue; }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
