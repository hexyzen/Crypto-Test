using CryptoAppWpf.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Diagnostics;

namespace CryptoAppWpf.ViewModels
{
    public class MarketTopViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<CoinItem> _topCoins;

        public ObservableCollection<CoinItem> TopCoins
        {
            get { return _topCoins; }
            set
            {
                _topCoins = value;
                OnPropertyChanged(nameof(TopCoins));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task LoadTopCoinsAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.coingecko.com/api/v3/");

                var response = await client.GetAsync("search/trending");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var root = JsonConvert.DeserializeObject<Root>(json);

                    TopCoins = new ObservableCollection<CoinItem>(root.coins.Take(8));
                }
                else
                {
                    // Handle error
                }
            }
        }

    }
}
