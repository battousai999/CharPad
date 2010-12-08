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
    /// Interaction logic for EditSkillValueWindow.xaml
    /// </summary>
    public partial class EditSkillValueWindow : Window
    {
        private SkillValue skill;

        public EditSkillValueWindow(SkillValue skill)
        {
            this.skill = skill;

            InitializeComponent();
        }

        public SkillValue Skill
        {
            get { return skill; }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
