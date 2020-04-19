using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

using Hangfire;
using TradingBot.Shared.Messaging.Rabbit;
using MediatR;

using TradingBot.Services.StockPrices.API.Application.Models;
using TradingBot.Services.StockPrices.API.Application.Queries;

namespace TradingBot.Services.StockPrices.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PriceController : ControllerBase
	{
		private readonly IRabbitMQManager _rabbitMQ;
		private readonly IMediator _mediator;
		private readonly AlphaVantageSettings _default;

		public PriceController(IRabbitMQManager rabbitMQ, IMediator mediator, AlphaVantageSettings defaultAlpha)
		{
			_rabbitMQ = rabbitMQ;
			_mediator = mediator;
			_default = defaultAlpha;
		}

		[HttpGet]
		[Route("demo/get")]
		public async Task<IActionResult> GetPricesDemo(	[FromQuery(Name = "symbol")] string[] symbols,
													[FromQuery(Name = "type")] string type = "query")
		{
			type = type.ToLower() switch
			{
				"sell" => "sell",
				"buy" => "buy",
				_ => "query"
			};

			var query = new PriceQuery
			{
				Symbols = symbols,
				ApiKey = _default.ApiKey,
				Type = type
			};

			await _mediator.Send(query);

			return Ok();
		}

		[HttpGet]
		[Route("get")]
		public async Task<IActionResult> GetPrices( [FromBody] Request request,
											  [FromQuery(Name = "symbol")] string[] symbols, 
											  [FromQuery(Name = "type")] string type = "query" )
		{

			if (string.IsNullOrEmpty(request.ApiKey))
				return BadRequest();
			
			type = type.ToLower() switch
			{
				"sell" => "sell",
				"buy" => "buy",
				_ => "query"
			};

			var query = new PriceQuery
			{
				ApiKey = request.ApiKey,
				Symbols = symbols,
				Type = type
			};

			await _mediator.Send(query);

			return Ok();



		}

	}
}
