using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StockPrices.API.Protos;

namespace StockPrices.API.Application.Models
{
	public interface IRequestPriceService
	{
		public Task<StockPriceResponse> RequestPriceAsync(StockPriceRequest request);
	}
}
