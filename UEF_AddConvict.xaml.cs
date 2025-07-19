using System.Windows;
using System.Windows.Controls;

namespace Social_Blade_Dashboard
{
    public partial class UEF_AddConvict : UserControl
    {
        public UEF_AddConvict()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Save button clicked!");
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Cancel button clicked!");
        }
    }
}
