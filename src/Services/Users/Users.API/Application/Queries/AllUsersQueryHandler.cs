using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TradingBot.Services.Users.API.Application.Models;
using TradingBot.Services.Users.API.Domain.Dtos;

namespace TradingBot.Services.Users.API.Application.Queries
{
	public class AllUsersQueryHandler : IRequestHandler<AllUsersQuery, IEnumerable<UserDto>>
	{
		private readonly IUserRepository _userRepository;

		public AllUsersQueryHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public async Task<IEnumerable<UserDto>> Handle(AllUsersQuery request, CancellationToken cancellationToken)
		{
			var users = await _userRepository.GetAllAsync();

			return users.Select(u => new UserDto
			{
				Id = u.Id,
				FirstName = u.FirstName,
				LastName = u.LastName,
				Phone = u.Phone,
				Email = u.Email,
				Created = u.Created
			});
		}
	}
}
