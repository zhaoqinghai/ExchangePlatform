using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Binance.Net;
using Binance.Net.Enums;
using ExchangeRobot.Log;
using ExchangeRobot.Models;
using Microsoft.Extensions.Hosting;
using Polly;

namespace ExchangeRobot.HostService
{
    public class DataCenterService : IHostedService
    {
        private static Dictionary<ExchangeType, Dictionary<string, Queue<KLineModel>>> _klineDict =
            new Dictionary<ExchangeType, Dictionary<string, Queue<KLineModel>>>();

        private static readonly Dictionary<ExchangeType, Dictionary<string, Queue<TickerModel>>> TickerDict =
            new Dictionary<ExchangeType, Dictionary<string, Queue<TickerModel>>>();

        private static readonly Dictionary<ExchangeType, Dictionary<string, Queue<TradeModel>>> TradeDict =
            new Dictionary<ExchangeType, Dictionary<string, Queue<TradeModel>>>();


        public static void EnqueueTicker(ExchangeType type, string symbol, TickerModel model)
        {
            if (!TickerDict.ContainsKey(type))
            {
                TickerDict[type] = new Dictionary<string, Queue<TickerModel>>();
            }

            if (!TickerDict[type].ContainsKey(symbol))
            {
                TickerDict[type][symbol] = new Queue<TickerModel>();
            }

            TickerDict[type][symbol].Enqueue(model);
        }

        public static void EnqueueTrade(ExchangeType type, string symbol, TradeModel model)
        {
            if (!TradeDict.ContainsKey(type))
            {
                TradeDict[type] = new Dictionary<string, Queue<TradeModel>>();
            }

            if (!TradeDict[type].ContainsKey(symbol))
            {
                TradeDict[type][symbol] = new Queue<TradeModel>();
            }
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Policy.Handle<Exception>(ex =>
            {
                LoggerHelper.Error<BinanceTickerService>("币安获取Ticker数据失败", ex);
                return true;
            }).RetryAsync(5).ExecuteAsync(async () =>
            {
                await Task.WhenAll(UpdateKline(), ClearData());

            });
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task ClearData()
        {
            while (true)
            {
                await Task.WhenAll(ClearTicker(), ClearTrade());

                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }

        public Task ClearTicker()
        {
            TickerDict.AsParallel().ForAll(x =>
            {
                foreach (var item in x.Value)
                {
                    x.Value[item.Key] = new Queue<TickerModel>(item.Value.Where(_ => _.Timestamp <= DateTimeOffset.Now.AddMinutes(-5)).OrderBy(_ => _.Timestamp));
                }
            });
            return Task.CompletedTask;
        }

        public Task ClearTrade()
        {
            TradeDict.AsParallel().ForAll(x =>
            {
                foreach (var item in x.Value)
                {
                    x.Value[item.Key] = new Queue<TradeModel>(item.Value.Where(_ => _.Timestamp <= DateTimeOffset.Now.AddMinutes(-5)).OrderBy(_ => _.Timestamp));
                }
            });
            return Task.CompletedTask;
        }
        public async Task UpdateKline()
        {
            while (true)
            {
                await Task.WhenAll(UpdateBinanceKline(), UpdateHuobiKline(), UpdateZbKline(), UpdateOkexKline(), UpdateGateIoKline());
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }

        public async Task UpdateBinanceKline()
        {
            var binanceClient = new BinanceClient();
            if (!_klineDict.ContainsKey(ExchangeType.Binance))
            {
                _klineDict[ExchangeType.Binance] = new Dictionary<string, Queue<KLineModel>>();
            }
            
            var binanceConfig = ExchangeConfig.Default.Exchanges.FirstOrDefault(x => x.Type == ExchangeType.Binance);
            if (binanceConfig != null)
            {
                foreach(var item in binanceConfig.Symbols)
                {
                    var tickers = await binanceClient.Spot.Market.GetKlinesAsync(item, KlineInterval.OneMinute);
                    _klineDict[ExchangeType.Binance][item] = new Queue<KLineModel>(tickers.Data.Select(_ =>
                    new KLineModel()
                    {
                        Open = _.Open,
                        Close = _.Close,
                        Amount = _.QuoteVolume,
                        Timestamp = _.CloseTime,
                        High = _.High,
                        Low = _.Low
                    }).OrderBy(x => x.Timestamp));
                    await Task.Delay(TimeSpan.FromSeconds(3));
                }
            }
        }

        public async Task UpdateHuobiKline()
        {

        }

        public async Task UpdateZbKline()
        {

        }

        public async Task UpdateOkexKline()
        {

        }

        public async Task UpdateGateIoKline()
        {

        }
    }
}
