using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRobot.Models
{
    public class TradeModel
    {
        public decimal Quantity { get; set; }

        public TradeType Type { get; set; }

        public decimal Price { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}
