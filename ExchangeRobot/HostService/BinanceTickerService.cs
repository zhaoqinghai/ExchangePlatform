using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Binance.Net;
using Binance.Net.Enums;
using Binance.Net.Objects.Spot;
using CryptoExchange.Net.Objects;
using ExchangeRobot.Log;
using ExchangeRobot.Models;
using Microsoft.Extensions.Hosting;
using Polly;

namespace ExchangeRobot.HostService
{
    public class BinanceTickerService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var option = new BinanceSocketClientOptions()
            {
                Proxy = new ApiProxy("127.0.0.1", 10809)
            };
            var client = new BinanceSocketClient(option);
            
            Policy.Handle<Exception>(ex =>
            {
                LoggerHelper.Error<BinanceTickerService>("币安获取Ticker数据失败", ex);
                return true;
            }).RetryAsync(5).ExecuteAsync(async () =>
            {
                var binanceConfig = ExchangeConfig.Default.Exchanges.FirstOrDefault(x => x.Type == ExchangeType.Binance);
                if (binanceConfig != null)
                {
                    await client.Spot.SubscribeToBookTickerUpdatesAsync(binanceConfig.Symbols, x =>
                    {
                        DataCenterService.EnqueueTicker(ExchangeType.Binance, x.Symbol, new TickerModel()
                        {
                            BestAskPrice = x.BestAskPrice,
                            BestAskQuantity = x.BestAskQuantity,
                            BestBidPrice = x.BestBidPrice,
                            BestBidQuantity = x.BestBidQuantity,
                            Timestamp = DateTimeOffset.Now
                        });
                    });
                    await client.Spot.SubscribeToTradeUpdatesAsync(binanceConfig.Symbols, x =>
                    {
                        DataCenterService.EnqueueTrade(ExchangeType.Binance, x.Symbol, new TradeModel()
                        {
                            Quantity = x.Quantity,
                            Price = x.Price,
                            Timestamp = x.TradeTime,
                            Type = x.BuyerIsMaker ? TradeType.SellTaker : TradeType.BuyTaker
                        });
                    });
                    await Task.Delay(Timeout.Infinite, cancellationToken);
                    await client.UnsubscribeAll();
                }
            }); 
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
