using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRobot.Models
{
    public class TickerModel
    {
        public decimal BestBidPrice { get; set; }

        public decimal BestBidQuantity { get; set; }

        public decimal BestAskPrice { get; set; }

        public decimal BestAskQuantity { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}
