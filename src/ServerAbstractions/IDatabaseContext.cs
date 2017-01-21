using System;
using System.Threading.Tasks;

namespace ServerAbstractions
{
	public interface IDatabaseContext
	{
		Task<T> FindSingleAsync<T>(Guid id);
	}
}
