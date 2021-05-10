using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infraestructure.ApiClients
{
    public class TransactionClient : BaseHttpClient, ITransactionClient<TransactionModel>
    {
        public TransactionClient(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public async virtual Task<IEnumerable<TransactionModel>> GetAll()
        {
            try
            {
                var result = await GetRequest("rates.json");
                return JsonConvert.DeserializeObject<IEnumerable<TransactionModel>>(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
