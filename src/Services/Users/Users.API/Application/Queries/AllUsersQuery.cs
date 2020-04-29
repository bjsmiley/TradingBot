using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using TradingBot.Services.Users.API.Application.Models;
using TradingBot.Services.Users.API.Domain.Dtos;

namespace TradingBot.Services.Users.API.Application.Queries
{
	public class AllUsersQuery : IRequest<IEnumerable<UserDto>>
	{
	}
}
