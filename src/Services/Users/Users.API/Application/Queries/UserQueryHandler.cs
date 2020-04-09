using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Users.API.Application.Models;
using Users.API.Domain.Dtos;

namespace Users.API.Application.Queries
{
	public class UserQueryHandler : IRequestHandler<UserQuery, UserDto>
	{
		private readonly IUserRepository _userRepository;

		public UserQueryHandler(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}
		public async Task<UserDto> Handle(UserQuery request, CancellationToken cancellationToken)
		{
			var user = await _userRepository.GetAsync(request.Id);

			if (user == null) return null;

			return new UserDto
			{ 
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				Phone = user.Phone,
				Created = user.Created,
			};

		}
	}
}
