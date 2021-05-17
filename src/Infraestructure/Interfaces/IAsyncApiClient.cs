using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infraestructure.Interfaces
{
    public interface IAsyncApiClient<T>
    {
        Task<T> GetRequestAsync(string url, CancellationToken cancellationToken = default);
    }
}
