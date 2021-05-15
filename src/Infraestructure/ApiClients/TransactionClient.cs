using Infraestructure.Interfaces;
using Infraestructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
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
                using var result = await GetRequestAsync(endpoint, cancellationToken);
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.Default,
                    NumberHandling = JsonNumberHandling.AllowReadingFromString
                };
                return await JsonSerializer.DeserializeAsync<IEnumerable<TransactionModel>>(result, options, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
