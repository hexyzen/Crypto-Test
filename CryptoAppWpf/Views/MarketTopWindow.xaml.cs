using System.Windows.Controls;
using CryptoAppWpf.ViewModels;

namespace CryptoAppWpf.Views
{
    public partial class MarketTopWindow : Page
    {
        private readonly MarketTopViewModel _viewModel;

        public MarketTopWindow()
        {
            InitializeComponent();
            _viewModel = new MarketTopViewModel();
            DataContext = _viewModel;
            LoadTopCoins();
        }

        private async void LoadTopCoins()
        {
            await _viewModel.LoadTopCoinsAsync();
        }
    }
}
