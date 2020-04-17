using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockPrices.API.Application.Models
{
	public interface IPricingService
	{
		public Task<double> GetPriceAsync(string symbol);
	}
}
