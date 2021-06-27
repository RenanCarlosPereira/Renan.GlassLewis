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
            var service = CreateService();

            var companies = _fixture.CreateMany<CompanyEntity>().ToList();

            _companyRepository.FindAsync(Arg.Any<Expression<Func<CompanyEntity, bool>>>())
                .Returns(companies.ToAsyncEnumerable());

            var result = service.GetAllCompanies();

            var count = await result.CountAsync(CancellationToken.None);
            companies.Should().HaveCount(count);
        }

        [Fact]
        public async Task FindCompanyByIsinAsync_ReturnsData_WhenExists()
        {
            // Arrange
            var service = CreateService();

            var companies = _fixture.CreateMany<CompanyEntity>().ToList();

            _companyRepository.FindAsync(Arg.Any<Expression<Func<CompanyEntity, bool>>>())
                .Returns(companies.ToAsyncEnumerable());

            // Act
            var company = companies.FirstOrDefault();
            var result = await service.GetByIsinAsync(company?.Isin, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(company);
        }

        [Fact]
        public async Task FindCompanyByIsinAsync_ReturnsNull_WhenNotExists()
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
                .Returns(companies.ToAsyncEnumerable());

            // Act
            var company = companies.First();
            var result = await service.GetByIdAsync(company.Id, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(company);
        }

        [Fact]
        public async Task FindCompanyByIdAsync_ReturnsNull_WhenNotExists()
        {
            // Arrange
            var service = CreateService();

            var companies = _fixture.CreateMany<CompanyEntity>().ToList();

            _companyRepository.FindAsync(Arg.Any<Expression<Func<CompanyEntity, bool>>>())
                .Returns(Array.Empty<CompanyEntity>().ToAsyncEnumerable());

            // Act
            var isin = _fixture.Create<int>();
            var result = await service.GetByIdAsync(isin, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateCompany_ShouldNotAllowCreateCompany_WhenIsinAlreadyExists()
        {
            // Arrange
            var service = CreateService();

            var company = _fixture.Create<CompanyEntity>();

            _companyRepository.FindAsync(Arg.Any<Expression<Func<CompanyEntity, bool>>>())
                .Returns(new[] { company }.ToAsyncEnumerable());

            // Act
            var result = await service.CreateCompanyAsync(company, CancellationToken.None);

            // Assert
            result.Errors.Select(x => x.ErrorMessage).Should()
                .Contain(CompanyConstants.CompanyWithSameIsinAlreadyExists);

            await _companyRepository.DidNotReceive().AddAsync(Arg.Any<CompanyEntity>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CreateCompany_ShouldCreateCompany_WhenIsinDoesNotExists()
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
            await _companyRepository.Received(1).AddAsync(Arg.Any<CompanyEntity>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task UpdateCompany_ShouldUpdateCompany_WhenExists()
        {
            // Arrange
            var service = CreateService();

            var companies = _fixture.CreateMany<CompanyEntity>().ToList();
            _companyRepository.FindAsync(Arg.Any<Expression<Func<CompanyEntity, bool>>>())
                .Returns(companies.ToAsyncEnumerable());

            var company = companies.First();

            // Act
            var result = await service.UpdateCompanyAsync(company, CancellationToken.None);

            // Assert
            result.IsValid.Should().BeTrue();
            await _companyRepository.Received(1).UpdateAsync(Arg.Any<CompanyEntity>(), Arg.Any<CancellationToken>());
        }


        [Fact]
        public async Task UpdateCompany_ShouldNotUpdateIfTheresADuplicatedIsin()
        {
            // Arrange
            var service = CreateService();

            var companies = _fixture.CreateMany<CompanyEntity>().ToList();
            _companyRepository.FindAsync(Arg.Any<Expression<Func<CompanyEntity, bool>>>())
                .Returns(companies.ToAsyncEnumerable());

            var company = _fixture.Build<CompanyEntity>()
                .With(x => x.Id, companies.First().Id)
                .With(x => x.Exchange)
                .With(x => x.Isin)
                .With(x => x.WebSite).Create();

            // Act
            var result = await service.UpdateCompanyAsync(company, CancellationToken.None);

            // Assert
            result.IsValid.Should().BeTrue();
            await _companyRepository.Received(1).UpdateAsync(Arg.Any<CompanyEntity>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task UpdateCompany_ShouldNotUpdateIfTheresADuplicatedIsinn()
        {
            // Arrange
            var service = CreateService();

            var companies = _fixture.CreateMany<CompanyEntity>().ToList();
            _companyRepository.FindAsync(Arg.Any<Expression<Func<CompanyEntity, bool>>>())
                .Returns(companies.ToAsyncEnumerable());

            var company = _fixture.Build<CompanyEntity>()
                .With(x => x.Id, 10)
                .With(x => x.Exchange)
                .With(x => companies.First().Isin)
                .With(x => x.WebSite).Create();

            // Act
            var result = await service.UpdateCompanyAsync(company, CancellationToken.None);

            // Assert
            result.IsValid.Should().BeTrue();
            await _companyRepository.Received(1).UpdateAsync(Arg.Any<CompanyEntity>(), Arg.Any<CancellationToken>());
        }
    }
}