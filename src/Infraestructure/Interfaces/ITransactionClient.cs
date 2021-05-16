using Infraestructure.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infraestructure.Interfaces
{
    public interface ITransactionClient
    {
        Task<IEnumerable<TransactionModel>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TransactionModel>> GetListAsync(string sku, CancellationToken cancellationToken);
    }
}
