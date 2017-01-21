using System;
using CoreAbstractions.Entities;

namespace ServerAbstractions.SqlEntities
{
	public class UserSql
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool Enabled { get; set; }

		public static User ToEntity(UserSql sql)
		{
			return new User
			{
				Id = sql.Id,
				FirstName = sql.FirstName,
				LastName = sql.LastName,
				Enabled = sql.Enabled
			};
		}
	}
}


