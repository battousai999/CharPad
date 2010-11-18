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

namespace CharPad
{
    /// <summary>
    /// Interaction logic for EditCharacterLevelWindow.xaml
    /// </summary>
    public partial class EditCharacterLevelWindow : Window
    {
        public EditCharacterLevelWindow(int playerLevel)
        {
            InitializeComponent();

            txtLevel.Text = playerLevel.ToString();
        }

        public int Level
        {
            get { return Convert.ToInt32(txtLevel.Text); }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            int tempLevel;

            if (!Int32.TryParse(txtLevel.Text, out tempLevel) || (tempLevel < 1) || (tempLevel > 30))
            {
                MessageBox.Show(this, "Please enter a valid level (1 - 30).", "Invalid Player Level", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtLevel.Focus();
                return;
            }

            DialogResult = true;
            Close();
        }
    }
}
