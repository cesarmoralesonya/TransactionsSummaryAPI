using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Interfaces
{
    public interface IApiClient<T, U> where T : IWebServicesEntity
                                        where U : IWebServicesEntity
    {
        IConversionClient<T> ConversionClient { get; }
        ITransactionClient<U> TransactionClient { get; }

    }
}
