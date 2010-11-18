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

namespace CharPad
{
    /// <summary>
    /// Interaction logic for CharacterWindow.xaml
    /// </summary>
    public partial class CharacterWindow : Window
    {
        private Player player;
        private bool isNew;

        public CharacterWindow(Player player, bool isNew)
        {
            this.player = player;
            this.isNew = isNew;

            InitializeComponent();

            mainGrid.DataContext = player;

            if (isNew)
                this.Title = "New Character";
            else
                this.SetBinding(Window.TitleProperty, new Binding { Source = player, Path = new PropertyPath("CharacterName"), StringFormat = "Edit {0}" });
        }

        public bool IsNew
        {
            get { return isNew; }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void btnGender_Click(object sender, RoutedEventArgs e)
        {
            player.IsMale = !player.IsMale;
        }

        private void btnRace_Click(object sender, RoutedEventArgs e)
        {
            EditCharacterRaceWindow window = new EditCharacterRaceWindow(player.Race);

            if (window.ShowDialog(this))
                player.Race.CopyValues(window.Race);
        }

        private void btnClass_Click(object sender, RoutedEventArgs e)
        {
            EditCharacterClassWindow window = new EditCharacterClassWindow(player.Class);

            if (window.ShowDialog(this))
                player.Class.CopyValues(window.Class);
        }

        private void btnLevel_Click(object sender, RoutedEventArgs e)
        {
            EditCharacterLevelWindow window = new EditCharacterLevelWindow(player.Level);

            if (window.ShowDialog(this))
                player.Level = window.Level;
        }
    }
}
