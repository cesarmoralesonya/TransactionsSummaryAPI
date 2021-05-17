using Infraestructure.Interfaces;
using Infraestructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Infraestructure.ApiClients
{
    public class RateClient : ApiClient, IRateClient
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        public RateClient(IHttpClientFactory httpClientFactory,
                                    ILogger<RateClient> logger,
                                    IConfiguration config) : base(httpClientFactory, config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<IEnumerable<RateModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = _configuration.GetSection("ConfigApp").GetSection("ratesEndpoint").Value;
                var result = await GetRequestAsync(endpoint, cancellationToken);
                return JsonConvert.DeserializeObject<IEnumerable<RateModel>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
