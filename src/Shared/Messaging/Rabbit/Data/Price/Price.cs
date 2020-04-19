using System;
using System.Collections.Generic;
using System.Text;

namespace TradingBot.Shared.Messaging.Rabbit.Data.Price
{
	public class Price
	{
		public string Symbol { get; set; }
		public double Value { get; set; }
		public DateTime Timestamp { get; set; }
	}
}
