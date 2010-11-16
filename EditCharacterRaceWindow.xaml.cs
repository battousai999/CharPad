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
    /// Interaction logic for EditCharacterRaceWindow.xaml
    /// </summary>
    public partial class EditCharacterRaceWindow : Window
    {
        #region Classes

        public class CreatureSizeItem
        {
            private static List<CreatureSizeItem> list;
            
            public string Name { get { return Enum.Format(typeof(CreatureSize), Size, "G"); } }
            public CreatureSize Size { get; set; }

            public static List<CreatureSizeItem> GetSizeList()
            {
                if (list != null)
                    return list;

                list = new List<CreatureSizeItem>();

                list.Add(new CreatureSizeItem { Size = CreatureSize.Small });
                list.Add(new CreatureSizeItem { Size = CreatureSize.Meduim });
                list.Add(new CreatureSizeItem { Size = CreatureSize.Large });

                return list;
            }
        }

        #endregion

        public static List<RaceDefinition> RaceList = RaceDefinition.GetClasses();
        public static List<CreatureSizeItem> SizeList = CreatureSizeItem.GetSizeList();

        private PlayerRace race = new PlayerRace();

        public EditCharacterRaceWindow(PlayerRace race)
        {
            if (race != null)
            {
                this.race.CopyValues(race);
            }

            InitializeComponent();

            RaceDefinition tempRace = RaceList.Find(x => StringComparer.CurrentCultureIgnoreCase.Compare(x.Name, this.race.Name) == 0);

            if (tempRace != null)
                UpdateNotes(tempRace);
        }

        private void UpdateNotes(RaceDefinition race)
        {
            List<Inline> list = new List<Inline>();

            list.Add(new Bold(new Run("Attribute Bonuses:" + Environment.NewLine)));
            list.Add(new Run(String.Join(", ", race.AttributeBonuses.ConvertAll(x =>
            {
                if (x.CanChooseAttribute)
                {
                    return String.Format("{0} {1} or {2}",
                        (x.Bonus < 0 ? x.Bonus.ToString() : "+" + x.Bonus.ToString()),
                        Enum.Format(typeof(AttributeType), x.Attribute, "G"),
                        Enum.Format(typeof(AttributeType), x.OptionalAttribute.Value, "G"));
                }
                else
                    return String.Format("{0} {1}", (x.Bonus < 0 ? x.Bonus.ToString() : "+" + x.Bonus.ToString()), (x.Attribute == AttributeType.Wildcard ? "any attribute" : Enum.Format(typeof(AttributeType), x.Attribute, "G")));
            }))));

            list.Add(new Run(Environment.NewLine));
            list.Add(new Run(Environment.NewLine));
            list.Add(new Bold(new Run("Skill Bonuses:" + Environment.NewLine)));

            if (race.SkillBonues.Count == 0)
                list.Add(new Run("<none>"));
            else
                list.Add(new Run(String.Join(", ", race.SkillBonues.ConvertAll(x => String.Format("{0} {1}", (x.Bonus < 0 ? x.Bonus.ToString() : "+" + x.Bonus.ToString()), (x.Skill == Skill.Wildcard ? "any skill" : Enum.Format(typeof(Skill), x.Skill, "G")))))));

            txtNotes.Inlines.Clear();
            txtNotes.Inlines.AddRange(list);
        }

        public PlayerRace Race
        {
            get { return race; }
        }

        private void cboRace_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RaceDefinition selectedRace = (cboRace.SelectedItem as RaceDefinition);

            if (selectedRace == null)
                return;

            race.Name = selectedRace.Name;
            race.Size = selectedRace.Size;
            race.BaseSpeed = selectedRace.Speed;

            UpdateNotes(selectedRace);
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
