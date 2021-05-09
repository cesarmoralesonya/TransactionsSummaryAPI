using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
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
            var client = _httpClientFactory.CreateClient("quiet-stone-2094");

            var response = client.GetAsync(url).Result;

            if(!response.IsSuccessStatusCode)
                throw new ArgumentException($"The path {url} gets the following status code: " + response.StatusCode);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
