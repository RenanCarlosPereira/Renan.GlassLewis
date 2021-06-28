using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Renan.GlassLewis.Service.CompaniesUseCases;
using Renan.GlassLewis.Service.CompaniesUseCases.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Renan.GlassLewis.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyUseCase _companyUseCase;
        private readonly IOptions<ApiBehaviorOptions> _options;

        public CompanyController(ICompanyUseCase companyUseCase, IOptions<ApiBehaviorOptions> options)
        {
            _companyUseCase = companyUseCase;
            _options = options;
        }

        [HttpGet]
        [ActionName(nameof(GetAllCompaniesAsync))]
        public IAsyncEnumerable<SelectCompanyModel> GetAllCompaniesAsync(CancellationToken cancellationToken)
        {
            return _companyUseCase.GetAllCompaniesAsync(cancellationToken);
        }

        [HttpGet("Value/{isin}")]
        [ActionName(nameof(GetByIsinAsync))]
        public async ValueTask<IActionResult> GetByIsinAsync([FromRoute] string isin, CancellationToken cancellationToken)
        {
            var result = await _companyUseCase.GetByIsinAsync(isin, cancellationToken);
            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetByIdAsync))]
        public async ValueTask<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _companyUseCase.GetByIdAsync(id, cancellationToken);
            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpPost("Create")]
        [ActionName(nameof(CreateCompanyAsync))]
        public async ValueTask<IActionResult> CreateCompanyAsync([FromBody] CreateCompanyModel company, CancellationToken cancellationToken)
        {
            var result = await _companyUseCase.CreateCompanyAsync(company, cancellationToken);

            if (result.IsValid)
                return CreatedAtAction(nameof(GetByIsinAsync), new { isin = company.Isin }, default);

            result.Errors.ForEach(x => ModelState.AddModelError(x.PropertyName, x.ErrorMessage));

            return _options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }

        [HttpPost("Update/{id}")]
        [ActionName(nameof(UpdateCompanyAsync))]
        public async ValueTask<IActionResult> UpdateCompanyAsync([FromRoute] int id, [FromBody] UpdateCompanyModel company, CancellationToken cancellationToken)
        {
            var result = await _companyUseCase.UpdateCompanyAsync(id, company, cancellationToken);

            if (result.IsValid)
                return CreatedAtAction(nameof(GetByIsinAsync), new { isin = company.Isin }, default);

            result.Errors.ForEach(x => ModelState.AddModelError(x.PropertyName, x.ErrorMessage));

            return _options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}