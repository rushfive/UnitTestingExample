using System;
using System.Threading.Tasks;
using ServerAbstractions;

namespace ServerComponents
{
	public class DatabaseContext : IDatabaseContext
	{
		public async Task<T> FindSingleAsync<T>(Guid id)
		{
			// pretending to find the correct entity of type T
			// from the database layer and returning it
			var findSingleTask = Task<T>.Factory
				.StartNew(() => (T)Activator.CreateInstance(typeof(T)));

			return await findSingleTask;
		}
	}
}
