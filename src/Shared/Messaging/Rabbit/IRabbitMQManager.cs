using System;
using System.Collections.Generic;
using System.Text;

namespace TradingBot.Shared.Messaging.Rabbit
{
	public interface IRabbitMQManager
	{
		void Publish<T>(T message, string exchangeName, string exchangeType, string routeKey) where T : class;
	}
}
