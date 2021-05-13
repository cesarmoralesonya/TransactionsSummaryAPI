using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infraestructure.Interfaces
{
    public interface ITransactionClient<T> where T : IWebServiceModel
    {
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
