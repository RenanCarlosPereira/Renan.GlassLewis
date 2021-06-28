using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Renan.GlassLewis.Application.Authentication;
using Renan.GlassLewis.Application.CompaniesUseCases;
using Renan.GlassLewis.Application.Extentions;
using Xunit;

namespace Renan.GlassLewis.Application.Tests.Extentions
{
    public class ApplicationExtentionTests
    {
        [Fact]
        public void AddDomainServices_RegisterAllRequiredInterfaces()
        {
            // Arrange
            var services = new ServiceCollection();
            IConfiguration config = new ConfigurationBuilder().Build();
            // Act
            var result = services.AddApplicationServices(config);

            // Assert
            using var serviceScope = result.BuildServiceProvider().CreateScope();
            serviceScope.ServiceProvider.GetService<IAuthenticationManager>().Should().NotBeNull();
            serviceScope.ServiceProvider.GetService<ICompanyUseCase>().Should().NotBeNull();
        }
    }
}