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
using System.ComponentModel;
using CharPad.Framework;

namespace CharPad
{
    /// <summary>
    /// Interaction logic for BasicAdjustmentGrid.xaml
    /// </summary>
    public partial class BasicAdjustmentGrid : UserControl
    {
        public BasicAdjustmentGrid()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ListProperty =
            DependencyProperty.Register("List", typeof(BasicAdjustmentList), typeof(BasicAdjustmentGrid),
            new UIPropertyMetadata(null));

        public BasicAdjustmentList List
        {
            get { return (BasicAdjustmentList)GetValue(ListProperty); }
            set { SetValue(ListProperty, value); }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
