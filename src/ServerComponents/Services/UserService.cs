using System;
using System.Threading.Tasks;
using CoreAbstractions.Entities;
using ServerAbstractions;
using ServerAbstractions.Services;

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

		    User user = await this.DbContext.FindSingleAsync<User>(userId);

		    if (user == null)
		    {
			    throw new Exception($"User {userId} was not found.");
		    }

		    return user;
	    }
    }
}
