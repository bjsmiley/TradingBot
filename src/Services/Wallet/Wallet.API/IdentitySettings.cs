﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingBot.Services.Wallet.API
{
	public class IdentitySettings : IIdentitySettings
	{
		public string ConnectionString { get; set; }
	}

	public interface IIdentitySettings
	{
		public string ConnectionString { get; set; }
	}
}
