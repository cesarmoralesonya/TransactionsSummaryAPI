using Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infraestructure.Interfaces
{
    public interface ITransactionRepository : IAsyncRepository<TransactionEntity>
    {
        Task UpdateBackupAsync(IEnumerable<TransactionEntity> rateEntity, CancellationToken cancellationToken = default);
    }
}
