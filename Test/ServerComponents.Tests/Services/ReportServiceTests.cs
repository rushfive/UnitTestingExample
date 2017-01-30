using System.Threading.Tasks;
using Moq;
using ServerAbstractions;
using ServerAbstractions.Services;
using ServerComponents.Services;
using TestsCore;

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

	    public async Task GetAsync_InvalidArgument_ThrowsArgumentException()
	    {
		    
	    }

	    public async Task GetAsync_ReportNotFound_ThrowsException()
	    {
		    
	    }

	    public async Task GetAsync_ExpiredReport_ThrowsException()
	    {
		    
	    }

        public async Task GetAsync_OwnerNotFound_ThrowsException()
        {

        }

	    public async Task GetAsync_OwnerDisabled_ThrowsException()
	    {
		    
	    }

	    public async Task GetAsync_Success_Updated_ReturnsCorrectResult()
	    {
		    
	    }

	    public async Task GetAsync_Success_NotUpdated_ReturnsCorrectResult()
	    {
		    
	    }
	}
}
