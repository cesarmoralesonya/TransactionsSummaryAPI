using ApplicationCore.Services.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface ITransactionService
    {
        Task<IEnumerable<TransactionDto>> GetAllTransactionsAsync();
        Task<TransactionsTotalDto> GetTransactionsWithTotal(string sku);
    }
}
