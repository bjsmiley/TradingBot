using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradingBot.Services.Wallet.API.Application.Models
{
    public interface IWalletRepository
    {
        public Task<IEnumerable<WalletItem>> GetAll();
        public Task<WalletItem> GetWalletById( Guid id );
        public Task<WalletItem> GetWalletByOwner( Guid id );
        public Task<bool> NewWallet( WalletItem wallet );
        public Task<WalletItem> AddMoney(Guid id, decimal amount );
        public Task<WalletItem> RemoveMoney(Guid id, decimal amount );
        public Task<WalletItem> DeleteWalletById( Guid id );
        public Task<WalletItem> DeleteWalletByOwner( Guid id );
    }
}
