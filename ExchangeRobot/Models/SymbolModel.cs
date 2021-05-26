using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRobot.Models
{
    public class SymbolModel
    {
        //计价
        public string Quote { get; set; }

        //货
        public string Base { get; set; }

        //币对
        public string Symbol { get; set; }
    }
}
