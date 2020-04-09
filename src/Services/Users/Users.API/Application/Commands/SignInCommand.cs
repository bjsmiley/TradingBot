using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using MediatR;
using Users.API.Domain.Dtos;

namespace Users.API.Application.Commands
{
	public class SignInCommand : IRequest<AuthenticateDto>
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
