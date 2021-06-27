using FluentAssertions;
using Renan.GlassLewis.Domain.Company;
using System.Linq;
using Xunit;

namespace Renan.GlassLewis.Domain.Tests.Company
{
    public class IsinTests
    {
        [Theory]
        [InlineData("", CompanyConstants.CompanyIsinMustHaveValue)]
        [InlineData(" ", CompanyConstants.CompanyIsinMustHaveValue)]
        [InlineData(null, CompanyConstants.CompanyIsinMustHaveValue)]
        [InlineData("A", CompanyConstants.CompanyIsinMustBeGreaterThenTwoChars)]
        [InlineData("123456", CompanyConstants.CompanyIsinMustStartWithTwoLetter)]
        [InlineData("A123456", CompanyConstants.CompanyIsinMustStartWithTwoLetter)]
        [InlineData("#$%123456", CompanyConstants.CompanyIsinMustStartWithTwoLetter)]
        public void ValidateIsin_ShouldNotAllow_InvalidIsin(string input, string message)
        {
            // Arrange
            var isin = new CompanyIsin(input);

            //Act
            var result = isin.Validate();

            //Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Select(x => x.ErrorMessage).Should().Contain(message);
        }

        [Theory]
        [InlineData("AB123456")]
        [InlineData("YASLKDJFL")]
        [InlineData("YA@#$%¨&*")]
        public void ValidateIsin_ShouldAllow_InvalidIsin(string input)
        {
            // Arrange
            var isin = new CompanyIsin(input);

            //Act
            var result = isin.Validate();

            //Assert
            result.IsValid.Should().BeTrue();
        }
    }
}