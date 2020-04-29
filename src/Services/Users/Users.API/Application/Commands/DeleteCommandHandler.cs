using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TradingBot.Services.Users.API.Application.Models;
using TradingBot.Services.Users.API.Domain.Events;

namespace TradingBot.Services.Users.API.Application.Commands
{
	public class DeleteCommandHandler : IRequestHandler<DeleteCommand, bool>
	{
		private readonly IUserRepository _userRepository;
		private readonly IMediator _mediator;

		public DeleteCommandHandler(IUserRepository userRepository, IMediator mediator)
		{
			_mediator = mediator;
			_userRepository = userRepository;
		}
		public Task<bool> Handle(DeleteCommand request, CancellationToken cancellationToken)
		{
			return _userRepository.DeleteAsync(request.UserId);
		}
	}
}
