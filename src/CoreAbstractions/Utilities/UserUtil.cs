using System;
using CoreAbstractions.Entities;

namespace CoreAbstractions.Utilities
{
	public static class UserUtil
	{
		public static string GetFullName(string firstName, string lastName)
		{
			if (string.IsNullOrWhiteSpace(firstName))
			{
				throw new ArgumentException("First name must be provided.");
			}
			if (string.IsNullOrWhiteSpace(lastName))
			{
				throw new ArgumentException("Last name must be provided.");
			}

			return $"{firstName.Trim()} {lastName.Trim()}".Trim();
		}
	}
}
