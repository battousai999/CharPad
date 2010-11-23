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
    /// Interaction logic for EditDefenseWindow.xaml
    /// </summary>
    public partial class EditDefenseWindow : Window
    {
        private Player player;
        private DefenseType defenseType;

        public EditDefenseWindow(Player player, DefenseType defenseType)
        {
            this.player = player;
            this.defenseType = defenseType;

            InitializeComponent();
        }

        public Player Player
        {
            get { return player; }
        }

        public DefenseValue DefenseValue
        {
            get { return player.GetDefenseValue(defenseType); }
        }

        public string DefenseTitle
        {
            get
            {
                switch (defenseType)
                {
                    case DefenseType.AC:
                        return "AC Defense:";
                    case DefenseType.Fortitude:
                        return "Fortitude Defense";
                    case DefenseType.Reflex:
                        return "Reflex Defense";
                    case DefenseType.Will:
                        return "Will Defense";
                    default:
                        throw new InvalidOperationException("Unexpected defense type: " + Enum.Format(typeof(DefenseType), defenseType, "G"));
                }
            }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
