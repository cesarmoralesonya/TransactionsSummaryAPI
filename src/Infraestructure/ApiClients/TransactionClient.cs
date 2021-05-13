using Infraestructure.Interfaces;
using Infraestructure.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Infraestructure.ApiClients
{
    public class TransactionClient : BaseHttpClient, ITransactionClient<TransactionModel>
    {
        private readonly ILogger _logger;
        public TransactionClient(IHttpClientFactory httpClientFactory, 
                                    ILogger<TransactionClient> logger) : base(httpClientFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async virtual Task<IEnumerable<TransactionModel>> GetAll()
        {
            try
            {
                return await GetRequest<IEnumerable<TransactionModel>>("transactions.json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
