using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingBot.Services.Wallet.API.Application.Models;
using MongoDB.Driver;

namespace TradingBot.Services.Wallet.API.Infrastructure.Repositories
{
	public class WalletRepository : IWalletRepository
	{

        private readonly IMongoCollection<WalletItem> _wallets;

		public WalletRepository(IWalletDatabaseSettings settings) 
        {
            var client = new MongoClient(settings.ConnectionString);

            var db = client.GetDatabase(settings.DatabaseName);

            _wallets = db.GetCollection<WalletItem>(settings.WalletCollectionName);

        }

        public async Task<WalletItem> GetByOwnerAsync( Guid id )
        {
            var walletCursor = await _wallets.FindAsync(w => w.Owner == id);

            return await walletCursor.FirstOrDefaultAsync();
        }

        public Task CreateAsync( WalletItem wallet )
        {
            return _wallets.InsertOneAsync(wallet);    
        }

        public Task<WalletItem> UpdateMoneyAsync(Guid id, double amount, MoneyUpdate by )
        {

            var filter = Builders<WalletItem>.Filter.Eq(w => w.Id, id);

            amount = by switch
            {
                MoneyUpdate.Adding => amount,
                _ => amount * -1
            };
   
            var updater = Builders<WalletItem>.Update.Inc(w => w.Amount, amount);

            return _wallets.FindOneAndUpdateAsync(filter, updater);

        }

        public async Task<bool> DeleteByIdAsync( Guid id )
        {
            var result = await _wallets.DeleteOneAsync(w => w.Id == id);
            return result.IsAcknowledged;
        }
        public async Task<bool> DeleteByOwnerAsync( Guid id )
        {
            var result = await _wallets.DeleteOneAsync(w => w.Owner == id);
            return result.IsAcknowledged;
        }

        public async Task<IEnumerable<WalletItem>> GetAllAsync()
        {
            var t = await _wallets.FindAsync(w => true);

            return t.ToEnumerable();
            
        }
    }
}
