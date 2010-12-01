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
    /// Interaction logic for EditCharacterClassWindow.xaml
    /// </summary>
    public partial class EditCharacterClassWindow : Window
    {
        public static List<ClassDefinition> ClassList = ClassDefinition.GetClasses();

        private PlayerClass _class = new PlayerClass();

        public EditCharacterClassWindow(PlayerClass _class)
        {
            if (_class != null)
                this._class.CopyValues(_class);

            InitializeComponent();

            ClassDefinition tempClass = ClassList.Find(x => StringComparer.CurrentCultureIgnoreCase.Compare(x.Name, this._class.Name) == 0);

            if (tempClass != null)
                UpdateNotes(tempClass);
        }

        private void UpdateNotes(ClassDefinition _class)
        {
            List<Inline> list = new List<Inline>();

            list.Add(new Bold(new Run("Key Abilities:" + Environment.NewLine)));
            list.Add(new Run(String.Join(", ", _class.KeyAttributes.ConvertAll(x => Enum.Format(typeof(AttributeType), x, "G")))));
            list.Add(new Run(Environment.NewLine));
            list.Add(new Run(Environment.NewLine));
            list.Add(new Bold(new Run("Armor Proficiencies:" + Environment.NewLine)));
            list.Add(new Run(String.Join(", ", _class.ArmorProficiencies.ConvertAll(x => Utility.GetArmorTypeName(x)))));
            list.Add(new Run(Environment.NewLine));
            list.Add(new Run(Environment.NewLine));
            list.Add(new Bold(new Run("Weapon Proficiencies:" + Environment.NewLine)));
            
            list.Add(new Run(String.Join(", ", _class.WeaponProficiencies.ConvertAll(x => Utility.GetWeaponCategoryName(x))
                .Union(_class.SpecificWeaponProficiencies.ConvertAll(x => Utility.GetWeaponTypeName(x))))));

            list.Add(new Run(Environment.NewLine));
            list.Add(new Run(Environment.NewLine));
            list.Add(new Bold(new Run(String.Format("Starting Skill{0}:{1}", (_class.AutomaticSkills.Count == 1 ? "" : "s"), Environment.NewLine))));

            if (_class.AutomaticSkills.Count > 0)
                list.Add(new Run(String.Join(", ", _class.AutomaticSkills.ConvertAll(x => Utility.GetSkillName(x)))));
            else
                list.Add(new Run("<none>"));

            list.Add(new Run(Environment.NewLine));
            list.Add(new Run(Environment.NewLine));

            if (_class.StartingSkills > 0)
            {
                list.Add(new Bold(new Run(String.Format("Select {0} {1} from the following list:{2}", _class.StartingSkills, (_class.StartingSkills == 1 ? "skill" : "skills"), Environment.NewLine))));
                list.Add(new Run(String.Join(", ", _class.TrainableSkills.ConvertAll(x => Utility.GetSkillName(x)))));
            }

            txtNotes.Inlines.Clear();
            txtNotes.Inlines.AddRange(list);
        }

        public PlayerClass Class
        {
            get { return _class; }
        }

        private void cboClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClassDefinition selectedClass = (cboClass.SelectedItem as ClassDefinition);

            if (selectedClass == null)
                return;

            _class.Name = selectedClass.Name;
            _class.BaseHealth = selectedClass.BaseHealth;
            _class.HealthPerLevel = selectedClass.HealthPerLevel;
            _class.BaseHealingSurges = selectedClass.BaseHealingSurges;
            _class.FortitudeBonus = selectedClass.DefenseBonuses.Sum(x => x.Defense == DefenseType.Fortitude ? x.Bonus : 0);
            _class.ReflexBonus = selectedClass.DefenseBonuses.Sum(x => x.Defense == DefenseType.Reflex ? x.Bonus : 0);
            _class.WillBonus = selectedClass.DefenseBonuses.Sum(x => x.Defense == DefenseType.Will ? x.Bonus : 0);

            UpdateNotes(selectedClass);
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
