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
using System.Threading;

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

        public async virtual Task<IEnumerable<TransactionModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await GetRequestAsync<IEnumerable<TransactionModel>>("transactions.json", cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
