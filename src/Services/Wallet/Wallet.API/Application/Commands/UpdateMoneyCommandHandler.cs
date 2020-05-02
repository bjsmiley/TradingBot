using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TradingBot.Services.Wallet.API.Application.Models;

namespace TradingBot.Services.Wallet.API.Application.Commands
{
	public class UpdateMoneyCommandHandler : IRequestHandler<UpdateMoneyCommand, WalletItem>
	{
		private readonly IWalletRepository _repo;

		public UpdateMoneyCommandHandler(IWalletRepository repo)
		{
			_repo = repo;
		}

		public Task<WalletItem> Handle(UpdateMoneyCommand request, CancellationToken cancellationToken)
		{
			return _repo.UpdateMoneyAsync(request.Id, request.Amount, request.By);
		}
	}
}
