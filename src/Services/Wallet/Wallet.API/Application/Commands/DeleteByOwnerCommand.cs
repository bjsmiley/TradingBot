using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace TradingBot.Services.Wallet.API.Application.Commands
{
	public class DeleteByOwnerCommand : IRequest<bool>
	{
		public Guid Owner { get; set; }
	}
}
