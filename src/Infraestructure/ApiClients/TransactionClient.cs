using Infraestructure.Interfaces;
using Infraestructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Infraestructure.ApiClients
{
    public class TransactionClient : ApiClient, ITransactionClient
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        public TransactionClient(IHttpClientFactory httpClientFactory,
                                    ILogger<TransactionClient> logger,
                                    IConfiguration config) : base(httpClientFactory, config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<IEnumerable<TransactionModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = _configuration.GetSection("ConfigApp").GetSection("TransactionsEndpoint").Value;
                var result = await GetRequestAsync(endpoint, cancellationToken);
                return JsonConvert.DeserializeObject<IEnumerable<TransactionModel>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<TransactionModel>> GetListAsync(string sku, CancellationToken cancellationToken)
        {
            try
            {
                var endpoint = _configuration.GetSection("ConfigApp").GetSection("TransactionsEndpoint").Value;
                var result = await GetRequestAsync(endpoint, cancellationToken);
                var transactions = JsonConvert.DeserializeObject<IEnumerable<TransactionModel>>(result);
                return transactions.Where(trans => trans.Sku == sku).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
