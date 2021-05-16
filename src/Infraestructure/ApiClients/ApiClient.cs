using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Infraestructure.ApiClients
{
    public class ApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ApiClient(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _configuration = config;

        }

        public async Task<string> GetRequestAsync(string url, CancellationToken cancellationToken = default)
        {
            var clientName = _configuration.GetSection("ConfigApp").GetSection("QuietStoneClient").Value;
            var client = _httpClientFactory.CreateClient(clientName);
            var response = client.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode)
                throw new ArgumentException($"The path {url} gets the following status code: " + response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content))
                throw new ArgumentException("Content null or empy");
            return content;
        }
    }
}
