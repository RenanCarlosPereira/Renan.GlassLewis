using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Renan.GlassLewis.Domain.Company;
using Renan.GlassLewis.Infrastructure.DbContexts;
using Renan.GlassLewis.Infrastructure.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Renan.GlassLewis.Infrastructure.Tests.Repositories
{
    public class CompanyRepositoryTests
    {
        private readonly ApplicationContext _applicationContext;
        private readonly IFixture _fixture;

        public CompanyRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>().UseInMemoryDatabase("Test").Options;
            _fixture = new Fixture();
            _applicationContext = new ApplicationContext(options);
        }

        private CompanyRepository CreateCompanyRepository()
        {
            return new CompanyRepository(_applicationContext);
        }

        [Fact]
        public async ValueTask GetAllAsync_WhenCalled_ShouldReturnAllDataFromDataBase()
        {
            // Arrange
            var companyRepository = CreateCompanyRepository();
            var companies = _fixture.CreateMany<CompanyEntity>().ToList();
            await companyRepository.AddRangeAsync(companies, CancellationToken.None);
            await _applicationContext.SaveChangesAsync();

            // Act
            var result = companyRepository.GetAllAsync();

            // Assert
            var count = await result.CountAsync();
            count.Should().Be(companies.Count);
        }

        [Fact]
        public async ValueTask FindAsync_WhenCalled_ShouldBringFilteredData()
        {
            // Arrange
            var companyRepository = CreateCompanyRepository();
            var companies = _fixture.CreateMany<CompanyEntity>().ToList();
            await companyRepository.AddRangeAsync(companies, CancellationToken.None);
            await _applicationContext.SaveChangesAsync();

            // Act
            var company = companies.First();
            var result = companyRepository.FindAsync(x => x.Isin.Isin == company.Isin.Isin);

            // Assert
            var count = await result.CountAsync();
            count.Should().Be(1);
        }

        [Fact]
        public async ValueTask AddRangeAsync_WhenCalled_ShouldAddAllRange()
        {
            // Arrange
            var companyRepository = CreateCompanyRepository();
            var companies = _fixture.CreateMany<CompanyEntity>().ToList();

            // Act
            await companyRepository.AddRangeAsync(companies, CancellationToken.None);
            await _applicationContext.SaveChangesAsync();

            // Assert
            var count = companyRepository.GetAllAsync().CountAsync();
            count.Should().Be(companies.Count);
        }

        [Fact]
        public async ValueTask AddAsync_WhenCalled_ShouldAddOneCompany()
        {
            // Arrange
            var companyRepository = CreateCompanyRepository();
            var company = _fixture.Create<CompanyEntity>();

            // Act
            await companyRepository.AddAsync(company, CancellationToken.None);
            await _applicationContext.SaveChangesAsync();

            // Assert
            var count = companyRepository.GetAllAsync().CountAsync();
            count.Should().Be(1);
        }

        [Fact]
        public async ValueTask UpdateAsync_WhenCalled_ShouldUpdateOneCompany()
        {
            // Arrange
            var companyRepository = CreateCompanyRepository();
            var company = _fixture.Create<CompanyEntity>();
            await companyRepository.AddAsync(company);
            await _applicationContext.SaveChangesAsync();

            // Act
            var updatedName = _fixture.Create<string>();
            company.Name = updatedName;
            await companyRepository.UpdateAsync(company, CancellationToken.None);
            await _applicationContext.SaveChangesAsync();

            // Assert
            var databaSeCompany = await companyRepository.GetByIdAsync(company.Id);
            databaSeCompany.Name.Should().Be(updatedName);
        }

        [Fact]
        public async ValueTask RemoveAsync_WhenCalled_ShouldAddOneCompany()
        {
            // Arrange
            var companyRepository = CreateCompanyRepository();
            var companies = _fixture.CreateMany<CompanyEntity>().ToList();
            await companyRepository.AddRangeAsync(companies);
            await _applicationContext.SaveChangesAsync();

            // Act
            var company = companies.First();
            await companyRepository.RemoveAsync(company, CancellationToken.None);
            await _applicationContext.SaveChangesAsync();

            // Assert
            var count = await companyRepository.GetAllAsync().CountAsync();
            count.Should().Be(companies.Count - 1);
        }

        [Fact]
        public async ValueTask RemoveRangeAsync_WhenCalled_ShouldRemoveAllInTheRange()
        {
            // Arrange
            var companyRepository = CreateCompanyRepository();
            var companies = _fixture.CreateMany<CompanyEntity>().ToList();
            await companyRepository.AddRangeAsync(companies);
            await _applicationContext.SaveChangesAsync();

            // Act
            await companyRepository.RemoveRangeAsync(companies, CancellationToken.None);
            await _applicationContext.SaveChangesAsync();

            // Assert
            var count = await companyRepository.GetAllAsync().CountAsync();
            count.Should().Be(0);
        }
    }
}