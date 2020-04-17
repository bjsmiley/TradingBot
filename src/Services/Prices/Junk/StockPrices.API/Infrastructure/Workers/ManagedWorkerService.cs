using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StockPrices.API.Infrastructure.Workers
{
	public class ManagedWorkerService<T> : IHostedService where T : IHostedService
	{
		private readonly T _service;

		public ManagedWorkerService(T service)
		{
			_service = service ?? throw new ArgumentNullException("service");
		}
		public Task StartAsync(CancellationToken cancellationToken)
		{
			return _service.StartAsync(cancellationToken);
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return _service.StopAsync(cancellationToken);
		}
	}
}
