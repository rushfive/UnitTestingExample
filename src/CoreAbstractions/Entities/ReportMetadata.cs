using System;

namespace CoreAbstractions.Entities
{
	public class ReportMetadata
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public DateTime Created { get; set; }
		public DateTime? LastUpdated { get; set; }
		public string AuthorFullName { get; set; }
		public Guid? LastRevisionById { get; set; }
	}
}
