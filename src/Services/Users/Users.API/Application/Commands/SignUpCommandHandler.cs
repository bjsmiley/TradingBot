using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using TradingBot.Services.Users.API.Application.Models;
using TradingBot.Services.Users.API.Domain.Dtos;
using TradingBot.Services.Users.API.Domain.Events;

namespace TradingBot.Services.Users.API.Application.Commands
{
	public class SignUpCommandHandler : IRequestHandler<SignUpCommand, AuthenticateDto>
	{
		private readonly IUserRepository _userRepository;
		private readonly IMediator _mediator;
		private readonly Global _globalSettings;

		public SignUpCommandHandler(IUserRepository userRepository, Global globalSettings , IMediator mediator)
		{
			_userRepository = userRepository ?? throw new ArgumentNullException("userService");
			_mediator = mediator ?? throw new ArgumentNullException("mediator");
			_globalSettings = globalSettings ?? throw new ArgumentNullException("globalSettings");
		}

		public async Task<AuthenticateDto> Handle(SignUpCommand request, CancellationToken cancellationToken)
		{
			var usersWithSameEmail = await _userRepository.GetAllAsync(u => u.Email == request.Email);

			if (usersWithSameEmail.Count() > 0)
				return null;


			var user = new User
			{
				Id = Guid.NewGuid(),
				FirstName = request.FirstName,
				LastName = request.LastName,
				Email = request.Email,
				Phone = request.Phone,
				Password = request.Password,
				Created = DateTime.UtcNow,
			};


			var task =  _userRepository.CreateAsync(user);

			//var userDto = new UserDto
			//{
			//	Id = user.Id,
			//	FirstName = user.FirstName,
			//	LastName = user.LastName,
			//	Email = user.Email,
			//	Phone = user.Phone,
			//	Created = user.Created
			//};

			// get token [start]
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_globalSettings.Secret);
			
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			// get token [end]

			var identity = new AuthenticateDto
			{
				UserId = user.Id,
				Token = tokenHandler.WriteToken(token)
			};

			await task;

			return identity;


		}
	}
}
