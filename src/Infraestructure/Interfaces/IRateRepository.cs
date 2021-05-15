using Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infraestructure.Interfaces
{
    public interface IRateRepository : IAsyncRepository<RateEntity>
    {
        Task UpdateBackupAsync(IEnumerable<RateEntity> rateEntity, CancellationToken cancellationToken = default);
    }
}
