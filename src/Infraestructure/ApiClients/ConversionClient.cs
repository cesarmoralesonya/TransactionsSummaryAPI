using Infraestructure.Interfaces;
using Infraestructure.Models;
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
        public ConversionClient(IHttpClientFactory httpClientFactory,
                                    ILogger<ConversionClient> logger) : base(httpClientFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async virtual Task<IEnumerable<ConversionModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                 return  await GetRequestAsync<IEnumerable<ConversionModel>>("rates.json", cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
