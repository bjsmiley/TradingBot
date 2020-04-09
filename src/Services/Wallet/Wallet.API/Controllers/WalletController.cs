using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Wallet.API.Application.Models;
using Wallet.API.Application.Commands;
using Wallet.API.Application.Queries;

namespace Wallet.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {

        private readonly ILogger<WalletController> _logger;
        private readonly IWalletRepository _walletRepo;

      

        public WalletController(ILogger<WalletController> logger, IWalletRepository walletRepo)
        {
            _logger = logger;
            _walletRepo = walletRepo;
        }

        #region Queries

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WalletItem>>> GetAll()
        {
             var wallets = await _walletRepo.GetAll();

            return Ok(wallets);
        }

        //[Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetWalletById([FromBody] Guid id)
        {
            var wallet = await _walletRepo.GetWalletById(id);

            if (wallet == null)
                return NotFound();

            return Ok(wallet);
        }

        //[Route("{id}/owner")]
        [Route("owner")]
        [HttpGet]
        public async Task<IActionResult> GetWalletByOwner([FromBody] Guid id)
        {
            var wallet = await _walletRepo.GetWalletByOwner(id);

            if (wallet == null)
                return NotFound();

            return Ok(wallet);
        }

        #endregion

        #region Commands

        //[Route("{id}/owner")]
        [Route("owner")]
        [HttpPost]
        public async Task<IActionResult> CreateNewWallet([FromBody] Guid id)
        {
            if (id == null)
                return NotFound();

            var existing = await _walletRepo.GetWalletByOwner(id);

            if(existing != null)
                return BadRequest("This user already has a wallet. Cannot create new wallet");

            var newWallet = new WalletItem
            {
                Id = Guid.NewGuid(),
                Owner = id,
                Created = DateTime.UtcNow,
            };

            if(await _walletRepo.NewWallet(newWallet))
            {
                //return CreatedAtRoute(nameof(GetWalletById), newWallet.Id, newWallet);
                return Created($"/api/wallet/{newWallet.Id}", newWallet);
            }

            return BadRequest("{\"error\" : \"Couldnt make a new wallet sorry :(\"}");
        }

        [Route("amount")]
        [HttpPatch]
        public async Task<IActionResult> ModifyWalletAmount([FromBody] ModifyWalletAmountCommand command)
        {
            WalletItem w;

            

            if(command.Difference > 0)
                w = await _walletRepo.AddMoney(command.WalletId, command.Difference);
            else
                w = await _walletRepo.RemoveMoney(command.WalletId, command.Difference);

            if (w == null)
                return NotFound();

            return Ok(w);
        }

        //[Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteById([FromBody] Guid id)
        {
            var deletedWallet = await _walletRepo.DeleteWalletById(id);

            if (deletedWallet != null)
                return Ok();
            else
                return BadRequest();
        }

        //[Route("{id}/owner")]
        [Route("owner")]
        [HttpDelete]
        public async Task<IActionResult> DeleteByOwner([FromBody] Guid id)
        {
            var deletedWallet = await _walletRepo.DeleteWalletByOwner(id);

            if (deletedWallet != null)
                return Ok();
            else
                return BadRequest();
        }

        #endregion










    }
}
