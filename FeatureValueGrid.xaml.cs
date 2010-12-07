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

namespace CharPad
{
    /// <summary>
    /// Interaction logic for FeatureValueGrid.xaml
    /// </summary>
    public partial class FeatureValueGrid : UserControl
    {
        public FeatureValueGrid()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ListProperty =
            DependencyProperty.Register("List", typeof(ObservableCollectionEx<FeatureValue>), typeof(FeatureValueGrid),
            new UIPropertyMetadata(null));

        public ObservableCollectionEx<FeatureValue> List
        {
            get { return (ObservableCollectionEx<FeatureValue>)GetValue(ListProperty); }
            set { SetValue(ListProperty, value); }
        }

        private void lvMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditSelectedItem();
        }

        private void EditSelectedItem()
        {
            FeatureValue feature = (lvMain.SelectedItem as FeatureValue);

            if (feature == null)
                return;

            AddFeatureValueWindow window = new AddFeatureValueWindow(feature);

            if (window.ShowDialog(Application.Current.MainWindow))
            {
                feature.Name = window.Feature.Name;
                feature.ShortDescription = window.Feature.ShortDescription;
                feature.LongDescription = window.Feature.LongDescription;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddFeatureValueWindow window = new AddFeatureValueWindow(null);

            if (window.ShowDialog(Application.Current.MainWindow))
            {
                List.Add(window.Feature);
            }
        }

        public void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            EditSelectedItem();
        }

        public void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            FeatureValue feature = (lvMain.SelectedItem as FeatureValue);

            if (feature == null)
                return;

            if (MessageBox.Show("Are you sure you want to delete this feature?", "Delete feature?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                List.Remove(feature);
            }
        }
    }
}
