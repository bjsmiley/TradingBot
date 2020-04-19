using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThreeFourteen.AlphaVantage;
using ThreeFourteen.AlphaVantage.Response;
using ThreeFourteen.AlphaVantage.Model;

namespace TradingBot.Services.StockPrices.API.Infrastructure
{
	public class QueueManager
	{

		private readonly Queue<string> toQuery;
		private readonly Queue<string> toSell;
		private readonly Queue<string> toBuy;
		private readonly object mutex;

		public bool IsCompleted { get; private set; }
		public AlphaVantage Client { get; private set; }
		public string ApiKey { get; private set; }

		public QueueManager(string key, string[] symbols, string type)
		{
			mutex = new object();
			toQuery = new Queue<string>();
			toSell = new Queue<string>();
			toBuy = new Queue<string>();
			IsCompleted = false;
			Client = new AlphaVantage(key);
			var @default = Environment.GetEnvironmentVariable("ALPHAVANTAGE_APIKEY") ?? throw new Exception("Enviroment variable 'ALPHAVANTAGE_APIKEY' is not set. This is needed for pricing data.");
			ApiKey = key == @default ? "Demo" : key;
			Add(symbols, type);
		}

		public bool Add(string[] symbols, string type)
		{

			lock(mutex)
			{
				var addingTo = type switch
				{
					"buy" => toBuy,
					"sell" => toSell,
					_ => toQuery,
				};

				if (IsCompleted)
					return false;

				foreach (var symbol in symbols)
				{
					addingTo.Enqueue(symbol);
				}
				
				return true;

			}

		}

		public bool Peek(out string symbol, out string type)
		{
			symbol = null;
			type = null;
			lock (mutex)
			{
				if (IsCompleted) return false;

				if (toSell.TryPeek(out symbol))
				{
					type = "sell";
					return true;
				}

				else if (toBuy.TryPeek(out symbol))
				{
					type = "buy";
					return true;
				}

				else if (toQuery.TryPeek(out symbol))
				{
					type = "query";
					return true;
				}

				else
				{
					IsCompleted = true;
					return false;
				}
			}
		}

		public bool Remove(out string symbol, out string type)
		{
			symbol = null;
			type = null;

			lock(mutex)
			{
				if (IsCompleted) return false;

				if (toSell.TryDequeue(out symbol))
				{
					type = "sell";
					return true;
				}
				else if (toBuy.TryDequeue(out symbol))
				{
					type = "buy";
					return true;
				}
				else if (toQuery.TryDequeue(out symbol))
				{
					type = "query";
					return true;
				}

				else
				{
					IsCompleted = true;
					return false;
				}
			}
		}
	}
}
