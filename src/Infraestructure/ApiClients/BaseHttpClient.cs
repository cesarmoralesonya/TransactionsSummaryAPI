using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Infraestructure.ApiClients
{
    public class BaseHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public BaseHttpClient(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _configuration = config;

        }

        public async Task<T> GetRequestAsync<T>(string url, CancellationToken cancellationToken = default)
        {
            var clientName = _configuration.GetSection("ConfigApp").GetSection("QuietStoneClient").Value;
            using var client = _httpClientFactory.CreateClient(clientName);
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead))
            {
                if (!response.IsSuccessStatusCode)
                    throw new ArgumentException($"The path {url} gets the following status code: " + response.StatusCode);
                using var stream = await response.Content.ReadAsStreamAsync();
                
                if (!stream.CanRead)
                    throw new ArgumentException("It is not possible read the stream");
                var options = new JsonSerializerOptions 
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.Default,  
                    NumberHandling = JsonNumberHandling.AllowReadingFromString
                };
                return await JsonSerializer.DeserializeAsync<T>(stream, options, cancellationToken);
            }
        }
    }
}
