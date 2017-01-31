using System;
using System.Threading.Tasks;
using Moq;
using ServerAbstractions;
using ServerAbstractions.Services;
using ServerComponents.Services;
using TestsCore;
using Xunit;
using ServerAbstractions.SqlEntities;
using CoreAbstractions.Entities;

namespace ServerComponents.Tests.Services
{
    public class ReportServiceTests
    {
		private TestingObject<ReportService> GetTestingObject()
		{
			var testingObject = new TestingObject<ReportService>();
			testingObject.AddDependency(new Mock<IUserService>(MockBehavior.Strict));
			testingObject.AddDependency(new Mock<IDatabaseContext>(MockBehavior.Strict));
			return testingObject;
		}

        [Fact]
	    public async Task GetAsync_InvalidArgument_ThrowsArgumentException()
	    {
            TestingObject<ReportService> testingObject = this.GetTestingObject();

            ReportService reportService = testingObject.GetResolvedTestingObject();

            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await reportService.GetAsync(Guid.Empty));
        }

        [Fact]
        public async Task GetAsync_ReportNotFound_ThrowsException()
	    {
            TestingObject<ReportService> testingObject = this.GetTestingObject();

            Guid reportIdArg = Guid.NewGuid();

            var mockDbContext = testingObject.GetDependency<Mock<IDatabaseContext>>();
            mockDbContext
                .Setup(m => m.FindSingleAsync<ReportMetadataSql>(It.Is<Guid>(id => id == reportIdArg)))
                .ReturnsAsync(null);

            ReportService reportService = testingObject.GetResolvedTestingObject();
            await Assert.ThrowsAsync<Exception>(async ()
               => await reportService.GetAsync(reportIdArg));
        }

        [Fact]
        public async Task GetAsync_ExpiredReport_ThrowsException()
	    {
            TestingObject<ReportService> testingObject = this.GetTestingObject();

            Guid reportIdArg = Guid.NewGuid();

            var mockDbContext = testingObject.GetDependency<Mock<IDatabaseContext>>();

            var report = new ReportMetadataSql
            {
                Created = DateTime.UtcNow.AddYears(-4)
            };
            mockDbContext
                .Setup(m => m.FindSingleAsync<ReportMetadataSql>(It.Is<Guid>(id => id == reportIdArg)))
                .ReturnsAsync(report);

            ReportService reportService = testingObject.GetResolvedTestingObject();
            await Assert.ThrowsAsync<Exception>(async ()
               => await reportService.GetAsync(reportIdArg));
        }

        [Fact]
        public async Task GetAsync_OwnerNotFound_ThrowsException()
        {
            TestingObject<ReportService> testingObject = this.GetTestingObject();

            Guid reportIdArg = Guid.NewGuid();

            var mockDbContext = testingObject.GetDependency<Mock<IDatabaseContext>>();

            var report = new ReportMetadataSql
            {
                Created = DateTime.UtcNow.AddYears(-2),
                OwnerId = Guid.NewGuid()
            };
            mockDbContext
                .Setup(m => m.FindSingleAsync<ReportMetadataSql>(It.Is<Guid>(id => id == reportIdArg)))
                .ReturnsAsync(report);

            mockDbContext
                .Setup(m => m.FindSingleAsync<UserSql>(It.Is<Guid>(id => id == report.OwnerId)))
                .ReturnsAsync(null);

            ReportService reportService = testingObject.GetResolvedTestingObject();
            await Assert.ThrowsAsync<Exception>(async ()
               => await reportService.GetAsync(reportIdArg));
        }

        [Fact]
        public async Task GetAsync_OwnerDisabled_ThrowsException()
	    {
            TestingObject<ReportService> testingObject = this.GetTestingObject();

            Guid reportIdArg = Guid.NewGuid();

            var mockDbContext = testingObject.GetDependency<Mock<IDatabaseContext>>();

            var report = new ReportMetadataSql
            {
                Created = DateTime.UtcNow.AddYears(-2),
                OwnerId = Guid.NewGuid()
            };
            mockDbContext
                .Setup(m => m.FindSingleAsync<ReportMetadataSql>(It.Is<Guid>(id => id == reportIdArg)))
                .ReturnsAsync(report);

            var owner = new UserSql
            {
                Enabled = false
            };
            mockDbContext
                .Setup(m => m.FindSingleAsync<UserSql>(It.Is<Guid>(id => id == report.OwnerId)))
                .ReturnsAsync(owner);

            ReportService reportService = testingObject.GetResolvedTestingObject();
            await Assert.ThrowsAsync<Exception>(async ()
               => await reportService.GetAsync(reportIdArg));
        }

        [Fact]
        public async Task GetAsync_Success_Updated_ReturnsCorrectResult()
	    {
            TestingObject<ReportService> testingObject = this.GetTestingObject();

            Guid reportIdArg = Guid.NewGuid();

            var mockDbContext = testingObject.GetDependency<Mock<IDatabaseContext>>();

            var report = new ReportMetadataSql
            {
                Created = DateTime.UtcNow.AddYears(-2),
                OwnerId = Guid.NewGuid(),
                LastUpdated = DateTime.UtcNow,
                Title = "Report Title"
            };
            mockDbContext
                .Setup(m => m.FindSingleAsync<ReportMetadataSql>(It.Is<Guid>(id => id == reportIdArg)))
                .ReturnsAsync(report);

            var owner = new UserSql
            {
                Enabled = true,
                FirstName = "Mary",
                LastName = "Jane"
            };
            mockDbContext
                .Setup(m => m.FindSingleAsync<UserSql>(It.Is<Guid>(id => id == report.OwnerId)))
                .ReturnsAsync(owner);

            ReportService reportService = testingObject.GetResolvedTestingObject();

            ReportMetadata result = await reportService.GetAsync(reportIdArg);

            Assert.EndsWith("(Revision)", result.Title);
        }

        [Fact]
        public async Task GetAsync_Success_NotUpdated_ReturnsCorrectResult()
	    {
            TestingObject<ReportService> testingObject = this.GetTestingObject();

            Guid reportIdArg = Guid.NewGuid();

            var mockDbContext = testingObject.GetDependency<Mock<IDatabaseContext>>();

            var report = new ReportMetadataSql
            {
                Created = DateTime.UtcNow.AddYears(-2),
                OwnerId = Guid.NewGuid(),
                Title = "Report Title"
            };
            mockDbContext
                .Setup(m => m.FindSingleAsync<ReportMetadataSql>(It.Is<Guid>(id => id == reportIdArg)))
                .ReturnsAsync(report);

            var owner = new UserSql
            {
                Enabled = true,
                FirstName = "Mary",
                LastName = "Jane"
            };
            mockDbContext
                .Setup(m => m.FindSingleAsync<UserSql>(It.Is<Guid>(id => id == report.OwnerId)))
                .ReturnsAsync(owner);

            ReportService reportService = testingObject.GetResolvedTestingObject();

            ReportMetadata result = await reportService.GetAsync(reportIdArg);

            Assert.EndsWith("(Original)", result.Title);
        }
	}
}
