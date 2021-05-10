using ApplicationCore.Entities;
using ApplicationCore.Interfaces;

namespace Infraestructure.Data.Repositories
{
    public class TransactionRepository : EfRepository<TransactionDb>, ITransactionRepository
    {
        public TransactionRepository(TransSummaryContext dbcontext) : base(dbcontext)
        {
        }
    }
}
