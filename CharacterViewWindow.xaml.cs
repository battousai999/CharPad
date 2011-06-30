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

            Title = String.Format("{0}{1} [Level {2}, {3} {4}{5}{6}]",
                player.CharacterName,
                (String.IsNullOrEmpty(player.PlayerName) ? "" : " (" + player.PlayerName + ")"),
                player.Level.ToString(),
                player.Race.Name,
                player.Class.Name,
                (String.IsNullOrEmpty(player.ParagonPath) ? "" : " (" + player.ParagonPath + ")"),
                (String.IsNullOrEmpty(player.EpicDestiny) ? "" : " (" + player.EpicDestiny + ")"));
        }

        public Player Player
        {
            get { return player; }
        }
    }
}
