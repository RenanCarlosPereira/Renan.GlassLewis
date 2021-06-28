using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Renan.GlassLewis.Domain.Company;
using Renan.GlassLewis.Infrastructure.EntityTypeConfigurations;
using System;
using Xunit;

namespace Renan.GlassLewis.Infrastructure.Tests.EntityTypeConfigurations
{
    public class CompanyTypeConfigurationTests
    {
        private readonly Fixture _fixture;

        public CompanyTypeConfigurationTests()
        {
            _fixture = new Fixture();
        }

        private CompanyTypeConfiguration CreateCompanyTypeConfiguration()
        {
            return new CompanyTypeConfiguration();
        }

        [Fact(Skip = "I need to find a way to mock EntityTypeBuilder")]
        public void Configure_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var companyTypeConfiguration = CreateCompanyTypeConfiguration();
            var builder = _fixture.Create<EntityTypeBuilder<CompanyEntity>>();

            // Act
            Action a = () => companyTypeConfiguration.Configure(builder);

            // Assert
            a.Should().NotThrow();
        }
    }
}