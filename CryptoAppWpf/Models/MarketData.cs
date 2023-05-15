using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAppWpf.Models
{
    public class MarketData
    {
        public string Id { get; set; }
        public string name { get; set; }
        public string Url { get; set; }
        public string Image { get; set; }
        public int trust_score_rank { get; set; }
        public double trade_volume_24h_btc { get; set; }
        public double trade_volume_24h_btc_normalized { get; set; }
    }

}
