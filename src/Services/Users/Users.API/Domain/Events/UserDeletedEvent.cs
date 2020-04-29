using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
namespace TradingBot.Services.Users.API.Domain.Events
{
	public class UserDeletedEvent : INotification
	{
		public Guid UserId { get; }

		public UserDeletedEvent(Guid id) => (UserId) = (id);
	}
}
