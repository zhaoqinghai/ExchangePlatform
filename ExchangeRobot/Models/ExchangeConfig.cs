using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRobot.Models
{
    public class ExchangeConfig
    {
        public static ExchangeConfig Default { get; set; }
        public List<ExchangeInfo> Exchanges { get; set; }
    }

    public class ExchangeInfo
    {
        public ExchangeType Type { get; set; }

        public List<string> Symbols { get; set; }
    }
}
