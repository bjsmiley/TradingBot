using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Users.API.Application.Commands
{
	public class DeleteCommand : IRequest<bool>
	{
		public Guid UserId { get; set; }
	}
}
