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
using Microsoft.Win32;

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
        public static RoutedCommand LoadPartyCommand = new RoutedCommand();
        public static RoutedCommand SavePartyCommand = new RoutedCommand();
        public static RoutedCommand SavePartyAsCommand = new RoutedCommand();
        public static RoutedCommand ExitCommand = new RoutedCommand();
        public static RoutedCommand LoadCharacterCommand = new RoutedCommand();
        public static RoutedCommand SaveCharacterCommand = new RoutedCommand();
        public static RoutedCommand RemoveCharacterCommand = new RoutedCommand();
        public static RoutedCommand ViewCharacterCommand = new RoutedCommand();

        private Party party;
        private string partyFilename;

        public MainWindow()
        {
            Party = new Party();

            //InitializeTestParty();

            InitializeComponent();

            this.CommandBindings.Add(new CommandBinding(NewPartyCommand, new ExecutedRoutedEventHandler(NewPartyCommand_Executed)));
            this.CommandBindings.Add(new CommandBinding(LoadPartyCommand, new ExecutedRoutedEventHandler(LoadPartyCommand_Executed)));
            this.CommandBindings.Add(new CommandBinding(SavePartyCommand, new ExecutedRoutedEventHandler(SavePartyCommand_Executed)));
            this.CommandBindings.Add(new CommandBinding(SavePartyAsCommand, new ExecutedRoutedEventHandler(SavePartyAsCommand_Executed)));
            this.CommandBindings.Add(new CommandBinding(NewCharacterCommand, new ExecutedRoutedEventHandler(NewCharacterCommand_Executed)));
            this.CommandBindings.Add(new CommandBinding(EditCharacterCommand, new ExecutedRoutedEventHandler(EditCharacterCommand_Executed)));
            this.CommandBindings.Add(new CommandBinding(LoadCharacterCommand, new ExecutedRoutedEventHandler(LoadCharacterCommand_Executed)));
            this.CommandBindings.Add(new CommandBinding(SaveCharacterCommand, new ExecutedRoutedEventHandler(SaveCharacterCommand_Executed)));
            this.CommandBindings.Add(new CommandBinding(RemoveCharacterCommand, new ExecutedRoutedEventHandler(RemoveCharacterCommand_Executed)));
            this.CommandBindings.Add(new CommandBinding(ExitCommand, new ExecutedRoutedEventHandler(ExitCommand_Executed)));
            this.CommandBindings.Add(new CommandBinding(ViewCharacterCommand, new ExecutedRoutedEventHandler(ViewCharacterCommand_Executed)));

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
            partyFilename = null;
        }

        private void NewCharacterCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Player player = new Player();
            CharacterWindow window = new CharacterWindow(player, true);

            if (window.ShowDialog(this))
            {
                Party.Members.Add(player);
            }
        }

        private void ViewCharacterCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Button button = (e.OriginalSource as Button);

            if ((button == null) || (button.Tag == null))
                return;

            Player player = (button.Tag as Player);
            CharacterViewWindow window = new CharacterViewWindow(player);

            window.ShowDialog(this);
        }

        private void EditCharacterCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Button button = (e.OriginalSource as Button);

            if ((button == null) || (button.Tag == null))
                return;

            Player player = (button.Tag as Player);
            CharacterWindow window = new CharacterWindow(player, false);
            window.RemoveCharacter += ((sender2, e2) => { Party.Members.Remove(player); });

            window.ShowDialog(this);
        }

        private void LoadPartyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Party.IsDirty)
            {
                // TODO: Add logic for saving a dirty party before loading a new one...
            }

            OpenFileDialog dialog = new OpenFileDialog();

            dialog.AddExtension = true;
            dialog.CheckFileExists = true;
            dialog.DefaultExt = ".cpd";
            dialog.Filter = "CharPad Data File|*.cpd|All Files|*.*";
            dialog.Multiselect = false;
            dialog.Title = "Open Party File";

            if (dialog.ShowDialog(this) == true)
            {
                Party tempParty;

                try
                {
                    tempParty = PlayerDataAdapter.LoadParty(dialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Error loading party: ({0}) {1}{2}{3}",
                        ex.GetType().Name,
                        ex.Message,
                        Environment.NewLine,
                        ex.StackTrace),
                        "Eror loading party",
                        MessageBoxButton.OK);

                    return;
                }

                Party.Members.Clear();

                foreach (Player player in tempParty.Members)
                {
                    Party.Members.Add(player);
                }

                partyFilename = dialog.FileName;
            }
        }

        private void SavePartyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(partyFilename))
                SavePartyAsCommand_Executed(sender, e);
            else
            {
                try
                {
                    PlayerDataAdapter.SaveParty(partyFilename, Party);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Error saving party: ({0}) {1}{2}{3}",
                        ex.GetType().Name,
                        ex.Message,
                        Environment.NewLine,
                        ex.StackTrace),
                        "Error saving party",
                        MessageBoxButton.OK);

                    return;
                }
            }
        }

        private void SavePartyAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.AddExtension = true;
            dialog.CreatePrompt = false;
            dialog.DefaultExt = ".cpd";

            if (!String.IsNullOrEmpty(partyFilename))
                dialog.FileName = partyFilename;

            dialog.Filter = "CharPad Data File|*.cpd|All Files|*.*";
            dialog.OverwritePrompt = true;
            dialog.Title = "Save Party File";
            dialog.ValidateNames = true;

            if (dialog.ShowDialog(this) == true)
            {
                try
                {
                    PlayerDataAdapter.SaveParty(dialog.FileName, Party);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format("Error saving party: ({0}) {1}{2}{3}",
                        ex.GetType().Name,
                        ex.Message,
                        Environment.NewLine,
                        ex.StackTrace),
                        "Error saving party",
                        MessageBoxButton.OK);

                    return;
                }

                partyFilename = dialog.FileName;
            }
        }

        private void LoadCharacterCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void SaveCharacterCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void RemoveCharacterCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (Party.IsDirty)
            {
                MessageBoxResult result = MessageBox.Show("Do you want to save the party before exiting?", "Save Party?", MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);

                if (result == MessageBoxResult.Cancel)
                    return;

                if (result == MessageBoxResult.Yes)
                {
                    SavePartyCommand_Executed(this, null);
                }
            }

            Close();
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
            player.Inventory.Add(WeaponDefinitions.Weapons.Find(x => StringComparer.CurrentCultureIgnoreCase.Compare(x.Name, "Longsword") == 0));
            player.Inventory.Add(WeaponDefinitions.Weapons.Find(x => StringComparer.CurrentCultureIgnoreCase.Compare(x.Name, "Battleaxe") == 0));
            player.Inventory.Add(WeaponDefinitions.Weapons.Find(x => StringComparer.CurrentCultureIgnoreCase.Compare(x.Name, "Dagger") == 0));
            player.Inventory.Add(ArmorDefinitions.NormalArmors.Find(x => StringComparer.CurrentCultureIgnoreCase.Compare(x.Name, "Leather") == 0));
            player.Inventory.Add(ArmorDefinitions.NormalArmors.Find(x => StringComparer.CurrentCultureIgnoreCase.Compare(x.Name, "Chainmail") == 0));
            player.Inventory.Add(ArmorDefinitions.Shields.Find(x => StringComparer.CurrentCultureIgnoreCase.Compare(x.Name, "Light shield") == 0));
            player.Weapon = (Weapon)player.Inventory[0];
            player.Armor = (Armor)player.Inventory[4];

            Power power = new Power(player);
            power.Level = 1;
            power.Name = "Cleave";
            power.Notes = "do 2 damage to adjacent mob";

            player.Powers.Add(power);

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
