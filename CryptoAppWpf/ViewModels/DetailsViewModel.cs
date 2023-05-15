using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptoAppWpf.ViewModels
{
    public class DetailsViewModel : INotifyPropertyChanged
    {
        private dynamic _selectedCoin;
 
        public dynamic SelectedCoin
        {
            get { return _selectedCoin; }
            set
            {
                _selectedCoin = value;
                OnPropertyChanged(nameof(SelectedCoin));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<object> LoadCoinDetailsAsync(string coinId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.coingecko.com/api/v3/coins/");

                var response = await client.GetAsync($"{coinId}?localization=false&tickers=false&market_data=true&community_data=false&developer_data=false&sparkline=false");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    SelectedCoin = JsonConvert.DeserializeObject(json);
                    return SelectedCoin;
                }
                else
                {
                    return SelectedCoin;
                    // Handle error
                }
            }
        }

    }
}
