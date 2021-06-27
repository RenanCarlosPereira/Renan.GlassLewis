using FluentValidation.Results;
using Renan.GlassLewis.Domain.Company;
using Renan.GlassLewis.Domain.Repositories;
using Renan.GlassLewis.Service.CompaniesUseCases.Models;
using System.Collections.Generic;
using System.Linq;
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

            var result = isin.Validate();

            if (!result.IsValid) return result;

            var company = new CompanyEntity(model.Name, model.Exchange, isin, model.WebSite);

            result = await _domainService.CreateCompanyAsync(company, cancellationToken);

            if (!result.IsValid) return result;

            await _unitOfWork.CompleteAsync(cancellationToken);

            return result;
        }

        public async ValueTask<ValidationResult> UpdateCompanyAsync(int id, UpdateCompanyModel model, CancellationToken cancellationToken = default)
        {
            var result = new ValidationResult();
            var company = await _domainService.GetByIdAsync(id, cancellationToken);
            if (company == null)
            {
                result.Errors.Add(new ValidationFailure(nameof(CompanyEntity.Id), CompanyConstants.CompanyIdMustExist));
                return result;
            }

            company.Name = model.Name;
            company.Isin = new CompanyIsin(model.Isin);
            company.WebSite = model.WebSite;
            company.Exchange = model.Exchange;

            result = company.Isin.Validate();

            if (!result.IsValid) return result;

            result = await _domainService.UpdateCompanyAsync(company, cancellationToken);

            if (result.IsValid)
                await _unitOfWork.CompleteAsync(cancellationToken);

            return result;
        }

        public async ValueTask<SelectCompanyModel> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _domainService.GetByIdAsync(id, cancellationToken);

            var companyModel = new SelectCompanyModel
            {
                Isin = result.Isin.Isin,
                Exchange = result.Exchange,
                Name = result.Name,
                WebSite = result.WebSite
            };

            return companyModel;
        }

        public async IAsyncEnumerable<SelectCompanyModel> GetAllCompaniesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var companies = _domainService.GetAllCompanies();
            await foreach (var company in companies.WithCancellation(cancellationToken))
            {
                yield return new SelectCompanyModel
                {
                    Isin = company.Isin.Isin,
                    Exchange = company.Exchange,
                    Name = company.Name,
                    WebSite = company.WebSite
                };
            }
        }

        public async ValueTask<SelectCompanyModel> GetByIsinAsync(string isin, CancellationToken cancellationToken)
        {
            var companyIsin = new CompanyIsin(isin);

            var result = await _domainService.GetByIsinAsync(companyIsin, cancellationToken);

            if (result == null) return null;

            var companyModel = new SelectCompanyModel
            {
                Isin = result.Isin.Isin,
                Exchange = result.Exchange,
                Name = result.Name,
                WebSite = result.WebSite
            };

            return companyModel;
        }
    }
}