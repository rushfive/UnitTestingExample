using System;
using System.Threading.Tasks;
using CoreAbstractions.Entities;

namespace ServerAbstractions.Services
{
	public interface IReportService
	{
		Task<ReportMetadata> GetAsync(Guid reportId);
	}
}
