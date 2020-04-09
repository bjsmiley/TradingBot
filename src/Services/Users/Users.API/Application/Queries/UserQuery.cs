using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Users.API.Application.Models;
using System.ComponentModel.DataAnnotations;
using Users.API.Domain.Dtos;

namespace Users.API.Application.Queries
{
	public class UserQuery : IRequest<UserDto>
	{
		[Required]
		public Guid Id { get; set; }

	}
}
