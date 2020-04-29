using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TradingBot.Services.Users.API.Application.Models;
using TradingBot.Services.Users.API.Application.Commands;
using TradingBot.Services.Users.API.Application.Queries;
using Microsoft.AspNetCore.Authorization;
using TradingBot.Services.Users.API.Domain.Dtos;
using TradingBot.Services.Users.API.Domain.Events;
using System.Security.Claims;

namespace TradingBot.Services.Users.API.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public UserController(ILogger<UserController> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException("logger");
            _mediator = mediator ?? throw new ArgumentNullException("mediator");
        }

        [Route("signup")]
        //[AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SignUpAsync([FromBody] SignUpCommand command)
        {
            var authenticateDto = await _mediator.Send(command);

            if (authenticateDto == null)
                return BadRequest(new { error = "Email is already in use." });

            await _mediator.Publish(new UserCreatedEvent(authenticateDto.UserId));

            return Ok(authenticateDto);

        }

        [Route("signin")]
        //[AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> SignInAsync([FromBody] SignInCommand command)
        {
            var authenticateDto = await _mediator.Send(command);

            if (authenticateDto == null)
                return BadRequest(new { message = "Email/Password incorrect." });

            return Ok(authenticateDto);
        }

        [Route("all")]
        [HttpGet]
        //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllAsync()
        {
            var users = await _mediator.Send(new AllUsersQuery());

            return Ok(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromBody] UserQuery query)
        {
            var r = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _mediator.Send(query));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result) BadRequest(new { error = "Could not delete user." });

            await _mediator.Publish(new UserDeletedEvent(command.UserId));

            return Ok();
        }
    }
}
