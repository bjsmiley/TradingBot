using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace TradingBot.Services.StockPrices.API.Application.Queries
{
	public class PriceQuery : IRequest
	{
		public string[] Symbols { get; set; }
		public string ApiKey { get; set; }
		public string Type { get; set; }
	}
}
