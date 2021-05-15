using Microsoft.Extensions.Configuration;
using System;
using System.IO;
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

        public virtual async Task<Stream> GetRequestAsync(string url, CancellationToken cancellationToken = default)
        {
            var clientName = _configuration.GetSection("ConfigApp").GetSection("QuietStoneClient").Value;
            using var client = _httpClientFactory.CreateClient(clientName);
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead);
            if (!response.IsSuccessStatusCode)
                throw new ArgumentException($"The path {url} gets the following status code: " + response.StatusCode);
            
            var stream = await response.Content.ReadAsStreamAsync();
            if (!stream.CanRead)
                throw new ArgumentException("It is not possible read the stream");
            return stream;
        }
    }
}
