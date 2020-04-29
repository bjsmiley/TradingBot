using System;

namespace TradingBot.Services.Wallet.API.Application.Commands
{
    public class ModifyWalletAmountCommand
    {
        public Guid WalletId { get; set; }
        public decimal OldAmount { get; set; }
        public decimal NewAmount { get; set; }
        public decimal Difference { get; set; } 
    }
}
