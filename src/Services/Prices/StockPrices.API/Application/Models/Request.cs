using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingBot.Services.StockPrices.API.Application.Models
{
	public class Request
	{
		public string ApiKey { get; set; }
		public string Type { get; set; }
	}
}
