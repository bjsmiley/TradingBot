using System;

namespace TradingBot.Services.Users.API.Application.Models
{
	public class User
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Password { get; set; }
		public DateTime Created { get; set; }
	}
}
