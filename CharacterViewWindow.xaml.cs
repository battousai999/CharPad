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
    /// Interaction logic for CharacterViewWindow.xaml
    /// </summary>
    public partial class CharacterViewWindow : Window
    {
        private Player player;

        public CharacterViewWindow(Player player)
        {
            this.player = player;

            InitializeComponent();

            gridMain.DataContext = player;
        }

        public Player Player
        {
            get { return player; }
        }
    }
}
