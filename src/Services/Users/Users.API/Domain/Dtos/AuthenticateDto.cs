﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingBot.Services.Users.API.Domain.Dtos
{
	public class AuthenticateDto
	{
		public Guid UserId { get; set; }
		public string Token { get; set; }
	}
}
