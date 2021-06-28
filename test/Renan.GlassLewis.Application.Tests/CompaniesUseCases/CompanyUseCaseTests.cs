using System.ComponentModel.DataAnnotations;
using AutoFixture;
using NSubstitute;
using Renan.GlassLewis.Application.CompaniesUseCases;
using Renan.GlassLewis.Application.CompaniesUseCases.Models;
using Renan.GlassLewis.Domain.Company;
using Renan.GlassLewis.Domain.Repositories;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using ValidationResult = FluentValidation.Results;

namespace Renan.GlassLewis.Application.Tests.CompaniesUseCases
{
    public class CompanyUseCaseTests
    {
        private readonly ICompanyDomainService _companyDomainService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFixture _fixture;

        public CompanyUseCaseTests()
        {
            _companyDomainService = Substitute.For<ICompanyDomainService>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _fixture = new Fixture();
        }

        private CompanyUseCase CreateCompanyUseCase()
        {
            return new CompanyUseCase(_companyDomainService, _unitOfWork);
        }

        [Fact]
        public async Task CreateCompanyAsync_WhenNoValidationErrors_CompleteTransaction()
        {
            // Arrange
            var companyUseCase = this.CreateCompanyUseCase();
            var model = _fixture.Create<CreateCompanyModel>();
            var cancellationToken = CancellationToken.None;
            _companyDomainService.CreateCompanyAsync(Arg.Any<CompanyEntity>(), Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(new ValidationResult.ValidationResult());

            // Act
            var result = await companyUseCase.CreateCompanyAsync(model, cancellationToken);

            // Assert
            result.IsValid.Should().BeTrue();
            await _unitOfWork.ReceivedWithAnyArgs().CompleteAsync(cancellationToken);
        }

        [Fact]
        public async Task UpdateCompanyAsync_WhenNoValidationErrors_CompleteTransaction()
        {
            // Arrange
            var companyUseCase = this.CreateCompanyUseCase();
            var model = _fixture.Create<UpdateCompanyModel>();
            var cancellationToken = CancellationToken.None;
            _companyDomainService.UpdateCompanyAsync(Arg.Any<int>(), Arg.Any<CompanyEntity>(), Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(new ValidationResult.ValidationResult());

            // Act
            var id = _fixture.Create<int>();
            var result = await companyUseCase.UpdateCompanyAsync(id, model, cancellationToken);

            // Assert
            result.IsValid.Should().BeTrue();
            await _unitOfWork.ReceivedWithAnyArgs().CompleteAsync(cancellationToken);
        }

        [Fact]
        public async Task GetByIdAsync_Call_ReturnValueFromDomain()
        {
            // Arrange
            var companyUseCase = this.CreateCompanyUseCase();
            int id = _fixture.Create<int>();
            var cancellationToken = CancellationToken.None;
            var companyEntity = _fixture.Create<CompanyEntity>();
            _companyDomainService.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
                .Returns(companyEntity);

            // Act
            var result = await companyUseCase.GetByIdAsync(id, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetAllCompaniesAsync_Call_ReturnSameCountAsDomainService()
        {
            // Arrange
            var companyUseCase = CreateCompanyUseCase();
            CancellationToken cancellationToken = CancellationToken.None;
            var companies = _fixture.CreateMany<CompanyEntity>().ToList();
            _companyDomainService.GetAllCompanies().Returns(companies.ToAsyncEnumerable());

            // Act
            var result = companyUseCase.GetAllCompaniesAsync(cancellationToken);

            // Assert
            var list = await result.ToListAsync(cancellationToken);
            list.Should().HaveSameCount(companies);
        }

        [Fact]
        public async Task GetByIsinAsync_GetCompany_HaveTheSameId()
        {
            // Arrange
            var companyUseCase = this.CreateCompanyUseCase();
            var isin = _fixture.Create<string>();
            var cancellationToken = CancellationToken.None;
            var companyEntity = _fixture.Create<CompanyEntity>();

            _companyDomainService.GetByIsinAsync(Arg.Any<CompanyIsin>(), Arg.Any<CancellationToken>())
                .Returns(companyEntity);

            // Act
            var result = await companyUseCase.GetByIsinAsync(isin, cancellationToken);

            // Assert
            companyEntity.Id.Should().Be(result.Id);
        }
    }
}