using System;
using System.Net.Http;
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

        public async Task<string> GetRequest(string url)
        {
            HttpResponseMessage response;
            using (var client = _httpClientFactory.CreateClient("quiet-stone-2094"))
            {
                response = client.GetAsync(url).Result;

                if (!response.IsSuccessStatusCode)
                    throw new ArgumentException($"The path {url} gets the following status code: " + response.StatusCode);

            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}
