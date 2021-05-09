using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infraestructure.ApiClients
{
    public class ConversionClient : IConversionClient<Conversion>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ConversionClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<Conversion>> GetAll()
        {
            var client = _httpClientFactory.CreateClient("quiet-stone-2094");

            var response = client.GetAsync("rates.json").Result;

            if(response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<Conversion>>(result);
            }
            else
            {
                //Error de Get
                return null;
            }

        }
    }
}
