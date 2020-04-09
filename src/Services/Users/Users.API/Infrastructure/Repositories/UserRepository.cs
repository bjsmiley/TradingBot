using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.API.Application.Models;

using MongoDB.Driver;
using System.Linq.Expressions;

namespace Users.API.Infrastructure.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly IMongoCollection<User> _users;

		public UserRepository( IUserDatabaseSettings settings)
		{
			var client = new MongoClient(settings.ConnectionString);

			var db = client.GetDatabase(settings.DatabaseName);

			_users = db.GetCollection<User>(settings.UserCollectionName);

		}

		public Task CreateAsync(User user)
		{
			return _users.InsertOneAsync(user);
		}

		public async Task<bool> DeleteAsync(Guid id)
		{
			var result = await _users.DeleteOneAsync(user => user.Id == id);
			return result.IsAcknowledged;
		}

		public async Task<IEnumerable<User>> GetAllAsync(Expression<Func<User,bool>> filter = null)
		{

			if(filter == null)
			{
				filter = (user) => true;
			}

			var users = await _users.FindAsync(filter);

			return users.ToEnumerable();
		}

		public async Task<User> GetAsync(Guid id)
		{
			var cursor = await _users.FindAsync(u => u.Id == id);
			return await cursor.FirstOrDefaultAsync();
		}

		public async Task<bool> UpdateAsync(User user)
		{
			var result = await _users.ReplaceOneAsync(u => u.Id == user.Id, user);

			return result.IsAcknowledged;
		}
	}
}
