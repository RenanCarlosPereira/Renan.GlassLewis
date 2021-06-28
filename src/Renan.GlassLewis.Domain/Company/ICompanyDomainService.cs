using FluentValidation.Results;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Renan.GlassLewis.Domain.Company
{
    public interface ICompanyDomainService
    {
        public IAsyncEnumerable<CompanyEntity> GetAllCompanies();

        public ValueTask<CompanyEntity> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        public ValueTask<CompanyEntity> GetByIsinAsync(CompanyIsin isin, CancellationToken cancellationToken = default);

        public ValueTask<ValidationResult> CreateCompanyAsync(CompanyEntity company, CancellationToken cancellationToken = default);

        public ValueTask<ValidationResult> UpdateCompanyAsync(int id, CompanyEntity company, CancellationToken cancellationToken = default);
    }
}