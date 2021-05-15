using Infraestructure.Interfaces;
using Infraestructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
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
            _configuration = config;
        }

        public async virtual Task<IEnumerable<TransactionModel>> GetAllAsync(CancellationToken cancellationToken = default)
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
    }
}
