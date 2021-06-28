using Microsoft.AspNetCore.Mvc;
using Renan.GlassLewis.Service.CompaniesUseCases;
using Renan.GlassLewis.Service.CompaniesUseCases.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Renan.GlassLewis.Mvc.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyUseCase _companyUseCase;

        public CompanyController(ICompanyUseCase companyUseCase)
        {
            _companyUseCase = companyUseCase;
        }

        // GET: CompanyController
        public async Task<ActionResult> Index(CancellationToken cancellationToken)
        {
            var result = await _companyUseCase.GetAllCompaniesAsync(cancellationToken).ToListAsync(cancellationToken);
            return View(result);
        }

        // GET: CompanyController/Details/5
        public async Task<ActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var company = await _companyUseCase.GetByIdAsync(id, cancellationToken);
            return View(company);
        }

        // GET: CompanyController/Create
        public ActionResult Create()
        {
            return View(new CreateCompanyModel());
        }

        // POST: CompanyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateCompanyModel model, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _companyUseCase.CreateCompanyAsync(model, cancellationToken);

                if (result.IsValid) return RedirectToAction(nameof(Index));

                result.Errors.ForEach(x => ModelState.AddModelError(x.PropertyName, x.ErrorMessage));

                return View(model);
            }
            catch
            {
                return View();
            }
        }

        // GET: CompanyController/Edit/5
        public async Task<ActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var company = await _companyUseCase.GetByIdAsync(id, cancellationToken);
            var edit = new UpdateCompanyModel
            {
                Isin = company.Isin,
                Exchange = company.Exchange,
                Name = company.Name,
                Ticker = company.Ticker
            };

            return View(edit);
        }

        // POST: CompanyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, UpdateCompanyModel model, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _companyUseCase.UpdateCompanyAsync(id, model, cancellationToken);

                if (result.IsValid) return RedirectToAction(nameof(Index));

                result.Errors.ForEach(x => ModelState.AddModelError(x.PropertyName, x.ErrorMessage));

                return View(model);
            }
            catch
            {
                return View();
            }
        }
    }
}