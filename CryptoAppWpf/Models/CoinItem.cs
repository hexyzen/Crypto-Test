using System.Collections.Generic;

namespace CryptoAppWpf.Models
{
    public class CoinItem
    {
        public Item item { get; set; }
    }

    public class Item
    {
        public string id { get; set; }
        public int coin_id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public int market_cap_rank { get; set; }
        public string thumb { get; set; }
        public string small { get; set; }
        public string large { get; set; }
        public string slug { get; set; }
        public double price_btc { get; set; }
        public int score{ get; set; }
    }

    public class Root
    {
        public List<CoinItem> coins { get; set; }
        public List<object> exchanges { get; set; }
    }
}
