using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Renan.GlassLewis.Domain.Company;
using Renan.GlassLewis.Infrastructure.DbContexts;
using Renan.GlassLewis.Infrastructure.Repositories;
using System.Threading.Tasks;
using Xunit;

namespace Renan.GlassLewis.Infrastructure.Tests.Repositories
{
    public class UnitOfWorkTests
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IFixture _fixture;

        public UnitOfWorkTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>().UseInMemoryDatabase("Test").Options;
            _fixture = new Fixture();
            _applicationContext = new ApplicationContext(options);
        }

        private UnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(_applicationContext);
        }

        [Fact]
        public void Complete_WhenNoTransactionAvalible_ShouldReturnZero()
        {
            // Arrange
            var unitOfWork = CreateUnitOfWork();

            // Act
            var result = unitOfWork.Complete();

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public void Complete_WhenTransactionAvalible_ShouldReturnTheNumberOfEntitiesUpdated()
        {
            // Arrange
            var unitOfWork = CreateUnitOfWork();
            var company = _fixture.Create<CompanyEntity>();
            _applicationContext.Companies.Add(company);
            // Act
            var result = unitOfWork.Complete();

            // Assert
            result.Should().BeGreaterThan(0);
        }

        [Fact]
        public async ValueTask CompleteAsync_WhenNoTransactionAvalible_ShouldReturnZero()
        {
            // Arrange
            var unitOfWork = CreateUnitOfWork();

            // Act
            var result = await unitOfWork.CompleteAsync();

            // Assert
            result.Should().Be(0);
        }

        [Fact]
        public async ValueTask CompleteAsync_WhenTransactionAvalible_ShouldReturnTheNumberOfEntitiesUpdated()
        {
            // Arrange
            var unitOfWork = CreateUnitOfWork();
            var company = _fixture.Create<CompanyEntity>();
            await _applicationContext.Companies.AddAsync(company);
            // Act
            var result = await unitOfWork.CompleteAsync();

            // Assert
            result.Should().BeGreaterThan(0);
        }
    }
}