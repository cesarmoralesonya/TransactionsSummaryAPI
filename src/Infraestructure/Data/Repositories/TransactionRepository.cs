using Domain.Entities;
using Infraestructure.Interfaces;

namespace Infraestructure.Data.Repositories
{
    public class TransactionRepository : EfRepository<TransactionEntity>, ITransactionRepository
    {
        public TransactionRepository(TransSummaryContext dbcontext) : base(dbcontext)
        {
        }
    }
}
