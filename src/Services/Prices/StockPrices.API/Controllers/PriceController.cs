using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

using Hangfire;
using TradingBot.Shared.Messaging.Rabbit;

using TradingBot.Services.StockPrices.API.Application.Models;

namespace TradingBot.Services.StockPrices.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PriceController : ControllerBase
	{
		private readonly IRabbitMQManager _rabbitMQ;
		public PriceController(IRabbitMQManager rabbitMQ)
		{
			_rabbitMQ = rabbitMQ;
		}

		[HttpGet]
		[Route("get")]
		public IActionResult GetPrices([FromQuery(Name = "symbol")] string[] symbols, 
											 [FromBody] Request request)
		{
			var type = request.Type.ToLower() switch
			{
				"sell" => "sell",
				"buy" => "buy",
				_ => "query"
			};

			// create background job

			_rabbitMQ.Publish(new { Stocks = symbols }, "exchange.price", "topic", $"price.{type}");

			return Ok();
		}

		[HttpGet]
		[Route("get/stream")] 
		public IActionResult GetPriceStreams([FromQuery(Name = "symbol")] string[] symbols,
											[FromBody] Request request)
		{
			var type = request.Type.ToLower() switch
			{
				"sell" => "sell",
				"buy" => "buy",
				_ => "query"
			};

			for(int i = 0; i < 3; i++)
				_rabbitMQ.Publish(new { Stocks = symbols }, "exchange.price", "topic", $"price.{type}");

			return Ok();
		}
	}
}
