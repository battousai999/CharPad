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
    /// Interaction logic for AddResistanceValueWindow.xaml
    /// </summary>
    public partial class AddResistanceValueWindow : Window
    {
        private ResistanceValue resistance;
        private ResistanceValue originalResistance;
        private List<string> damageTypes;

        public AddResistanceValueWindow(ResistanceValue resistance)
        {
            this.originalResistance = resistance;

            damageTypes = new List<string>();

            damageTypes.Add("acid");
            damageTypes.Add("cold");
            damageTypes.Add("fire");
            damageTypes.Add("force");
            damageTypes.Add("lightning");
            damageTypes.Add("necrotic");
            damageTypes.Add("poison");
            damageTypes.Add("psychic");
            damageTypes.Add("radiant");
            damageTypes.Add("thunder");
            damageTypes.Add("normal");

            InitializeComponent();

            this.Title = (resistance == null ? "Add Resistance Value" : "Edit Resistance Value");

            if (resistance != null)
            {
                txtAdjustment.Text = resistance.Modifier.ToString();
                cboDamageType.Text = resistance.DamageType;
                txtNote.Text = resistance.Description;
            }
        }

        public List<string> DamageTypes
        {
            get { return damageTypes; }
        }

        public ResistanceValue Resistance
        {
            get { return resistance; }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            string adjText = txtAdjustment.Text.Trim();
        
            if (adjText.StartsWith("+"))
                adjText = adjText.Substring(1);

            int tempValue;

            if (!Int32.TryParse(adjText, out tempValue))
            {
                MessageBox.Show("Please enter a number for the resistance.", "Invalid resistance", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtAdjustment.Focus();
                return;
            }

            if (String.IsNullOrWhiteSpace(cboDamageType.Text))
            {
                MessageBox.Show("Please enter a damage type.", "Invalid damage type", MessageBoxButton.OK, MessageBoxImage.Warning);
                cboDamageType.Focus();
                return;
            }

            if (String.IsNullOrWhiteSpace(txtNote.Text))
            {
                MessageBox.Show("Please enter a note.", "Invalid note", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNote.Focus();
                return;
            }

            resistance = new ResistanceValue(tempValue, cboDamageType.Text, txtNote.Text);

            DialogResult = true;
            Close();
        }
    }
}
