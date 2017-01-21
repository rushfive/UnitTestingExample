using System;
using System.Threading.Tasks;
using CoreAbstractions.Entities;
using ServerAbstractions;
using ServerAbstractions.Services;
using ServerAbstractions.SqlEntities;

namespace ServerComponents.Services
{
	public class UserService : IUserService
	{
		private IDatabaseContext DbContext { get; }

		public UserService(IDatabaseContext dbContext)
		{
			this.DbContext = dbContext;
		}

		public async Task<User> GetAsync(Guid userId)
		{
			if (userId == Guid.Empty)
			{
				throw new ArgumentException("Must specify a user id.", nameof(userId));
			}

			UserSql userSql = await this.DbContext.FindSingleAsync<UserSql>(userId);

			if (userSql == null)
			{
				throw new Exception($"User '{userId}' was not found.");
			}

			return UserSql.ToEntity(userSql);
		}
	}
}
