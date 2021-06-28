using FluentValidation.Results;
using Renan.GlassLewis.Domain.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Renan.GlassLewis.Domain.Company
{
    internal class CompanyDomainService : ICompanyDomainService, IDomainService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyDomainService(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public IAsyncEnumerable<CompanyEntity> GetAllCompanies()
        {
            return _companyRepository.GetAllAsync();
        }

        public ValueTask<CompanyEntity> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var companies = _companyRepository.FindAsync(x => x.Id == id);
            return companies.FirstOrDefaultAsync(cancellationToken);
        }

        public ValueTask<CompanyEntity> GetByIsinAsync(CompanyIsin isin, CancellationToken cancellationToken = default)
        {
            var companies = _companyRepository.FindAsync(x => x.Isin.Value == isin.Value);
            return companies.FirstOrDefaultAsync(cancellationToken);
        }

        public async ValueTask<ValidationResult> CreateCompanyAsync(CompanyEntity company, CancellationToken cancellationToken = default)
        {
            var result = new ValidationResult();

            var companies = _companyRepository.FindAsync(x => x.Isin.Value == company.Isin.Value);
            if (await companies.AnyAsync(cancellationToken))
                result.Errors.Add(new ValidationFailure(nameof(CompanyEntity.Isin), CompanyConstants.CompanyWithSameIsinAlreadyExists));

            if (result.IsValid)
                company = await _companyRepository.AddAsync(company, cancellationToken);

            return result;
        }

        public async ValueTask<ValidationResult> UpdateCompanyAsync(int id, CompanyEntity company, CancellationToken cancellationToken = default)
        {
            var result = new ValidationResult();

            var companies = await _companyRepository.FindAsync(x => x.Id == id).ToListAsync(cancellationToken);

            if (!companies.Any())
            {
                result.Errors.Add(new ValidationFailure(nameof(CompanyIsin), CompanyConstants.CompanyIdMustExist));
                return result;
            }

            var exists = await _companyRepository.FindAsync(x => x.Id != id && x.Isin.Value == company.Isin.Value).AnyAsync(cancellationToken);

            if (exists)
            {
                result.Errors.Add(new ValidationFailure(nameof(CompanyEntity.Isin), CompanyConstants.CompanyWithSameIsinAlreadyExists));
                return result;
            }

            var companyDatabase = companies.First();
            companyDatabase.Name = company.Name;
            companyDatabase.Isin = company.Isin;
            companyDatabase.Exchange = company.Exchange;
            companyDatabase.WebSite = company.WebSite;
            companyDatabase.Ticker = company.Ticker;

            await _companyRepository.UpdateAsync(companyDatabase, cancellationToken);
            return result;
        }
    }
}