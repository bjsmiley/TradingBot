using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradingBot.Services.Wallet.API.Application.Models
{
    public interface IWalletRepository
    {
        public Task<IEnumerable<WalletItem>> GetAllAsync();
        public Task<WalletItem> GetByOwnerAsync( Guid id );
        public Task CreateAsync( WalletItem wallet );
        public Task<WalletItem> UpdateMoneyAsync(Guid id, double amount, MoneyUpdate by );
        public Task<bool> DeleteByIdAsync( Guid id );
        public Task<bool> DeleteByOwnerAsync( Guid id );
    }

    public enum MoneyUpdate
    {
        Adding,Subtracting
    }
}
