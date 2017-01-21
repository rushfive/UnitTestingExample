using System;

namespace CoreAbstractions.Entities
{
	public class User
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public bool Enabled { get; set; }
	}
}
