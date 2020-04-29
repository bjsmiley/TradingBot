using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediatR;

using TradingBot.Services.Wallet.API.Application.Models;
using TradingBot.Services.Wallet.API.Application.Commands;

namespace TradingBot.Services.Wallet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {

        private readonly ILogger<WalletController> _logger;
        private readonly IWalletRepository _walletRepo;
        private readonly IMediator _mediator;

      

        public WalletController(ILogger<WalletController> logger, IWalletRepository walletRepo, IMediator mediator)
        {
            _logger = logger;
            _walletRepo = walletRepo;
            _mediator = mediator;
        }

        [Route("owner")]
        [HttpGet]
        public async Task<IActionResult> GetWalletByOwner([FromBody] Guid id)
        {
            var wallet = await _walletRepo.GetByOwnerAsync(id);

            if (wallet == null)
                return NotFound();

            return Ok(wallet);
        }

        [Route("owner")]
        [HttpPost]
        public async Task<IActionResult> CreateNewWallet([FromBody] CreateCommand command)
        {

            var result = await _mediator.Send(command);

            if (result == null)
                return BadRequest(new { Error = "User already owns a wallet." });

            return Ok(result);
        }

        [Route("amount/add")]
        [HttpPatch]
        public async Task<IActionResult> AddWalletAmount([FromBody] UpdateMoneyCommand command)
        {
            command.By = MoneyUpdate.Adding;

            var wallet = await _mediator.Send(command);

            if (wallet == null)
                return NotFound();
            else
                return Ok(wallet);
        }

        [Route("amount/sub")]
        [HttpPatch]
        public async Task<IActionResult> SubWalletAmount([FromBody] UpdateMoneyCommand command)
        {
            command.By = MoneyUpdate.Subtracting;

            var wallet = await _mediator.Send(command);

            if (wallet == null)
                return NotFound();
            else
                return Ok(wallet);
        }



        [HttpDelete]
        public async Task<IActionResult> DeleteById([FromBody] DeleteByIdCommand command)
        {

            var success = await _mediator.Send(command);

            if (success)
                return Ok();
            else
                return NotFound();
        }

        [Route("owner")]
        [HttpDelete]
        public async Task<IActionResult> DeleteByOwner([FromBody] DeleteByOwnerCommand command)
        {
            var success = await _mediator.Send(command);

            if (success)
                return Ok();
            else
                return NotFound();
        }
    }
}
