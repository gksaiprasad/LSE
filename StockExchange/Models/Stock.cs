using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LSE.TradeApi.Models
{
    public class Stock
    {
        [Key]
        public string TickerSymbol { get; set; }
        public string CompanyName { get; set; }
        public string Sector { get; set; }

        public ICollection<Trade> Trades { get; set; }
    }
}