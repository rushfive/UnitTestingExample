using System;
using System.Threading.Tasks;
using CoreAbstractions.Entities;

namespace ServerAbstractions.Services
{
	public interface IUserService
	{
		Task<User> GetAsync(Guid userId);
	}
}
