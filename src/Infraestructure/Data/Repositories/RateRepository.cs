using Domain.Entities;
using Infraestructure.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infraestructure.Data.Repositories
{
    public class RateRepository : EfRepository<RateEntity>, IRateRepository
    {
        public RateRepository(TransSummaryContext dbcontext) : base(dbcontext)
        {
        }

        public async Task UpdateBackupAsync(IEnumerable<RateEntity> rateEntity, CancellationToken cancellationToken = default)
        {
            await DeleteAllAsync(cancellationToken);
            await AddRangeAsync(rateEntity, cancellationToken);
        }

    }
}
