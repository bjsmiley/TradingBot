using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using TradingBot.Services.Users.API.Application.Models;
using System.ComponentModel.DataAnnotations;
using TradingBot.Services.Users.API.Domain.Dtos;

namespace TradingBot.Services.Users.API.Application.Queries
{
	public class UserQuery : IRequest<UserDto>
	{
		[Required]
		public Guid Id { get; set; }

	}
}
