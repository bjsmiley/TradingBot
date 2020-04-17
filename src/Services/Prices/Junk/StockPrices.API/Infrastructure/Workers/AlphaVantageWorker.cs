using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Channels;
using StockPrices.API.Infrastructure.Models;
using StockPrices.API.Infrastructure.Services;
using Priority_Queue;
using StockPrices.API.Protos;
using StockPrices.API.Application.Models;

namespace StockPrices.API.Infrastructure.Workers
{
	public class AlphaVantageWorker : BackgroundService, IRequestPriceService
	{

		private readonly IPricingService _defaultService;
		private readonly SimplePriorityQueue<LookupRequest, int> _queue;
		private readonly ConcurrentDictionary<Guid, ChannelWriter<LookUpResponse>> _writers;

		public AlphaVantageWorker(IPricingService defaultService)
		{
			_defaultService = defaultService ?? throw new ArgumentNullException("defaultService");
			_queue = new SimplePriorityQueue<LookupRequest, int>();
			_writers = new ConcurrentDictionary<Guid, ChannelWriter<LookUpResponse>>();
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			LookupRequest request;

			while(!stoppingToken.IsCancellationRequested)
			{
				if(_queue.TryDequeue(out request))
				{

					var response = new LookUpResponse { LookUpId = request.LookUpId };

					IPricingService service;

					if (string.IsNullOrEmpty(request.ApiKey))
						service = _defaultService;
					else
						service = new AlphaVantageClient(new AlphaVantageSettings { ApiKey = request.ApiKey });

					response.Price = await service.GetPriceAsync(request.Symbol);

					var result = _writers.Remove(response.LookUpId, out var writer);

					if (!result) throw new NotSupportedException();

					await writer.WriteAsync(response);
					writer.Complete();
				}
				else
				{
					await Task.Delay(100, stoppingToken);
				}
			}
		}

		public async Task<StockPriceResponse> RequestPriceAsync(StockPriceRequest request)
		{
			var lookUp = new LookupRequest
			{
				LookUpId = Guid.NewGuid(),
				Symbol = request.Symbol,
				ApiKey = request.ApiKey,
			};

			lookUp.Context = request.Context switch
			{
				Context.Query => LookUpContext.Query,
				Context.Buy => LookUpContext.Buy,
				Context.Sell => LookUpContext.Sell,
				_ => throw new NotSupportedException()
			};

			var options = new BoundedChannelOptions(1);
			options.FullMode = BoundedChannelFullMode.DropNewest;
			options.SingleReader = true;
			options.SingleWriter = true;

			var channel = Channel.CreateBounded<LookUpResponse>(options);

			while (!_writers.TryAdd(lookUp.LookUpId, channel.Writer));

			_queue.Enqueue(lookUp, (int)lookUp.Context);

			if (!await channel.Reader.WaitToReadAsync()) throw new NotSupportedException();


			return new StockPriceResponse
			{
				Price = (await channel.Reader.ReadAsync()).Price
			};
			
			
		}

	}
}
