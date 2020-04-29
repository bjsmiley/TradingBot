using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TradingBot.Services.Wallet.API.Application.Models;

namespace TradingBot.Services.Wallet.API.Application.Commands
{
	public class CreateCommandHandler : IRequestHandler<CreateCommand, WalletItem>
	{
		private readonly IWalletRepository _repo;

		public CreateCommandHandler(IWalletRepository repo)
		{
			_repo = repo;
		}
		public async Task<WalletItem> Handle(CreateCommand request, CancellationToken cancellationToken)
		{
			var alreadyOwner = await _repo.GetByOwnerAsync(request.Owner);

			if (alreadyOwner != null)
				return null;

			var newWallet = new WalletItem
			{
				Id = Guid.NewGuid(),
				Owner = request.Owner,
				Created = DateTime.UtcNow,
			};

			await _repo.CreateAsync(newWallet);

			return newWallet;



		}
	}
}
