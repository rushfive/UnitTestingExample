using System;
using CoreAbstractions.Utilities;
using Xunit;

namespace CoreAbstractions.Tests.Utilities
{
    public class UserUtilTests
    {
		[Fact]
	    public void GetFullName_InvalidArguments_ThrowsArgumentException()
		{
			Assert.Throws<ArgumentException>(() => UserUtil.GetFullName("", ""));
			Assert.Throws<ArgumentException>(() => UserUtil.GetFullName("Mary", ""));
		}

	    [Fact]
	    public void GetFullName_ValidInputs_ReturnsCorrectResult_Example1()
	    {
			Assert.Equal("Mary Jane", UserUtil.GetFullName(" Mary ", " Jane   "));
	    }

		[Theory]
		[InlineData(" Mary", "  Jane  ", "Mary Jane")]
		[InlineData(" Bob", "Marley", "Bob Marley")]
		[InlineData(" Joe Hanson", " Lee   ", "Joe Hanson Lee")]
	    public void GetFullName_ValidInputs_ReturnsCorrectResult_Example2(string firstName, 
			string lastName, string expectedFullName)
		{
			string fullNameResult = UserUtil.GetFullName(firstName, lastName);

			Assert.Equal(expectedFullName, fullNameResult);
	    }
	}
}
