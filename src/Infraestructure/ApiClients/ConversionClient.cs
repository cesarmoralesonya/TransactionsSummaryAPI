using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infraestructure.ApiClients
{
    public class ConversionClient : BaseHttpClient, IConversionClient<IWebServicesEntity>
    {
        public ConversionClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public async virtual Task<IEnumerable<IWebServicesEntity>> GetAll()
        {
            try
            {
                var result = await GetRequest("rates.json");
                return JsonConvert.DeserializeObject<IEnumerable<Conversion>>(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
