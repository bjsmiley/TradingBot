using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace TradingBot.Services.Users.API.Domain.Events
{
	public class UserDeletedEventHandler : INotificationHandler<UserDeletedEvent>
	{
		private readonly ILogger _logger;

		public UserDeletedEventHandler(ILogger<UserDeletedEventHandler> logger)
		{
			_logger = logger;
		}
		public Task Handle(UserDeletedEvent notification, CancellationToken cancellationToken)
		{
			_logger.LogDebug($"User {notification.UserId} has been deleted.");

			return Task.CompletedTask;
		}
	}
}
