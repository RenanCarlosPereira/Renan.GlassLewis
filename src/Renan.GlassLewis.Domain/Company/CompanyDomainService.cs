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
            var companies = _companyRepository.FindAsync(x => x.Isin.Isin == isin.Isin);
            return companies.FirstOrDefaultAsync(cancellationToken);
        }

        public async ValueTask<ValidationResult> CreateCompanyAsync(CompanyEntity company, CancellationToken cancellationToken = default)
        {
            var result = new ValidationResult();
            var companies = _companyRepository.FindAsync(x => x.Isin.Isin == company.Isin.Isin);
            if (await companies.AnyAsync(cancellationToken))
            {
                result.Errors.Add(new ValidationFailure(nameof(CompanyIsin), CompanyConstants.CompanyWithSameIsinAlreadyExists));
                return result;
            }

            company = await _companyRepository.AddAsync(company, cancellationToken);
            return result;
        }

        public async ValueTask<ValidationResult> UpdateCompanyAsync(CompanyEntity company, CancellationToken cancellationToken = default)
        {
            var result = new ValidationResult();

            var companies = _companyRepository.FindAsync(x => x.Id == company.Id);

            if (!await companies.AnyAsync(cancellationToken))
            {
                result.Errors.Add(new ValidationFailure(nameof(CompanyIsin), CompanyConstants.CompanyIdMustExist));
                return result;
            }
            var exists = await _companyRepository.FindAsync(x => x.Id != company.Id && x.Isin.Isin == company.Isin.Isin).AnyAsync(cancellationToken);

            if (exists)
            {
                result.Errors.Add(new ValidationFailure(nameof(CompanyIsin), CompanyConstants.CompanyWithSameIsinAlreadyExists));
                return result;
            }

            await _companyRepository.UpdateAsync(company, cancellationToken);
            return result;
        }
    }
}