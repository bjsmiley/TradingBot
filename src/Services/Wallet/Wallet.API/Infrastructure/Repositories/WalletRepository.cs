using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wallet.API.Models;
using MongoDB.Driver;

namespace Wallet.API.Infrastructure.Repositories
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

		public async Task<WalletItem> GetWalletById( Guid id )
        {
            var walletCursor = await _wallets.FindAsync(w => w.Id == id);

            return await walletCursor.FirstOrDefaultAsync();

        }
        public async Task<WalletItem> GetWalletByOwner( Guid id )
        {
            var walletCursor = await _wallets.FindAsync(w => w.Owner == id);

            return await walletCursor.FirstOrDefaultAsync();
        }

        public Task<bool> NewWallet( WalletItem wallet )
        {
            if(wallet != null)
            {
                _wallets.InsertOneAsync(wallet);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
        public Task<WalletItem> AddMoney(Guid id, decimal amount )
        {

            var filter = Builders<WalletItem>.Filter.Eq(w => w.Id, id);

            var updater = Builders<WalletItem>.Update.Inc(w => w.Amount, amount);

            return _wallets.FindOneAndUpdateAsync(filter, updater);

        }
        public Task<WalletItem> RemoveMoney(Guid id, decimal amount )
        {
            return AddMoney(id, amount * -1);
        }
        public Task<WalletItem> DeleteWalletById( Guid id )
        {
            return _wallets.FindOneAndDeleteAsync(w => w.Id == id);
        }
        public Task<WalletItem> DeleteWalletByOwner( Guid id )
        {
            return _wallets.FindOneAndDeleteAsync(w => w.Owner == id);
        }

        public async Task<IEnumerable<WalletItem>> GetAll()
        {
            var t = await _wallets.FindAsync(w => true);

            return t.ToEnumerable();
            
        }
    }
}
