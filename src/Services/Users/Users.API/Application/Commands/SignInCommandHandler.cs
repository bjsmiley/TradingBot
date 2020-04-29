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

namespace TradingBot.Services.Users.API.Application.Commands
{
	public class SignInCommandHandler : IRequestHandler<SignInCommand, AuthenticateDto>
	{
		private readonly IUserRepository _userRepository;
		private readonly Global _globalSettings;

		public SignInCommandHandler(IUserRepository userRepository, Global globalSettings)
		{
			_userRepository = userRepository ?? throw new ArgumentNullException("userService");
			_globalSettings = globalSettings ?? throw new ArgumentNullException("globalSettings");
		}

		public async Task<AuthenticateDto> Handle(SignInCommand request, CancellationToken cancellationToken)
		{
			var users = await _userRepository.GetAllAsync(u => u.Email == request.Email && u.Password == request.Password);

			var user = users.FirstOrDefault();

			if (user == null) return null;

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

			return new AuthenticateDto
			{
				UserId = user.Id,
				Token = tokenHandler.WriteToken(token)
			};
		}
	}
}
