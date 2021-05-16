using Application.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<TransactionDto>> GetTransactionsBySku(string sku, CancellationToken cancellationToken = default);
    }
}
