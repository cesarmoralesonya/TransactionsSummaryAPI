using Infraestructure.Interfaces;
using Infraestructure.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Infraestructure.ApiClients
{
    public class ConversionClient : BaseHttpClient, IConversionClient<ConversionModel>
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        public ConversionClient(IHttpClientFactory httpClientFactory,
                                    ILogger<ConversionClient> logger,
                                    IConfiguration config) : base(httpClientFactory, config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = config;
        }

        public async virtual Task<IEnumerable<ConversionModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var endpoint = _configuration.GetSection("ConfigApp").GetSection("ConversionsEndpoint").Value;
                return  await GetRequestAsync<IEnumerable<ConversionModel>>(endpoint, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
