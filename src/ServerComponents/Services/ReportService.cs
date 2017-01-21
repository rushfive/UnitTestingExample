using System;
using System.Threading.Tasks;
using CoreAbstractions.Entities;
using CoreAbstractions.Utilities;
using ServerAbstractions;
using ServerAbstractions.Services;
using ServerAbstractions.SqlEntities;

namespace ServerComponents.Services
{
	public class ReportService : IReportService
	{
		private IUserService UserService { get; }
		private IDatabaseContext DbContext { get; }

		public ReportService(
			IUserService userService,
			IDatabaseContext dbContext)
		{
			this.UserService = userService;
			this.DbContext = dbContext;
		}

		public async Task<ReportMetadata> GetAsync(Guid reportId)
		{
			if (reportId == Guid.Empty)
			{
				throw new ArgumentException("Report id must be provided", nameof(reportId));
			}

			ReportMetadataSql reportSql = await this.DbContext.FindSingleAsync<ReportMetadataSql>(reportId);
			if (reportSql == null)
			{
				throw new Exception($"Report '{reportId}' was not found.");
			}


			// if report is older than 3 years, dont return it
			if (reportSql.Created.AddYears(3) < DateTime.UtcNow)
			{
				throw new Exception($"Report '{reportId}' is too old and will not be retrieved.");
			}

			UserSql ownerSql = await this.DbContext.FindSingleAsync<UserSql>(reportSql.OwnerId);
			if (ownerSql == null)
			{
				throw new Exception($"User '{reportSql.OwnerId}' not found.");
			}


			// if owner of report is disabled, dont return it
			if (!ownerSql.Enabled)
			{
				throw new Exception($"Report '{reportId}' is owned by a disabled user and will not be retrieved.");
			}

			User owner = UserSql.ToEntity(ownerSql);
			string authorFullName = UserUtil.GetFullName(owner.FirstName, owner.LastName);

			// modify title based on whether it's been updated
			if (reportSql.LastUpdated.HasValue)
			{
				reportSql.Title += " (Revision)";
			}
			else
			{
				reportSql.Title += " (Original)";
			}

			return new ReportMetadata
			{
				Id = reportSql.Id,
				LastUpdated = reportSql.LastUpdated,
				LastRevisionById = reportSql.LastRevisionById,
				Title = reportSql.Title,
				Created = reportSql.Created,
				AuthorFullName = authorFullName
			};
		}
	}
}
