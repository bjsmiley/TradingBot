using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TradingBot.Services.Users.API.Application.Models
{
	public interface IUserRepository
	{
		public Task<IEnumerable<User>> GetAllAsync(Expression<Func<User,bool>> filter = null);
		public Task<User> GetAsync(Guid id);
		public Task CreateAsync(User user);
		public Task<bool> UpdateAsync(User user);
		public Task<bool> DeleteAsync(Guid id);
	}
}
