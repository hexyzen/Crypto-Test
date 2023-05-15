using CryptoAppWpf.Models;
using CryptoAppWpf.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace CryptoAppWpf.Views
{
    public partial class DetailsWindow : Page
    {
        private readonly DetailsViewModel _viewModel;

        public DetailsWindow()
        {
            SwitchLanguage("EN");
            InitializeComponent();
            _viewModel = new DetailsViewModel();
            DataContext = _viewModel;
        }

        private async Task<object> LoadCoinDetails(string coinId)
        {
           return await _viewModel.LoadCoinDetailsAsync(coinId);
        }


        private string currencyPrice;
        private async void ButtonSearchClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    dynamic selCoin = await LoadCoinDetails(textbox_search.Text);
                    currencyPrice = selCoin.market_data.current_price.usd;
                    await SetButtonContent(selCoin.links.homepage, buttonHomepage);
                    await SetButtonContent(selCoin.links.official_forum_url, buttonForum);
                    var market = await GetMarketData();
                    UpdateMarketLabels(market);
                    frame_OnTop.Visibility = Visibility.Collapsed;
                    label_error.Content = "";
                }
            }
            catch (HttpRequestException ex)
            {
                frame_OnTop.Visibility = Visibility.Visible;
                string errorMessage = $"Error: {ex.StatusCode}";
                label_error.Content = errorMessage;
            }
        }

        private async Task SetButtonContent(JArray array, Button button)
        {
            int i = 0;
            while (i < array.Count && i <= 2)
            {
                string page = array[i].ToString();
                i++;
                string trim = Regex.Replace(page, @"^(?:http(?:s)?://)?(?:www(?:[0-9]+)?\.)?", string.Empty, RegexOptions.IgnoreCase);
                await button.Dispatcher.InvokeAsync(() =>
                {
                    button.Content = trim;
                    button.Click += (sender, e) => OpenWebPage(page);
                });
                break;
            }
        }

        private void OpenWebPage(string url)
        {
            Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
        }

        private void ButtonConvertClick(object sender, RoutedEventArgs e)
        {
            Converter();
        }

        public async Task Converter()
        {
            CultureInfo culture = new CultureInfo("en-US");
            if (string.IsNullOrEmpty(textbox_convert.Text))
            {
                MessageBox.Show("Please Enter API id of a currency", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                textbox_convert.Focus();
                return;
            }
            else if (string.IsNullOrEmpty(textbox_mul.Text))
            {
                MessageBox.Show("Please Enter amount", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                textbox_mul.Focus();
                return;
            }

            using (HttpClient client = new HttpClient())
            {
                string url = $"https://api.coingecko.com/api/v3/coins/{textbox_convert.Text}?localization=false&tickers=true&market_data=true&community_data=true&developer_data=false&sparkline=false";
                string json = await client.GetStringAsync(url);

                dynamic obj = JObject.Parse(json);

                decimal val1 = Convert.ToDecimal(currencyPrice);
                decimal val2 = obj.market_data.current_price.usd;
                decimal mul = Convert.ToDecimal(textbox_mul.Text, culture);

                textbox_result.Text = (val1 * mul / val2).ToString("N1", culture);
            }

        }

        private async Task<List<MarketData>> GetMarketData()
        {
            using (HttpClient client = new HttpClient())
            {
                string url = "https://api.coingecko.com/api/v3/exchanges?per_page=6&page=1";
                string json = await client.GetStringAsync(url);
                List<MarketData> market = JsonConvert.DeserializeObject<List<MarketData>>(json);
                return market;
            }
        }

        private void UpdateMarketLabels(List<MarketData> market)
        {
            var numberOfLabels = 4;

            for (int i = 0; i <= numberOfLabels; i++)
            {
                var labelName = $"label_market{i}";
                var label = (Label)FindName(labelName);
                label.Content = string.Format(CultureInfo.CreateSpecificCulture("en-US"), "{0}. {1} {2:C1}", market[i].trust_score_rank, market[i].name, market[i].trade_volume_24h_btc);
            }
        }

        private void buttonGeckoClick(object sender, RoutedEventArgs e)
        {
            string url = $"https://www.coingecko.com/en/coins/{textbox_search.Text}";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }


        public void SwitchLanguage(string languageCode)
        {
            ResourceDictionary dictionary = new ResourceDictionary();

            switch (languageCode)
            {
                case "en":
                    dictionary.Source = new Uri("..\\Localization\\StringResources.en.xaml", UriKind.Relative);
                    break;

                case "ua":
                    dictionary.Source = new Uri("..\\Localization\\StringResources.ua.xaml", UriKind.Relative);
                    break;

                default:
                    dictionary.Source = new Uri("..\\Localization\\StringResources.en.xaml", UriKind.Relative);
                    break;
            }
            Resources.MergedDictionaries.Add(dictionary);
        }

        public void HandleCheckUA(object sender, RoutedEventArgs e)
        {
            SwitchLanguage("ua");
        }

        public void HandleUncheckedEN(object sender, RoutedEventArgs e)
        {
            SwitchLanguage("en");
        }
    }
}
