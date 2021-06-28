using Microsoft.Extensions.Options;
using Renan.GlassLewis.Mvc.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Renan.GlassLewis.Mvc
{
    public class AuthenticationHttpMessageHandler : DelegatingHandler
    {
        private readonly ApiOption _options;
        private readonly HttpClient _httpClient;

        public AuthenticationHttpMessageHandler(IHttpClientFactory clientFactory, IOptions<ApiOption> options)
        {
            _options = options.Value;
            _httpClient = clientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(options.Value.Url);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var login = new { _options.Username, _options.Password };
            using var content = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json");
            var result = await _httpClient.PostAsync("api/Token/login", content, cancellationToken);

            await using var stream = await result.Content.ReadAsStreamAsync(cancellationToken);
            var auth = await JsonSerializer.DeserializeAsync<TokenSourceModel>(stream, cancellationToken: cancellationToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", auth?.Token);

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }

    public class ApiOption
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string Url { get; set; }
    }
}