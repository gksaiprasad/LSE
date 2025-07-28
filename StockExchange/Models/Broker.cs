using System;
using System.Collections.Generic;

namespace LSE.TradeApi.Models
{
    public class Broker
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Trade> Trades { get; set; }
    }
}