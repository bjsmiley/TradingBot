using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockPrices.API.Infrastructure.Models
{
	public enum LookUpContext : int
	{
		Query = 0,
		Buy = 1,
		Sell = 2
	}

	public class LookupRequest
	{
		public Guid LookUpId { get; set; }
		public string Symbol { get; set; }
		public string ApiKey { get; set; }
		public LookUpContext Context { get; set; }

	}
}
