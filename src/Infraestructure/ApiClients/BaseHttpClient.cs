using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infraestructure.ApiClients
{
    public class BaseHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;        

        public BaseHttpClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

        }

        public async Task<T> GetRequest<T>(string url)
        {
            using var client = _httpClientFactory.CreateClient("quiet-stone-2094");
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead))
            {
                if (!response.IsSuccessStatusCode)
                    throw new ArgumentException($"The path {url} gets the following status code: " + response.StatusCode);
                var stream = await response.Content.ReadAsStreamAsync();
                
                if (!stream.CanRead)
                    throw new ArgumentException("It is not possible read the stream");
                var options = new JsonSerializerOptions 
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.Default,  
                    NumberHandling = JsonNumberHandling.AllowReadingFromString
                };
                return await JsonSerializer.DeserializeAsync<T>(stream, options);
            }
        }
    }
}
