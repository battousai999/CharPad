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
    /// Interaction logic for AddFeatureValueWindow.xaml
    /// </summary>
    public partial class AddFeatureValueWindow : Window
    {
        private FeatureValue feature;
        private FeatureValue originalFeature;

        public AddFeatureValueWindow(FeatureValue feature)
        {
            this.originalFeature = feature;

            InitializeComponent();

            this.Title = (feature == null ? "Add Feature" : "Edit Feature");

            if (feature != null)
            {
                txtName.Text = feature.Name;
                txtShortNote.Text = feature.ShortDescription;
                txtLongNote.Text = feature.LongDescription;
            }
        }

        public FeatureValue Feature
        {
            get { return feature; }
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a name.", "Invalid name", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtName.Focus();
                return;
            }

            if (String.IsNullOrWhiteSpace(txtShortNote.Text))
            {
                MessageBox.Show("Please enter a short note.", "Invalid short note", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtShortNote.Focus();
                return;
            }

            feature = new FeatureValue(txtName.Text, txtShortNote.Text, txtLongNote.Text);

            DialogResult = true;
            Close();
        }
    }
}
