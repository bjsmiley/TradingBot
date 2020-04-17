using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockPrices.API.Application.Models;
using ThreeFourteen.AlphaVantage;
using ThreeFourteen.AlphaVantage.Response;
using ThreeFourteen.AlphaVantage.Model;

namespace StockPrices.API.Infrastructure.Services
{
	public class AlphaVantageClient : IPricingService
	{
		private readonly AlphaVantage _alphaVantage;

		public AlphaVantageClient(AlphaVantageSettings settings)
		{
			_alphaVantage = new AlphaVantage(settings.ApiKey);
		}

		public async Task<double> GetPriceAsync(string symbol)
		{
			try
			{
				var result = await _alphaVantage.Stocks.IntraDay(symbol).SetInterval(Interval.OneMinute).GetAsync();
				return result.Data[0].Close;
			}
			catch(AlphaVantageApiLimitException)
			{
				await Task.Delay(TimeSpan.FromSeconds(1));
				var result = await _alphaVantage.Stocks.IntraDay(symbol).SetInterval(Interval.OneMinute).GetAsync();
				return result.Data[0].Close;
			}

		}
	}
}
