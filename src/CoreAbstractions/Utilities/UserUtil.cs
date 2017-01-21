using System;
using CoreAbstractions.Entities;

namespace CoreAbstractions.Utilities
{
	public static class UserUtil
	{
		public static string GetFullName(User user)
		{
			if (string.IsNullOrWhiteSpace(user.FirstName))
			{
				throw new Exception("First name must be provided.");
			}
			if (string.IsNullOrWhiteSpace(user.LastName))
			{
				throw new Exception("Last name must be provided.");
			}

			return $"{user.FirstName} {user.LastName}".Trim();
		}
	}
}
