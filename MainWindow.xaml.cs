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
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CharPad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public static RoutedCommand NewPartyCommand = new RoutedCommand();
        public static RoutedCommand NewCharacterCommand = new RoutedCommand();
        public static RoutedCommand EditCharacterCommand = new RoutedCommand();

        private Party party;

        public MainWindow()
        {
            Party = new Party();

            InitializeTestParty();

            InitializeComponent();

            this.CommandBindings.Add(new CommandBinding(NewPartyCommand, new ExecutedRoutedEventHandler(NewPartyCommand_Executed)));
            this.CommandBindings.Add(new CommandBinding(NewCharacterCommand, new ExecutedRoutedEventHandler(NewCharacterCommand_Executed)));
            this.CommandBindings.Add(new CommandBinding(EditCharacterCommand, new ExecutedRoutedEventHandler(EditCharacterCommand_Executed)));

            charPanel.ItemsSource = party.Members;
        }

        public Party Party
        {
            get { return party; }
            set { party = value; Notify("Party"); }
        }

        private void NewPartyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Party.IsDirty)
            {
                // TODO: Add logic for saving a dirty party before clearing it...
            }

            Party.Members.Clear();
        }

        private void NewCharacterCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Player player = new Player();
            CharacterWindow window = new CharacterWindow(player, true);

            if (window.ShowDialog() == true)
            {
                Party.Members.Add(player);
            }
        }

        private void EditCharacterCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Button button = (e.OriginalSource as Button);

            if ((button == null) || (button.Tag == null))
                return;

            Player player = (button.Tag as Player);
            CharacterWindow window = new CharacterWindow(player, false);

            window.ShowDialog();
        }

        private void InitializeTestParty()
        {
            Party.Members.Clear();

            Player player = new Player();
            player.CharacterName = "Telegis";
            player.Race = new PlayerRace { Name = "Elf", Size = CreatureSize.Meduim, BaseSpeed = 7 };
            player.Class = new PlayerClass { Name = "Warrior", BaseHealth = 12, BaseHealingSurges = 7, HealthPerLevel = 5, FortitudeBonus = 2, ReflexBonus = 0, WillBonus = 0 };
            player.IsMale = true;
            player.Level = 1;
            player.Str = 18;
            player.Con = 16;
            player.Dex = 14;
            player.Int = 8;
            player.Wis = 14;
            player.Cha = 10;
            player.SurgesPerDay = 7;

            Party.Members.Add(player);

            player = new Player();
            player.CharacterName = "Velyss";
            player.Race = new PlayerRace { Name = "Elf", Size = CreatureSize.Meduim, BaseSpeed = 7 };
            player.Class = new PlayerClass { Name = "Warrior", BaseHealth = 12, BaseHealingSurges = 7, HealthPerLevel = 5, FortitudeBonus = 2, ReflexBonus = 0, WillBonus = 0 };
            player.IsMale = true;
            player.Level = 1;
            player.Str = 18;
            player.Con = 16;
            player.Dex = 14;
            player.Int = 8;
            player.Wis = 14;
            player.Cha = 10;
            player.SurgesPerDay = 7;

            Party.Members.Add(player);

            player = new Player();
            player.CharacterName = "Kharzhur";
            player.Race = new PlayerRace { Name = "Elf", Size = CreatureSize.Meduim, BaseSpeed = 7 };
            player.Class = new PlayerClass { Name = "Warrior", BaseHealth = 12, BaseHealingSurges = 7, HealthPerLevel = 5, FortitudeBonus = 2, ReflexBonus = 0, WillBonus = 0 };
            player.IsMale = true;
            player.Level = 1;
            player.Str = 18;
            player.Con = 16;
            player.Dex = 14;
            player.Int = 8;
            player.Wis = 14;
            player.Cha = 10;
            player.SurgesPerDay = 7;

            Party.Members.Add(player);
        }

        private void Notify(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
