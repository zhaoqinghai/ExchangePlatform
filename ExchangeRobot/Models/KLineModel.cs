using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRobot.Models
{
    public class KLineModel
    {
        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Open { get; set; }

        public decimal Close { get; set; }

        public decimal Amount { get; set; }

        public DateTimeOffset Timestamp { get; set; }
    }
}
