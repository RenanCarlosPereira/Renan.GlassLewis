using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Renan.GlassLewis.Domain.Company;
using Renan.GlassLewis.Domain.Extensions;
using Xunit;

namespace Renan.GlassLewis.Domain.Tests.Extensions
{
    public class DomainExtensionsTests
    {
        [Fact]
        public void AddDomainServices_RegisterAllRequiredInterfaces()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddDomainServices();

            // Assert
            using var serviceScope = result.BuildServiceProvider().CreateScope();
            serviceScope.ServiceProvider.GetService<ICompanyDomainService>().Should().NotBeNull();
        }
    }
}