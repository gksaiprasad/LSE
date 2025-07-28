using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LSE.TradeApi.Models
{
    public class Trade
    {
        public int Id { get; set; }

        [ForeignKey("Stock")]
        public string TickerSymbol { get; set; }
        public Stock Stock { get; set; }

        public decimal Price { get; set; }
        public decimal Shares { get; set; }

        [ForeignKey("Broker")]
        public Guid BrokerId { get; set; }
        public Broker Broker { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}