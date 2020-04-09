using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
namespace Users.API.Domain.Events
{
	public class UserCreatedEvent : INotification
	{
		public Guid UserId { get; set; }

		public UserCreatedEvent(Guid id) => (UserId) = (id);
	}
}
