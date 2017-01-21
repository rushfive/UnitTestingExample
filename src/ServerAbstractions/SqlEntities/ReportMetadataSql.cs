using System;

namespace ServerAbstractions.SqlEntities
{
	public class ReportMetadataSql
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public DateTime Created { get; set; }
		public DateTime? LastUpdated { get; set; }
		public Guid OwnerId { get; set; }
		public Guid? LastRevisionById { get; set; }
	}
}
