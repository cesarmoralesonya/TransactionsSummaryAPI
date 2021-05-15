using Domain.Entities;
using Infraestructure.Interfaces;

namespace Infraestructure.Data.Repositories
{
    public class RateRepository : EfRepository<RateEntity>, IRateRepository
    {
        public RateRepository(TransSummaryContext dbcontext) : base(dbcontext)
        {
        }
    }
}
