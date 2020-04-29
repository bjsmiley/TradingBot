using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TradingBot.Services.Users.API.Domain.Events
{
	public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
	{
		private readonly ILogger _logger;

		public UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger)
		{
			_logger = logger;
		}
		public Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
		{
			_logger.LogDebug($"User {notification.UserId} has been created.");

			return Task.CompletedTask;
		}
	}
}
