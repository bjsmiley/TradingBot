using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TradingBot.Services.StockPrices.API.Infrastructure.Workers;

namespace TradingBot.Services.StockPrices.API.Application.Queries
{
	public class PriceQueryHandler : IRequestHandler<PriceQuery>
	{
		private readonly AlphaVantageWorker _worker;
		public PriceQueryHandler(AlphaVantageWorker worker)
		{
			_worker = worker;
		}

		public Task<Unit> Handle(PriceQuery request, CancellationToken cancellationToken)
		{
			_worker.AddOrUpdate(request.ApiKey, request.Type, request.Symbols);
			return Unit.Task;
		}
	}
}
