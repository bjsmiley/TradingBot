using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ThreeFourteen.AlphaVantage;
using TradingBot.Shared.Messaging.Rabbit;
using TradingBot.Shared.Messaging.Rabbit.Data.Price;

namespace TradingBot.Services.StockPrices.API.Infrastructure.Workers
{
	public class AlphaVantageWorker : BackgroundService
	{
		private readonly ConcurrentDictionary<string, QueueManager> map;
		private readonly AutoResetEvent workToDo;
		private readonly ILogger<AlphaVantageWorker> logger;
		private readonly IRabbitMQManager _rabbitMQ;

		public AlphaVantageWorker(ILogger<AlphaVantageWorker> logger, IRabbitMQManager rabbitMQ)
		{
			map = new ConcurrentDictionary<string, QueueManager>();
			workToDo = new AutoResetEvent(false);
			this.logger = logger;
			_rabbitMQ = rabbitMQ;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			List<string> scheduledToDelete = new List<string>();

			while(!stoppingToken.IsCancellationRequested)
			{
				if (map.Count == 0)
					await Task.Delay(20);
					// workToDo.WaitOne();

				foreach(var pair in map)
				{

					while (pair.Value.Remove(out string symbol, out string type))
					{
						Task.Run(async () => 
						{
							var attempts = 0;
							var delay = 10;
							while(true)
							{
								if (attempts > 6)
								{
									logger.LogError("Fire and Forget API price Failed");
									return;
								}
									
								try
								{
									var result = await pair.Value.Client.Stocks.IntraDay(symbol).SetInterval(Interval.OneMinute).GetAsync();
									var data = result.Data[0].Close;
									var time = result.Data[0].Timestamp;

									var message = new Price
									{
										Symbol = symbol,
										Value = data,
										Timestamp = time
									};

									// send message
									var lastpart = pair.Value.ApiKey == "Demo" ? "Demo" : pair.Value.ApiKey.Sha256Hash();

									var routeKey = $"price.{type}.{lastpart}";

									_rabbitMQ.Publish(message, "exchange.price", "topic", routeKey);

									return;
								}
								catch (AlphaVantageApiLimitException e)
								{
									logger.LogWarning(e, $"Attempt #{attempts} to access AV api.");
								}

								await Task.Delay(delay);
								attempts++;
								delay *= 10;
							}
							

						}).Forget();
						
					}
					scheduledToDelete.Add(pair.Key);
				}

				foreach(var apikey in scheduledToDelete)
				{
					map.TryRemove(apikey, out _);
				}
				scheduledToDelete.Clear();
			}
		}

		public void AddOrUpdate(string apikey,string type, string[] symbols)
		{
			Func<string,QueueManager> addFactory = (apikey) => new QueueManager(apikey, symbols, type);
			Func<string, QueueManager, QueueManager> updateFactory = (apiKey, manager) =>
			{
				if (manager.Add(symbols, type)) return manager;
				else
					return new QueueManager(apiKey, symbols, type);
			};

			map.AddOrUpdate(apikey, addFactory, updateFactory);


			workToDo.Set();
		}
	}
}
