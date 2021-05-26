using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRobot.Models
{
    public class CommonModel
    {
        public static readonly Dictionary<ExchangeType, string> Exchanges = new Dictionary<ExchangeType, string>()
        {
            [ExchangeType.Binance] = "Binance",
            [ExchangeType.Huobi] = "Huobi",
            [ExchangeType.Okex] = "Okex",
            [ExchangeType.Zb] = "Zb",
            [ExchangeType.GateIo] = "GateIo",
        };
    }

    public enum ExchangeType
    {
        Binance,
        Huobi,
        Okex,
        Zb,
        GateIo
    }

    public enum TradeType
    {
        BuyTaker,
        SellTaker
    }
}
