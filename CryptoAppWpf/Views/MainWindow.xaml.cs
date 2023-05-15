using System.Windows;
using System.Windows.Controls;
using System.Xaml;
namespace CryptoAppWpf.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
 
        private void sidebar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = sidebar.SelectedItem as NavButton;
            navframe.Navigate(selected.Navlink);

        }


    }


}





