using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using StockPrices.API.Protos;
using StockPrices.API.Application.Models;
using System.Threading.Channels;

namespace StockPrices.API
{
	public class PricingService : Pricing.PricingBase
	{
		private readonly IRequestPriceService _client;


		public PricingService(IRequestPriceService client)
		{
			_client = client ?? throw new ArgumentNullException("client");

		}

		public override async Task<StockPriceResponse> GetCurrentPrice(StockPriceRequest request, ServerCallContext context)
		{
			return await _client.RequestPriceAsync(request);
		}

		public override async Task GetPriceStream(StockPriceRequest request, IServerStreamWriter<StockPriceResponse> responseStream, ServerCallContext context)
		{
		
			while (!context.CancellationToken.IsCancellationRequested)
			{
				await responseStream.WriteAsync(await _client.RequestPriceAsync(request));
			}
		}
	}
}
