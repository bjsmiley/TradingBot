using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TradingBot.Services.Wallet.API.Application.Models;

namespace TradingBot.Services.Wallet.API.Application.Commands
{
	public class DeleteByOwnerCommandHandler : IRequestHandler<DeleteByOwnerCommand, bool>
	{
		private readonly IWalletRepository _repo;

		public DeleteByOwnerCommandHandler(IWalletRepository repo)
		{
			_repo = repo;
		}
		public Task<bool> Handle(DeleteByOwnerCommand request, CancellationToken cancellationToken)
		{
			return _repo.DeleteByOwnerAsync(request.Owner);	
		}
	}
}
