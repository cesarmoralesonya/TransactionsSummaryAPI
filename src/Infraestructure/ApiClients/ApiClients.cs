using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructure.ApiClients
{
    public class ApiClients : IApiClient<IWebServicesEntity, IWebServicesEntity>
    {
        public ApiClients(IConversionClient<IWebServicesEntity> conversionClient,
            ITransactionClient<IWebServicesEntity> transactionClient)
        {
            ConversionClient = conversionClient;
            TransactionClient = transactionClient;
        }

        public IConversionClient<IWebServicesEntity> ConversionClient { get; }

        public ITransactionClient<IWebServicesEntity> TransactionClient { get; }
    }
}
