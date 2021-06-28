using Microsoft.AspNetCore.Mvc;
using Renan.GlassLewis.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Renan.GlassLewis.Mvc.Controllers
{
    public class CompanyController : Controller
    {
        private readonly HttpClient _httpClient;

        private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public CompanyController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Company");
        }

        // GET: CompanyController
        public async Task<ActionResult> Index(CancellationToken cancellationToken)
        {
            var httpResponse = await _httpClient.GetAsync("/Company", cancellationToken);

            await using var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);

            if (!httpResponse.IsSuccessStatusCode)
                return View(Array.Empty<SelectCompanyModel>());

            var data = await JsonSerializer.DeserializeAsync<List<SelectCompanyModel>>(stream, cancellationToken: cancellationToken);

            return View(data);
        }

        // GET: CompanyController/Details/5
        public async Task<ActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var httpResponse = await _httpClient.GetAsync($"/Company/{id}", cancellationToken);

            await using var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);

            if (!httpResponse.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            var data = await JsonSerializer.DeserializeAsync<SelectCompanyModel>(stream, cancellationToken: cancellationToken);

            return View(data);
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
                var json = JsonSerializer.Serialize(model);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                var httpResponse = await _httpClient.PostAsync("/Company/Create", content, cancellationToken);

                await using var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);

                if (httpResponse.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                var data = await JsonSerializer.DeserializeAsync<BadRequestResponseModel>(stream, JsonSerializerOptions, cancellationToken);

                data?.Errors.ToList().ForEach(x => ModelState.AddModelError(x.Key, string.Concat(x.Value)));

                return View(model);
            }
            catch
            {
                return View(model);
            }
        }

        // GET: CompanyController/Edit/5
        public async Task<ActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var httpResponse = await _httpClient.GetAsync($"/Company/{id}", cancellationToken);

            await using var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);

            if (!httpResponse.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            var data = await JsonSerializer.DeserializeAsync<SelectCompanyModel>(stream, cancellationToken: cancellationToken);

            var edit = new UpdateCompanyModel
            {
                Isin = data?.Isin,
                Exchange = data?.Exchange,
                Name = data?.Name,
                Ticker = data?.Ticker
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
                var json = JsonSerializer.Serialize(model);
                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                var httpResponse = await _httpClient.PostAsync($"/Company/Update/{id}", content, cancellationToken);

                await using var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken);

                if (httpResponse.IsSuccessStatusCode) return RedirectToAction(nameof(Index));

                var data = await JsonSerializer.DeserializeAsync<BadRequestResponseModel>(stream, JsonSerializerOptions, cancellationToken);

                data?.Errors.ToList().ForEach(x => ModelState.AddModelError(x.Key, string.Concat(x.Value)));

                return View(model);
            }
            catch
            {
                return View(model);
            }
        }
    }
}