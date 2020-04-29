using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TradingBot.Services.Wallet.API.Application.Models;

namespace TradingBot.Services.Wallet.API.Application.Commands
{
	public class CreateCommand : IRequest<WalletItem>
	{
		[BindRequired]
		public Guid Owner { get; set; }
	}
}
