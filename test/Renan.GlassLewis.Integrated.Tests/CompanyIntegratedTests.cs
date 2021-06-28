using AutoFixture;
using FluentAssertions;
using Renan.GlassLewis.Application.CompaniesUseCases.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Renan.GlassLewis.Integrated.Tests
{
    public class TokenSourceModel
    {
        public string Token { get; set; }
    }

    public class AuthenticationHttpMessageHandler : DelegatingHandler
    {
        private readonly HttpClient _httpClient;

        public AuthenticationHttpMessageHandler()
        {
            InnerHandler = new HttpClientHandler();
            _httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:800") };
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var login = new { Username = "GlassLewis", Password = "123" };
            using var content = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync("api/Token/login", content, cancellationToken);

            await using var stream = await result.Content.ReadAsStreamAsync(cancellationToken);
            var auth = await JsonSerializer.DeserializeAsync<TokenSourceModel>(stream, cancellationToken: cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth?.Token);

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }

    public class CompanyIntegratedTests
    {
        private readonly HttpClient _httpClient;
        private readonly IFixture _fixture;

        public CompanyIntegratedTests()
        {
            _fixture = new Fixture();
            _httpClient = new HttpClient(new AuthenticationHttpMessageHandler())
            { BaseAddress = new Uri("http://localhost:800") };
        }

        [Fact]
        public async Task<(SelectCompanyModel, Uri)> CreateCompany()
        {
            var model = _fixture.Create<CreateCompanyModel>();

            var json = JsonSerializer.Serialize(model);

            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PostAsync("/Company", content);

            await using var stream = await httpResponse.Content.ReadAsStreamAsync();

            httpResponse.IsSuccessStatusCode.Should().BeTrue();

            var data = await JsonSerializer.DeserializeAsync<SelectCompanyModel>(stream);

            return (data, httpResponse.Headers.Location);
        }

        [Fact]
        public async Task GetCompanies()
        {
            var (model, uri) = await CreateCompany();

            var httpResponse = await _httpClient.GetAsync("/Company");

            await using var stream = await httpResponse.Content.ReadAsStreamAsync();

            httpResponse.IsSuccessStatusCode.Should().BeTrue();

            var data = await JsonSerializer.DeserializeAsync<List<SelectCompanyModel>>(stream);

            data.Should().HaveCountGreaterThan(0);

            data.Should().Contain(x => x.Isin == model.Isin);
        }

        [Fact]
        public async Task GetCompanyById()
        {
            var (model, uri) = await CreateCompany();

            var httpResponse = await _httpClient.GetAsync(uri);

            await using var stream = await httpResponse.Content.ReadAsStreamAsync();

            httpResponse.IsSuccessStatusCode.Should().BeTrue();

            var data = await JsonSerializer.DeserializeAsync<SelectCompanyModel>(stream);

            data?.Isin.Should().Be(model.Isin);
        }

        [Fact]
        public async Task UpdateCompany()
        {
            var (selectModel, uri) = await CreateCompany();

            var updateModel = _fixture.Create<UpdateCompanyModel>();

            var json = JsonSerializer.Serialize(updateModel);

            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            var httpResponse = await _httpClient.PutAsync(uri, content);

            httpResponse.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public async Task GetByIsinAsync()
        {
            var (model, uri) = await CreateCompany();

            var httpResponse = await _httpClient.GetAsync($"/Company/isin/{model.Isin}");

            await using var stream = await httpResponse.Content.ReadAsStreamAsync();

            httpResponse.IsSuccessStatusCode.Should().BeTrue();

            var data = await JsonSerializer.DeserializeAsync<SelectCompanyModel>(stream);

            data?.Isin.Should().Be(model.Isin);
        }

        
    }
}