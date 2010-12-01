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
    /// Interaction logic for AddBasicAdjustmentWindow.xaml
    /// </summary>
    public partial class AddBasicAdjustmentWindow : Window
    {
        private BasicAdjustment adjustment;
        private BasicAdjustment originalAdjustment;

        public AddBasicAdjustmentWindow(BasicAdjustment adjustment)
        {
            this.originalAdjustment = adjustment;
            
            InitializeComponent();

            this.Title = (adjustment == null ? "Add Adjustment" : "Edit Adjustment");

            if (adjustment != null)
            {
                txtAdjustment.Text = adjustment.Modifier.ToString();
                txtNote.Text = adjustment.Note;
            }
        }

        public BasicAdjustment Adjustment
        {
            get { return adjustment; }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            string adjText = txtAdjustment.Text.Trim();

            if (adjText.StartsWith("+"))
                adjText = adjText.Substring(1);

            int tempValue;

            if (!Int32.TryParse(adjText, out tempValue))
            {
                MessageBox.Show("Please enter a number for the adjustment.", "Invalid adjustment", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtAdjustment.Focus();
                return;
            }

            if (String.IsNullOrWhiteSpace(txtNote.Text))
            {
                MessageBox.Show("Please enter a note.", "Invalid note", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNote.Focus();
                return;
            }

            adjustment = new BasicAdjustment(tempValue, txtNote.Text);

            DialogResult = true;
            Close();
        }
    }
}
