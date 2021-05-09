﻿using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Infraestructure.ApiClients
{
    public class TransactionClient: BaseHttpClient, ITransactionClient<Transaction>
    {
        public TransactionClient(IHttpClientFactory httpClientFactory): base(httpClientFactory)
        { 
        }

        public async Task<IEnumerable<Transaction>> GetAll()
        {
            try
            {
                var result = await GetRequest("rates.json");
                return JsonConvert.DeserializeObject<IEnumerable<Transaction>>(result);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
