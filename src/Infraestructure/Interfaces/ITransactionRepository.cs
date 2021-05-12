using Domain.Entities;

namespace Infraestructure.Interfaces
{
    public interface ITransactionRepository : IAsyncRepository<TransactionEntity>
    {
    }
}
