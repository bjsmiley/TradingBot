using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace TradingBot.Services.Users.API.Application.Commands
{
	public class DeleteCommand : IRequest<bool>
	{
		public Guid UserId { get; set; }
	}
}
