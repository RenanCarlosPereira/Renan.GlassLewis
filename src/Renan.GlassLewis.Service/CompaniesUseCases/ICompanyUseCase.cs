using System.Collections.Generic;
using FluentValidation.Results;
using System.Threading;
using System.Threading.Tasks;
using Renan.GlassLewis.Service.CompaniesUseCases.Models;

namespace Renan.GlassLewis.Service.CompaniesUseCases
{
    public interface ICompanyUseCase
    {
        ValueTask<SelectCompanyModel> GetByIsinAsync(string isin, CancellationToken cancellationToken);

        ValueTask<ValidationResult> CreateCompanyAsync(CreateCompanyModel model, CancellationToken cancellationToken = default);

        ValueTask<ValidationResult> UpdateCompanyAsync(int id, UpdateCompanyModel model, CancellationToken cancellationToken = default);

        ValueTask<SelectCompanyModel> GetByIdAsync(int id, CancellationToken cancellationToken);
        IAsyncEnumerable<SelectCompanyModel> GetAllCompaniesAsync(CancellationToken cancellationToken);
    }
}