using Domain.Entities;
using Infraestructure.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infraestructure.Data.Repositories
{
    public class TransactionRepository : EfRepository<TransactionEntity>, ITransactionRepository
    {
        public TransactionRepository(TransSummaryContext dbcontext) : base(dbcontext)
        {
        }

        public async Task UpdateBackupAsync (IEnumerable<TransactionEntity> transEntity, CancellationToken cancellationToken = default)
        {
            await DeleteAllAsync(cancellationToken);
            await AddRangeAsync(transEntity, cancellationToken);
        }
    }
}
