using FluentValidation.Results;
using Renan.GlassLewis.Domain.Company;
using Renan.GlassLewis.Domain.Repositories;
using Renan.GlassLewis.Service.CompaniesUseCases.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Renan.GlassLewis.Service.CompaniesUseCases
{
    internal class CompanyUseCase : ICompanyUseCase
    {
        private readonly ICompanyDomainService _domainService;
        private readonly IUnitOfWork _unitOfWork;

        public CompanyUseCase(ICompanyDomainService domainService, IUnitOfWork unitOfWork)
        {
            _domainService = domainService;
            _unitOfWork = unitOfWork;
        }

        public async ValueTask<ValidationResult> CreateCompanyAsync(CreateCompanyModel model, CancellationToken cancellationToken = default)
        {
            var isin = new CompanyIsin(model.Isin);

            var company = new CompanyEntity(model.Name, model.Exchange, isin, model.WebSite, model.Ticker);

            var result = company.Validate();

            if (!result.IsValid) return result;

            result = await _domainService.CreateCompanyAsync(company, cancellationToken);

            if (!result.IsValid) return result;

            await _unitOfWork.CompleteAsync(cancellationToken);

            return result;
        }

        public async ValueTask<ValidationResult> UpdateCompanyAsync(int id, UpdateCompanyModel model, CancellationToken cancellationToken = default)
        {
            var company = new CompanyEntity(model.Name, model.Exchange, new CompanyIsin(model.Isin), model.WebSite, model.Ticker);

            var result = company.Validate();

            if (!result.IsValid) return result;

            result = await _domainService.UpdateCompanyAsync(id, company, cancellationToken);

            if (result.IsValid)
                await _unitOfWork.CompleteAsync(cancellationToken);

            return result;
        }

        public async ValueTask<SelectCompanyModel> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var company = await _domainService.GetByIdAsync(id, cancellationToken);

            var companyModel = Map(company);

            return companyModel;
        }

        public async IAsyncEnumerable<SelectCompanyModel> GetAllCompaniesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var companies = _domainService.GetAllCompanies();
            await foreach (var company in companies.WithCancellation(cancellationToken))
            {
                yield return Map(company);
            }
        }

        public async ValueTask<SelectCompanyModel> GetByIsinAsync(string isin, CancellationToken cancellationToken)
        {
            var companyIsin = new CompanyIsin(isin);

            var company = await _domainService.GetByIsinAsync(companyIsin, cancellationToken);

            if (company == null) return null;

            var companyModel = Map(company);

            return companyModel;
        }

        private static SelectCompanyModel Map(CompanyEntity company)
        {
            return new SelectCompanyModel
            {
                Id = company.Id,
                Isin = company.Isin.Value,
                Exchange = company.Exchange,
                Name = company.Name,
                WebSite = company.WebSite,
                Ticker = company.Ticker
            };
        }
    }
}