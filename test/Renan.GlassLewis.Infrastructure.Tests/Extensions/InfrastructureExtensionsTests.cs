using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Renan.GlassLewis.Domain.Company;
using Renan.GlassLewis.Domain.Repositories;
using Renan.GlassLewis.Infrastructure.DbContexts;
using Renan.GlassLewis.Infrastructure.Extensions;
using Xunit;

namespace Renan.GlassLewis.Infrastructure.Tests.Extensions
{
    public class InfrastructureExtensionsTests
    {
        private readonly Fixture _fixture;

        public InfrastructureExtensionsTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void AddEntityFrameworkSqlServer_RegisterAllRequiredInterfaces()
        {
            // Arrange
            var services = new ServiceCollection();
            var connectionString = _fixture.Create<string>();

            // Act
            var result = services.AddEntityFrameworkSqlServer(connectionString);

            // Assert
            using var serviceScope = result.BuildServiceProvider().CreateScope();
            serviceScope.ServiceProvider.GetService<IUnitOfWork>().Should().NotBeNull();
            serviceScope.ServiceProvider.GetService<ICompanyRepository>().Should().NotBeNull();
            serviceScope.ServiceProvider.GetService<ApplicationContext>().Should().NotBeNull();
        }
    }
}