
using Application.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITransSummaryService
    {
        Task<TransactionsTotalDto> GetTransactionsWithTotal(string sku, CancellationToken cancellationToken = default);
    }
}
