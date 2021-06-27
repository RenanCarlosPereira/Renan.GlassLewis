using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Renan.GlassLewis.Domain.Company;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Renan.GlassLewis.Domain.Tests.Company
{
    public class CompanyDomainServiceTests
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IFixture _fixture;

        public CompanyDomainServiceTests()
        {
            _fixture = new Fixture();
            _companyRepository = Substitute.For<ICompanyRepository>();
        }

        private CompanyDomainService CreateService()
        {
            return new CompanyDomainService(_companyRepository);
        }

        [Fact]
        public async Task GetAllCompanies_ShouldReciveAllCompaniesFromRepository()
        {
            // Arrange
            var service = CreateService();
            var companies = _fixture.CreateMany<CompanyEntity>().ToList();
            _companyRepository.GetAllAsync().Returns(companies.ToAsyncEnumerable());

            //Act
            var result = await service.GetAllCompanies().ToListAsync(CancellationToken.None);

            //Assert
            result.Should().BeEquivalentTo(companies);
        }

        [Fact]
        public async Task FindCompanyByIsinAsync_WhenExists_ReturnsData()
        {
            // Arrange
            var service = CreateService();
            var companies = _fixture.CreateMany<CompanyEntity>().ToList();
            _companyRepository.FindAsync(Arg.Any<Expression<Func<CompanyEntity, bool>>>())
                .Returns(x => companies.Where(x.Arg<Expression<Func<CompanyEntity, bool>>>().Compile()).ToAsyncEnumerable());
            var company = companies.First();

            // Act
            var result = await service.GetByIsinAsync(company?.Isin, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(company);
        }

        [Fact]
        public async Task FindCompanyByIsinAsync_WhenNotExists_ReturnNull()
        {
            // Arrange
            var service = CreateService();

            var companies = _fixture.CreateMany<CompanyEntity>().ToList();

            _companyRepository.FindAsync(Arg.Any<Expression<Func<CompanyEntity, bool>>>())
                .Returns(Array.Empty<CompanyEntity>().ToAsyncEnumerable());

            // Act
            var isin = _fixture.Create<CompanyIsin>();
            var result = await service.GetByIsinAsync(isin, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task FindCompanyByIdAsync_ReturnsData_WhenExists()
        {
            // Arrange
            var service = CreateService();

            var companies = _fixture.CreateMany<CompanyEntity>().ToList();

            _companyRepository.FindAsync(Arg.Any<Expression<Func<CompanyEntity, bool>>>())
                .Returns(x => companies.Where(x.Arg<Expression<Func<CompanyEntity, bool>>>().Compile()).ToAsyncEnumerable());

            // Act
            var company = companies.First();
            var result = await service.GetByIdAsync(company.Id, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(company);
        }

        [Fact]
        public async Task FindCompanyByIdAsync_WhenNotExists_ReturnsNull()
        {
            // Arrange
            var service = CreateService();

            _companyRepository.FindAsync(Arg.Any<Expression<Func<CompanyEntity, bool>>>())
                .Returns(Array.Empty<CompanyEntity>().ToAsyncEnumerable());

            // Act
            var isin = _fixture.Create<int>();
            var result = await service.GetByIdAsync(isin, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateCompany_WhenIsinAlreadyExists_NotAllowCreateCompany()
        {
            // Arrange
            var service = CreateService();

            var companies = _fixture.CreateMany<CompanyEntity>().ToList();
            var company = _fixture.Create<CompanyEntity>();
            company.Isin = companies.First().Isin;

            _companyRepository.FindAsync(Arg.Any<Expression<Func<CompanyEntity, bool>>>())
                .Returns(x => companies.Where(x.Arg<Expression<Func<CompanyEntity, bool>>>().Compile()).ToAsyncEnumerable());

            // Act
            var result = await service.CreateCompanyAsync(company, CancellationToken.None);

            // Assert
            result.Errors.Select(x => x.ErrorMessage).Should()
                .Contain(CompanyConstants.CompanyWithSameIsinAlreadyExists);

            await _companyRepository.DidNotReceiveWithAnyArgs().AddAsync(Arg.Any<CompanyEntity>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CreateCompany_WhenIsinDoesNotExists_CreateCompany()
        {
            // Arrange
            var service = CreateService();

            var company = _fixture.Create<CompanyEntity>();
            _companyRepository.FindAsync(Arg.Any<Expression<Func<CompanyEntity, bool>>>())
                .Returns(Array.Empty<CompanyEntity>().ToAsyncEnumerable());

            // Act
            var result = await service.CreateCompanyAsync(company, CancellationToken.None);

            // Assert
            result.IsValid.Should().BeTrue();
            await _companyRepository.ReceivedWithAnyArgs(1).AddAsync(Arg.Any<CompanyEntity>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task UpdateCompany_WhenExists_UpdateCompany()
        {
            // Arrange
            var service = CreateService();

            var companies = _fixture.CreateMany<CompanyEntity>().ToList();
            _companyRepository.FindAsync(Arg.Any<Expression<Func<CompanyEntity, bool>>>())
                .Returns(x => companies.Where(x.Arg<Expression<Func<CompanyEntity, bool>>>().Compile()).ToAsyncEnumerable());

            var company = companies.First();

            // Act
            var result = await service.UpdateCompanyAsync(company, CancellationToken.None);

            // Assert
            result.IsValid.Should().BeTrue();
            await _companyRepository.ReceivedWithAnyArgs(1).UpdateAsync(Arg.Any<CompanyEntity>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task UpdateCompany_WhenDuplicateIsin_NotUpdate()
        {
            // Arrange
            var service = CreateService();

            var companies = _fixture.CreateMany<CompanyEntity>().ToList();

            var company = _fixture.Build<CompanyEntity>()
                .With(x => x.Id, 10)
                .With(x => x.Exchange)
                .With(x => x.Isin, companies.First().Isin)
                .With(x => x.WebSite).Create();

            companies.Add(company);

            _companyRepository.FindAsync(Arg.Any<Expression<Func<CompanyEntity, bool>>>())
                .Returns(x => companies.Where(x.Arg<Expression<Func<CompanyEntity, bool>>>().Compile()).ToAsyncEnumerable());

            // Act
            var result = await service.UpdateCompanyAsync(company, CancellationToken.None);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Select(x => x.ErrorMessage).Should().Contain(CompanyConstants.CompanyWithSameIsinAlreadyExists);
            await _companyRepository.DidNotReceiveWithAnyArgs().UpdateAsync(Arg.Any<CompanyEntity>(), Arg.Any<CancellationToken>());
        }
    }
}