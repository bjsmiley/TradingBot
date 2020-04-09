using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Users.API.Application.Models;
using Users.API.Domain.Dtos;

namespace Users.API.Application.Queries
{
	public class AllUsersQuery : IRequest<IEnumerable<UserDto>>
	{
	}
}
