using ApplicationCore.Entities;

namespace ApplicationCore.Interfaces
{
    public interface ITransactionRepository : IAsyncRepository<TransactionEntity>
    {
    }
}
