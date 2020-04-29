using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TradingBot.Services.Wallet.API.Application.Models;
using MediatR;

namespace TradingBot.Services.Wallet.API.Application.Commands
{
	public class DeleteByIdCommandHandler : IRequestHandler<DeleteByIdCommand, bool>
	{
		private readonly IWalletRepository _repo;

		public DeleteByIdCommandHandler(IWalletRepository repo)
		{
			_repo = repo;
		}
		public Task<bool> Handle(DeleteByIdCommand request, CancellationToken cancellationToken)
		{
			return _repo.DeleteByIdAsync(request.Id);
		}
	}
}
