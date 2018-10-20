using System.Windows;

namespace Client.Windows
{
    public partial class CrossOrNullWindow : Window
    {
        public CrossOrNullWindow()
        {
            InitializeComponent();
        }

        public CrossOrNullWindow(Window owner) : this()
        {
            Owner = owner;
        }

        private void ButtonCrossChoice_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void ButtonNullChoice_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}