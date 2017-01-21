using ServerComponents.Services;
using System;
using System.Threading.Tasks;
using CoreAbstractions.Entities;
using Moq;
using ServerAbstractions;
using ServerAbstractions.SqlEntities;
using TestsCore;
using Xunit;

namespace ServerComponents.Tests.Services
{
    public class UserServiceTests
    {
	    private TestingObject<UserService> GetTestingObject()
	    {
		    var testingObject = new TestingObject<UserService>();
			testingObject.AddDependency(new Mock<IDatabaseContext>(MockBehavior.Strict));
		    return testingObject;
	    }

		[Fact]
	    public async Task GetAsync_InvalidArgument_ThrowsArgumentException()
		{
			TestingObject<UserService> testingObject = this.GetTestingObject();
			UserService userService = testingObject.GetResolvedTestingObject();

			await Assert.ThrowsAsync<ArgumentException>(async () =>
				await userService.GetAsync(Guid.Empty));
		}

		[Fact]
	    public async Task GetAsync_UserNotFound_ThrowsException()
	    {
			TestingObject<UserService> testingObject = this.GetTestingObject();
			UserService userService = testingObject.GetResolvedTestingObject();
			var mockDbContext = testingObject.GetDependency<Mock<IDatabaseContext>>();

			Guid userIdArg = Guid.NewGuid();

		    mockDbContext
			    .Setup(dbc => dbc.FindSingleAsync<UserSql>(It.Is<Guid>(id => id == userIdArg)))
			    .ReturnsAsync(null);

		    await Assert.ThrowsAsync<Exception>(async ()
			    => await userService.GetAsync(userIdArg));
	    }

		[Fact]
		public async Task GetAsync_Success_ReturnsCorrectResult()
		{
			TestingObject<UserService> testingObject = this.GetTestingObject();
			UserService userService = testingObject.GetResolvedTestingObject();
			var mockDbContext = testingObject.GetDependency<Mock<IDatabaseContext>>();

			Guid userIdArg = Guid.NewGuid();

			var userSql = new UserSql
			{
				FirstName = "Mary",
				LastName = "Jane"
			};
			mockDbContext
				.Setup(dbc => dbc.FindSingleAsync<UserSql>(It.Is<Guid>(id => id == userIdArg)))
				.ReturnsAsync(userSql);

			User result = await userService.GetAsync(userIdArg);

			Assert.Equal("Mary", result.FirstName);
			Assert.Equal("Jane", result.LastName);
		}
	}
}
