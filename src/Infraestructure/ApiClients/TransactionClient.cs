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
using Microsoft.Extensions.Configuration;

namespace Infraestructure.ApiClients
{
    public class TransactionClient : BaseHttpClient, ITransactionClient<TransactionModel>
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
                return await GetRequestAsync<IEnumerable<TransactionModel>>(endpoint, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
